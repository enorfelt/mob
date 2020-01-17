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
  public class Startup
  {
    private readonly IHostBuilder builder;

    public Startup()
    {
      builder = new HostBuilder()
          .ConfigureServices((hostContext, services) =>
          {
            ConfigureServices(hostContext, services);
          });
    }

    public virtual void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
      services.AddLogging(config =>
      {
        config.ClearProviders();
        config.AddProvider(new SerilogLoggerProvider(Log.Logger));
      });
      services.AddSingleton<IShellCmdService, CmdShellCmdService>();
      services.AddSingletonIfNotExists<IGitService, GitService>();
      services.AddSingleton<IMobSwitchService, MobSwitchService>();
      services.AddSingleton<ITimerService, TimerService>();
      services.AddSingleton<IToastService, ToastService>();
      services.AddSingletonIfNotExists<ISayService>((provider) =>
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
          .AddGitPath(services)
          .Build();
      services.Configure<AppSettings>(configuration);
    }

    public virtual Task<int> Run(string[] args)
    {
      return builder.RunCommandLineApplicationAsync<MobCmd>(args);
    }
  }
}
