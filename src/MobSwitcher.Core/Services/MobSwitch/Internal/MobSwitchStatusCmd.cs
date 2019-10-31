using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
    internal class MobSwitchStatusCmd : MobSwitchBaseCmd
    {
        internal MobSwitchStatusCmd(MobSwitchService service)
            : base(service)
        {
        }

        internal override void Run()
        {
            if (IsMobbing())
            {
                service.Say.SayInfo("mobbing in progress");
                var output = Git($"--no-pager log {BASE_BRANCH}..{WIP_BRANCH} --pretty=\"format: %h %cr <%an>\" --abbrev-commit");
                service.Say.Say(output);
            }
            else
            {
                service.Say.SayInfo("you aren't mobbing right now");
            }
        }
    }
}