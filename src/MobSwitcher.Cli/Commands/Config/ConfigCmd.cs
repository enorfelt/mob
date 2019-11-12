using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobSwitcher.Cli.Commands.Config
{
    [Command(new[] { "config" }, Description = "Get or set configuration values")]
    [Subcommand(
        typeof(ConfigGetCmd),
        typeof(ConfigSetCmd),
        typeof(ConfigResetCmd)
    )]
    public class ConfigCmd
    {

        public Task<int> OnExecute()
        {
            return Task.FromResult(0);
        }
    }
}
