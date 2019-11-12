using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;
using MobSwitcher.Core;
using MobSwitcher.Core.Services.Git;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MobSwitcher.Cli.Commands.Config
{
    [Command(new[] { "reset" }, Description = "Resets all configuration values")]
    public class ConfigResetCmd
    {
        private readonly IGitService gitService;

        public ConfigResetCmd(IGitService gitService)
        {
            this.gitService = gitService;
        }

        public Task<int> OnExecute()
        {

            if (gitService.IsInsideWorkingTree) {
                var directoryPath = Path.Combine(gitService.GitDir, ".mob");
                var filePath = Path.Combine(directoryPath, "appsettings.json");
                if (Directory.Exists(directoryPath))
                {
                    File.Delete(filePath);
                }
                
            }

            return Task.FromResult(0);
        }
    }
}
