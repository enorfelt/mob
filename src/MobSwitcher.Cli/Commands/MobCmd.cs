
using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Cli.Commands.Config;

namespace MobSwitcher.Cli.Commands
{
    [Command(Name = "mob", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue, OptionsComparison = System.StringComparison.InvariantCultureIgnoreCase )]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(StartCmd),
        typeof(JoinCmd),
        typeof(DoneCmd),
        typeof(NextCmd),
        typeof(ResetCmd),
        typeof(StatusCmd),
        typeof(TimerCmd),
        typeof(ConfigCmd)
    )]
    public class MobCmd
    {

        public Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app?.ShowHelp();
            return Task.FromResult(0);
        }

        private static string GetVersion() => typeof(MobCmd).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}