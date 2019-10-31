using MobSwitcher.Core.Services.Git;
using System.Threading;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
    internal class MobSwitchJoinCmd : MobSwitchBaseCmd
    {
        internal MobSwitchJoinCmd(MobSwitchService service)
            : base(service)
        {
        }

        internal override void Run()
        {
            if (!IsLastChangeSecondsAgo())
                service.Say.SayInfo("Actively waiting for new remote commit...");

            while(!IsLastChangeSecondsAgo())
            {
                Thread.Sleep(1000);
                Git("pull");
            }
                
        }
    }
}