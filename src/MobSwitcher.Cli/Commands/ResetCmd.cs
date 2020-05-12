using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.MobSwitch;

namespace MobSwitcher.Cli.Commands 
{
    [Command(new []{"reset", "r"}, Description = "Resets any unfinished mob session")]
    public class ResetCmd
    {
        private readonly IMobSwitchService mobSwitch;

        public ResetCmd(IMobSwitchService mobSwitch)
        {
            this.mobSwitch = mobSwitch;
        }

        public Task<int> OnExecute()
        {
            this.mobSwitch.Reset();
            return Task.FromResult(0);
        }
    }
}