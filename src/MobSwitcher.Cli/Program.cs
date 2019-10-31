using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.Hosting.CommandLine;
using MobSwitcher.Cli.Commands;
using MobSwitcher.Core.Services.Shell;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services;
using MobSwitcher.Windows;
using MobSwitcher.Cli.Services;
using MobSwitcher.Core;
using Microsoft.Extensions.Options;

namespace MobSwitcher.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("MOBSWTICHER_")
                .Build();

            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration);
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.Configure<AppSettings>(configuration);
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
                });

            try 
            {
                return await builder.RunCommandLineApplicationAsync<MobCmd>(args).ConfigureAwait(true);
            }
            catch(Exception ex)
            {
                Log.Logger.Error(ex, "Error");
                throw;
            }
        }
    }
}
