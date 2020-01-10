using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services.MobSwitch;

namespace MobSwitcher.Cli.Commands
{
  [Command(new[] { "next", "n" }, Description = "Hand over to next typist")]
  public class NextCmd
  {
    private readonly IMobSwitchService mobSwitch;

    public NextCmd(IMobSwitchService mobSwitch)
    {
      this.mobSwitch = mobSwitch;
    }

    [Option("-s|--Stay", CommandOptionType.NoValue)]
    public (bool hasValue, bool value) Stay { get; }

    public Task<int> OnExecute()
    {
      this.mobSwitch.Next();
      return Task.FromResult(0);
    }
  }
}