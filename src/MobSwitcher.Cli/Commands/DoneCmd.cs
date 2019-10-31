using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Commands 
{
    [Command(new []{"done", "d"}, Description = "Finish mob session")]
    public class DoneCmd
    {
        private readonly IMobSwitchService mobSwitch;

        public DoneCmd(IMobSwitchService mobSwitch)
        {
            this.mobSwitch = mobSwitch;
        }

        public Task<int> OnExecute()
        {
            this.mobSwitch.Done();
            return Task.FromResult(0);
        }
    }
}