using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
    internal class MobSwitchDoneCmd : MobSwitchBaseCmd
    {
        internal MobSwitchDoneCmd(MobSwitchService service)
            : base (service)
        { }
        
        internal override void Run()
        {
            if (!base.IsMobbing())
            {
                service.Say.SayError("you aren't mobbing");
                return;
            }

            Git("fetch --prune");

            if (HasMobbingBranchOrigin())
            {
                if (IsNothingToCommit())
                {
                    Git("add --all");
                    Git($"commit --message \"{WIP_COMMIT_MESSAGE}\"");
                }

                Git($"push {REMOTE_NAME} {WIP_BRANCH}");

                Git($"checkout {BASE_BRANCH}");
                Git($"merge {REMOTE_NAME}/{BASE_BRANCH} --ff-only");
                Git($"merge --squash {WIP_BRANCH}");

                Git($"branch -D {WIP_BRANCH}");
                Git($"push {REMOTE_NAME} --delete {WIP_BRANCH}");
                service.Say.Say(base.GetCachedChanges());
                service.Say.SayTodo("git commit -m 'describe the changes'");
            }
            else
            {
                Git($"checkout {BASE_BRANCH}");
                Git($"branch -D {WIP_BRANCH}");
                service.Say.SayInfo("someone else already ended your mob session");
            }
        }
    }
}