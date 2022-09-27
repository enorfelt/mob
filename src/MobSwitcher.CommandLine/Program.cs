﻿namespace MobSwitcher.CommandLine;
internal class Program
{
  static async Task<int> Main(string[] args)
  {
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .CreateLogger();

    Console.WriteLine($"Current dir {Directory.GetCurrentDirectory()}");

    var builder = BuildCommandLine();
    var runner = builder
        .UseHost(_ => CreateHostBuilder(args), (hostBuilder) => hostBuilder
            .ConfigureServices((hostContext, services) =>
            {
              services.AddSingleton<IShellCmdService, CmdShellCmdService>();
              services.AddSingleton<IGitService, GitService>();
              services.AddSingleton<IMobSwitchService, MobSwitchService>();
              services.AddSingleton<ITimerService, TimerToastService>();
              services.AddSingleton<IToastService, ToastService>();
              services.AddSingleton<ISayService, SayPrettyService>();

              services.AddCliCommands();
              services.AddCommandsToRootCommand(builder.Command);

              var configuration = new ConfigurationBuilder()
                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables("MOBSWTICHER_")
                      .AddEnvironmentVariables("MOBSWITCHER_")
                      .AddGitPath(services)
                      .Build();

              services.Configure<AppSettings>(configuration);
            })
            .UseSerilog()
            .UseCommandHandler<StatusCommand, StatusCommand.Handler>()
            )
            .UseDefaults()
            .Build();

    

    return await runner.InvokeAsync(args);
  }

  static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);

  static CommandLineBuilder BuildCommandLine()
  {
    var root = new RootCommand();
    root.Name = "mob";

    // root.AddCommand(new StatusCommand());

    root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

    return new CommandLineBuilder(root);
  }
}
