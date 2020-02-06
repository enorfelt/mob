namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
  internal class MobSwitchStartCmd : MobSwitchBaseCmd
  {
    internal MobSwitchStartCmd(MobSwitchService service) : base(service)
    {
    }

    internal override void Run()
    {
      var keepUncommitedChanges = false;
      var nothingToCommit = IsNothingToCommit();
      if (!nothingToCommit)
      {
        keepUncommitedChanges = service.Say.GetYesNo("Keep uncommited changes?", true);
        var stashOrResetCmd = keepUncommitedChanges ? "stash" : "reset --hard";
        Git(stashOrResetCmd);
      }

      Git("fetch --prune");
      Git("pull --ff-only");

      var hasMobbingBranch = HasMobbingBranch();
      var hasMobbingBranchOrigin = HasMobbingBranchOrigin();

      if (hasMobbingBranch && hasMobbingBranchOrigin)
      {
        service.Say.SayInfo("rejoining mob session");
        if (!base.IsMobbing())
        {
          Git($"branch -D {WIP_BRANCH}");
          Git($"checkout {WIP_BRANCH}");
          Git($"branch --set-upstream-to={REMOTE_NAME}/{WIP_BRANCH} {WIP_BRANCH}");
        }
      }
      else if (!hasMobbingBranch && !hasMobbingBranchOrigin)
      {
        service.Say.SayInfo($"create {WIP_BRANCH} from {BASE_BRANCH}");
        Git($"checkout {BASE_BRANCH}");
        Git($"merge {REMOTE_NAME}/{BASE_BRANCH} --ff-only");
        Git($"branch {WIP_BRANCH}");
        Git($"checkout {WIP_BRANCH}");
        Git($"push --set-upstream {REMOTE_NAME} {WIP_BRANCH}");
      }
      else if (!hasMobbingBranch && hasMobbingBranchOrigin)
      {
        service.Say.SayInfo("joining mob session");
        Git($"checkout {WIP_BRANCH}");
        Git($"branch --set-upstream-to={REMOTE_NAME}/{WIP_BRANCH} {WIP_BRANCH}");
      }
      else
      {
        service.Say.SayInfo($"purging local branch and start new {WIP_BRANCH} branch from {BASE_BRANCH}");
        Git($"branch -D {WIP_BRANCH}");
        Git($"checkout {BASE_BRANCH}");
        Git($"merge {REMOTE_NAME}/{BASE_BRANCH} --ff-only");
        Git($"branch {WIP_BRANCH}");
        Git($"checkout {WIP_BRANCH}");
        Git($"push --set-upstream {REMOTE_NAME} {WIP_BRANCH}");
      }

      if (keepUncommitedChanges)
      {
        Git("stash pop");
      }
    }
  }
}
