using System;
using MobSwitcher.Core.Services.Git;

namespace MobSwitcher.Core.Services.MobSwitch.Internal
{
    internal class MobSwitchNextCmd : MobSwitchBaseCmd
    {
        internal MobSwitchNextCmd(MobSwitchService service)
            : base(service)
        {;
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
            }

            ShowNext();

            Git($"checkout {BASE_BRANCH}");
        }

        private void ShowNext()
        {
            var changes = Git($"--no-pager log {BASE_BRANCH}..{WIP_BRANCH} --pretty=\"format:%an\" --abbrev-commit")?.Trim();
            var lines = changes.Replace("\\r\\n", "\\n", StringComparison.InvariantCulture).Split("\\n");
            var numberOfLines = lines.Length;

            var gitUserName = GetGitUserName();

            if (numberOfLines < 1)
                return;

            var history = string.Empty;
            for (var i = 0; i < numberOfLines; i++)
            {
                if (lines[i].Equals(gitUserName, StringComparison.InvariantCultureIgnoreCase) && i > 0)
                {
                    service.Say.SayInfo($"Committers after your last commit: {history}");
                    service.Say.SayInfo($"***{lines[i - 1]}*** is (probably next.");
                    return;
                }
                if (!string.IsNullOrEmpty(history))
                {
                    history = $", {history}";
                }
                history = $"{lines[i]}{history}";
            }
        }

        private string GetGitUserName()
        {
            return Git("config --get user.name", true)?.Trim();
        }
    }
}