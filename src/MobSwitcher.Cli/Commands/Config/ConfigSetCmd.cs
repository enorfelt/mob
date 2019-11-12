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
    [Command(new[] { "set" }, Description = "Set configuration value")]
    public class ConfigSetCmd
    {
        private readonly IConsole console;
        private readonly IOptions<AppSettings> appSettings;
        private readonly IGitService gitService;

        public ConfigSetCmd(IConsole console, IOptions<AppSettings> appSettings, IGitService gitService)
        {
            this.console = console;
            this.appSettings = appSettings;
            this.gitService = gitService;
        }

        [Argument(0, Description = "Config key", Name = "Key", ShowInHelpText = true)]
        [Required]
        public string Key { get; set; }

        [Argument(1, Description = "Config value", Name = "Value", ShowInHelpText = true)]
        [Required]
        public string Value { get; set; }

        public Task<int> OnExecute()
        {
            var type = typeof(AppSettings);
            var property = type.GetProperty(Key);
            if (property == null)
                return Task.FromResult(0);

            var propertyType = property.PropertyType;
            if (propertyType == typeof(bool))
            {
                if (bool.TryParse(Value, out var result))
                {
                    property.SetValue(appSettings.Value, result);
                }
            }
            else
            {
                property.SetValue(appSettings.Value, Value);
            }

            if (gitService.IsInsideWorkingTree) {
                var directoryPath = Path.Combine(gitService.GitDir, ".mob");
                var filePath = Path.Combine(directoryPath, "appsettings.json");
                if (!Directory.Exists(directoryPath))
                { 
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, JsonConvert.SerializeObject(appSettings.Value));
            }

            return Task.FromResult(0);
        }
    }
}
