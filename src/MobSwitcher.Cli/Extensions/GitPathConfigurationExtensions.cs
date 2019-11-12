using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MobSwitcher.Cli.Extensions
{
    public static class GitPathConfigurationExtensions
    {
        public static IConfigurationBuilder AddGitPath(this IConfigurationBuilder builder)
        {
            var gitService = new GitService(new CmdShellCmdService(), null);

            var path = Path.Combine(gitService.GitDir, ".mob", "appsettings.json");

            return builder.AddJsonFile(path, true, true);
        }
    }
}
