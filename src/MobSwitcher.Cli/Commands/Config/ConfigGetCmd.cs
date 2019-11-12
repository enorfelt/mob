using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;
using MobSwitcher.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobSwitcher.Cli.Commands.Config
{
    [Command(new[] { "get" }, Description = "Get configuration value(s)")]
    public class ConfigGetCmd
    {
        private readonly IConsole console;
        private readonly IOptions<AppSettings> appSettings;

        public ConfigGetCmd(IConsole console, IOptions<AppSettings> appSettings)
        {
            this.console = console;
            this.appSettings = appSettings;
        }

        [Argument(0, Description = "Config key", Name = "Key", ShowInHelpText = true)]
        public string Key { get; set; }

        public Task<int> OnExecute()
        {
            if (string.IsNullOrEmpty(Key))
            {
                console.WriteLine(appSettings.Value);
                return Task.FromResult(0);
            }

            var type = typeof(AppSettings);
            var property = type.GetProperty(Key);
            if (property != null)
                console.WriteLine($"{property.Name} = {property.GetValue(appSettings.Value)}");

            return Task.FromResult(0);
        }
    }
}
