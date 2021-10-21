namespace MobSwitcher.CommandLine
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MobSwitcher.CommandLine.Commands;
    using MobSwitcher.CommandLine.Services;
    using MobSwitcher.Core.Services;
    using MobSwitcher.Core.Services.Git;
    using MobSwitcher.Core.Services.MobSwitch;
    using MobSwitcher.Core.Services.Shell;
    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Hosting;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.Threading.Tasks;

    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var runner = BuildCommandLine()
                .UseHost(_ => CreateHostBuilder(args), (hostBuilder) => hostBuilder
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<IShellCmdService, CmdShellCmdService>();
                        services.AddSingleton<IGitService, GitService>();
                        services.AddSingleton<IMobSwitchService, MobSwitchService>();
                        services.AddSingleton<ISayService, SayPrettyService>();
                    })
                    .UseCommandHandler<StatusCommand, StatusCommand.Handler>()
                    ).UseDefaults().Build();

            return await runner.InvokeAsync(args);
        }

        static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);

        static CommandLineBuilder BuildCommandLine()
        {
            var root = new RootCommand();
            root.Name = "mob";

            root.AddCommand(new StatusCommand());

            root.Handler = CommandHandler.Create(() => root.Invoke("-h"));

            return new CommandLineBuilder(root);
        }
    }
}
