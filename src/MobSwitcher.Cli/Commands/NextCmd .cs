using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Commands 
{
    [Command(new []{"next", "n"}, Description = "Hand over to next typist")]
    public class NextCmd
    {
        private readonly IMobSwitchService mobSwitch;

        public NextCmd(IMobSwitchService mobSwitch)
        {
            this.mobSwitch = mobSwitch;
        }

        public Task<int> OnExecute()
        {
            this.mobSwitch.Next();
            return Task.FromResult(0);
        }
    }
}