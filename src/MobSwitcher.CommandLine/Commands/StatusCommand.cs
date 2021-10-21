namespace MobSwitcher.CommandLine.Commands
{
    using MobSwitcher.Core.Services.MobSwitch;
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatusCommand : Command
    {
        public StatusCommand() : base("status", "Show status of mob session")
        {
            
        }

        public new class Handler : ICommandHandler
        {
            private readonly IMobSwitchService mobSwitchService;

            public Handler(IMobSwitchService mobSwitchService)
            {
                this.mobSwitchService = mobSwitchService;
            }

            public Task<int> InvokeAsync(InvocationContext context)
            {
                mobSwitchService.Status();
                return Task.FromResult(0);
            }
        }
    }
}
