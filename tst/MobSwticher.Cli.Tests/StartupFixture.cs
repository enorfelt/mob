using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MobSwitcher.Cli.Tests.Fakes;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Cli.Tests {
  public class StartupFixture : Startup {
    public FakeShellCmdService FakeShellCmdService;
    public FakeSayService FakeSayService;
    public StartupFixture () : base () {
      FakeShellCmdService = new FakeShellCmdService();
      FakeSayService = new FakeSayService();
    }

    public override void ConfigureServices (HostBuilderContext hostContext, IServiceCollection services) {
      services.AddSingleton<ISayService> (services => {
        return FakeSayService;
      });
      services.AddSingleton<IGitService> (services => {
        return new GitService (FakeShellCmdService, services.GetService<ISayService> ());
      });

      base.ConfigureServices (hostContext, services);
    }

    protected readonly object lockObject = new object ();
    public override Task<int> Run (string[] args) {
      lock (lockObject) {
        return base.Run (args);
      }
    }

    public Task<int> Run (string cmd) {
      return Run (new [] { cmd });
    }
  }
}