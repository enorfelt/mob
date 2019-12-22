using System;
using System.Collections.Generic;
using System.Text;
using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal {
  internal abstract class MobSwitchBaseCmd {
    internal readonly string WIP_BRANCH = "mob-session";
    internal readonly string REMOTE_NAME = "origin";
    internal readonly string BASE_BRANCH = "master";
    internal readonly string WIP_COMMIT_MESSAGE = "Mob Session DONE[ci - skip]";

    internal readonly MobSwitchService service;

    internal MobSwitchBaseCmd (MobSwitchService service) {
      this.service = service;
      var appSettings = service.AppSettings.Value;
      WIP_BRANCH = appSettings.WipBranch;
      REMOTE_NAME = appSettings.RemoteName;
      BASE_BRANCH = appSettings.BaseBranch;
      WIP_COMMIT_MESSAGE = appSettings.WipCommitMessage;
    }

    internal abstract void Run ();

    internal bool IsMobbing () {
      var result = Git ("branch", true);
      return result.Contains ($"* {WIP_BRANCH}", StringComparison.InvariantCulture);
    }

    internal bool IsNothingToCommit () {
      return Git ("status --short", true)?.Trim ().Length == 0;
    }

    internal string GetChangesOfLastCommit () {
      return Git ("diff HEAD^1 --stat");
    }

    internal bool HasMobbingBranch () {
      var result = Git ("branch", true);
      return result.Contains ($"  {WIP_BRANCH}", StringComparison.InvariantCulture) || result.Contains ($"* {WIP_BRANCH}", StringComparison.InvariantCulture);
    }

    internal bool HasMobbingBranchOrigin () {
      var result = Git ("branch --remotes", true);
      return result.Contains ($"  {REMOTE_NAME}/{WIP_BRANCH}", StringComparison.InvariantCulture);
    }

    internal string GetCachedChanges () {
      return Git ("diff --cached --stat", true)?.Trim ();
    }

    internal bool IsLastChangeSecondsAgo () {
      var changes = Git ($"--no-pager log {BASE_BRANCH}..{WIP_BRANCH} --pretty=\"format:%cr\" --abbrev-commit", true);
      var lines = changes.Replace ("\\r\\n", "\\n", StringComparison.InvariantCulture).Split ("\\n");
      var numberOfLines = lines.Length;
      if (numberOfLines < 1) {
        return true;
      }

      return lines[0].Contains ("seconds ago", StringComparison.InvariantCulture) || lines[0].Contains ("second ago", StringComparison.InvariantCulture);
    }

    internal string Git (string args, bool silent = false) {
      return this.service.GitService.Git (args, silent);
    }
  }
}