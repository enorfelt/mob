using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Commands 
{
    [Command(Name = "status", Description = "Show status of mob session")]
    public class StatusCmd
    {
        private readonly IMobSwitchService mobSwitch;

        public StatusCmd(IMobSwitchService mobSwitch)
        {
            this.mobSwitch = mobSwitch;
        }

        public Task<int> OnExecute()
        {
            this.mobSwitch.Status();
            return Task.FromResult(0);
        }
    }
}