namespace MobSwitcher.CommandLine;
internal class Program
{
  static async Task<int> Main(string[] args)
  {
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .CreateLogger();

    var serviceProvider = BuildServiceProvider();

    var runner = BuildCommandLine(serviceProvider)
      .UseDefaults()
      .Build();

    return await runner.InvokeAsync(args);
  }

  static CommandLineBuilder BuildCommandLine(ServiceProvider serviceProvider)
  {
    var root = new RootCommand();
    root.Name = "mob";
    root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

    foreach (var command in serviceProvider.GetServices<Command>())
    {
      root.AddCommand(command);
    }

    return new CommandLineBuilder(root);
  }

  private static ServiceProvider BuildServiceProvider()
  {
    var services = new ServiceCollection();

    services.AddLogging(builder => builder.AddSerilog(Log.Logger));

    services.AddSingleton<IShellCmdService, CmdShellCmdService>();
    services.AddSingleton<IGitService, GitService>();
    services.AddSingleton<IMobSwitchService, MobSwitchService>();
    services.AddSingleton<ITimerService, TimerToastService>();
    services.AddSingleton<IToastService, ToastService>();
    services.AddSingleton<ISayService, SayPrettyService>();

    services.AddCliCommands();

    var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables("MOBSWTICHER_")
            .AddEnvironmentVariables("MOBSWITCHER_")
            .AddGitPath(services)
            .Build();

    services.Configure<AppSettings>(configuration);

    return services.BuildServiceProvider();
  }
}
