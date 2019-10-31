using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Commands 
{
    [Command(new []{"join", "j"}, Description = "Like start but waits for recent commit")]
    public class JoinCmd
    {
        private readonly IMobSwitchService mobSwitch;

        public JoinCmd(IMobSwitchService mobSwitch)
        {
            this.mobSwitch = mobSwitch;
        }

        public Task<int> OnExecute()
        {
            this.mobSwitch.Join();
            return Task.FromResult(0);
        }
    }
}