using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Commands 
{
    [Command(new []{"timer", "t"}, Description = "Creates a new timer in minutes")]
    public class TimerCmd
    {
        private readonly ITimerService timer;
        
        [Argument(0, Description = "Time in minutes", Name = "Time", ShowInHelpText = true)]
        [Required]
        public int Time { get; set; }

        public TimerCmd(ITimerService timer)
        {
            this.timer = timer;
        }

        public Task<int> OnExecute()
        {
            timer.Start(Time);
            return Task.FromResult(0);
        }
    }
}