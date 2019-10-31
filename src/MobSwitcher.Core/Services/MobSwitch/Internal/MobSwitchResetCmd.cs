using System;
using System.Collections.Generic;
using System.Text;
using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
    internal class MobSwitchResetCmd : MobSwitchBaseCmd
    {
        public MobSwitchResetCmd(MobSwitchService service) : base(service)
        { }

        internal override void Run()
        {
            Git("fetch --prune");
            Git($"checkout {BASE_BRANCH}");

            if (HasMobbingBranch())
                Git($"branch -D {WIP_BRANCH}");

            if (HasMobbingBranchOrigin())
                Git($"push {REMOTE_NAME} --delete {WIP_BRANCH}");
        }
    }
}
