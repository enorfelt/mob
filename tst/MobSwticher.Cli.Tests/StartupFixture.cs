using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MobSwitcher.Cli;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.Shell;
using MobSwticher.Cli.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobSwticher.Cli.Tests
{
    public class StartupFixture : Startup
    {
        public FakeShellCmdService FakeShellCmdService;
        public FakeSayService FakeSayService = new FakeSayService();
        public StartupFixture()
            : base()
        {
            
        }


        public override void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<ISayService>(services =>
            {
                return FakeSayService;
            });
            services.AddSingleton<IGitService>(services =>
            {
                return new GitService(FakeShellCmdService, services.GetService<ISayService>());
            });

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
    }
}
