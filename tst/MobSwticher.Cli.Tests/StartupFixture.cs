using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MobSwitcher.Cli.Tests.Fakes;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Tests
{
  public class StartupFixture : Startup
  {
    public FakeSayService FakeSayService = new FakeSayService();
    public FakeShellCmdService FakeShellCmdService = new FakeShellCmdService();
    
    public StartupFixture() : base()
    {
      Environment.SetEnvironmentVariable("MOBSWITCHER_WipBranch", "mob-session");
    }

    public override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
      services.AddSingleton<ISayService>(FakeSayService);
      services.AddSingleton<IShellCmdService>(FakeShellCmdService);

      base.ConfigureServices(hostContext, services);

    }

    protected readonly object lockObject = new object();
    public override Task<int> Run(string[] args)
    {
      lock (lockObject)
      {
        return base.Run(args);
      }
    }

    public Task<int> Run(string cmd)
    {
      return Run(new[] { cmd });
    }
  }
}