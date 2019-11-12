using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MobSwitcher.Cli.Commands;
using MobSwitcher.Cli.Extensions;
using MobSwitcher.Cli.Services;
using MobSwitcher.Core;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;
using MobSwitcher.Windows;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MobSwitcher.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration);
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(config =>
                    {
                        config.ClearProviders();
                        config.AddProvider(new SerilogLoggerProvider(Log.Logger));
                    });
                    services.AddSingleton<IShellCmdService, CmdShellCmdService>();
                    services.AddSingleton<IGitService, GitService>();
                    services.AddSingleton<IMobSwitchService, MobSwitchService>();
                    services.AddSingleton<ITimerService, TimerService>();
                    services.AddSingleton<IToastService, ToastService>();
                    services.AddSingleton<ISayService>((provider) =>
                    {
                        var appSettings = provider.GetService<IOptions<AppSettings>>();
                        if (appSettings.Value.UsePrettyPrint)
                            return new SayPrettyService(provider.GetService<IConsole>());
                        return new SayService(provider.GetService<IConsole>());
                    });


                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables("MOBSWTICHER_")
                        .AddGitPath()
                        .Build();
                    services.Configure<AppSettings>(configuration);
                });

            try
            {
                return await builder.RunCommandLineApplicationAsync<MobCmd>(args).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error");
                throw;
            }
        }
    }
}
