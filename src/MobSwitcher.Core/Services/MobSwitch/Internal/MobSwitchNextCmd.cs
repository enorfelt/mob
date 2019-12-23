using System;
using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
  internal class MobSwitchNextCmd : MobSwitchBaseCmd
  {
    internal MobSwitchNextCmd(MobSwitchService service) : base(service)
    {
      ;
    }

    internal override void Run()
    {
      if (!IsMobbing())
      {
        service.Say.SayError("you aren't mobbing");
        return;
      }

      if (IsNothingToCommit())
      {
        service.Say.SayInfo("nothing was done, so nothing to commit");
      }
      else
      {
        Git("add --all");
        Git($"commit --message \"{WIP_COMMIT_MESSAGE}\"");
        var changes = base.GetChangesOfLastCommit();
        Git($"push {REMOTE_NAME} {WIP_BRANCH}");
        service.Say.Say(changes);
        if (base.service.AppSettings.Value.UsePullRequest)
          service.Say.SayTodo("Remember to create pull request");
      }

      ShowNext();

      Git($"checkout {BASE_BRANCH}");
    }

    private void ShowNext()
    {
      var changesByUsers = Git($"--no-pager log {BASE_BRANCH}..{WIP_BRANCH} --pretty=\"format:%an\" --abbrev-commit")?.Trim();
      var users = changesByUsers.Replace("\r\n", "\n", StringComparison.InvariantCulture).Split("\n");
      var numberOfUsers = users.Length;

      var currentUser = GetGitUserName();

      if (numberOfUsers < 1)
        return;

      var history = string.Empty;
      for (var i = 0; i < numberOfUsers; i++)
      {
        if (users[i].Equals(currentUser, StringComparison.InvariantCultureIgnoreCase) && i > 0)
        {
          SayNextInfo(history, users[i - 1]);
          return;
        }
        if (!string.IsNullOrEmpty(history))
        {
          history = ", " + history;
        }
        history = users[i] + history;
      }
      SayNextInfo(history, users[numberOfUsers - 1]);
    }

    private void SayNextInfo(string history, string nextUser)
    {
      service.Say.SayInfo($"Committers after your last commit: {history}");
      service.Say.SayInfo($"***{nextUser}*** is (probably) next.");
    }

    private string GetGitUserName()
    {
      return Git("config --get user.name", true)?.Trim();
    }
  }
}