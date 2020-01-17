using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Options;
using MobSwitcher.Core;
using MobSwitcher.Core.Services.MobSwitch;
using System.Threading.Tasks;

namespace MobSwitcher.Cli.Commands
{
  [Command(new[] { "next", "n" }, Description = "Hand over to next typist")]
  public class NextCmd
  {
    private readonly IMobSwitchService mobSwitch;
    private readonly IOptions<AppSettings> appSettings;

    public NextCmd(IMobSwitchService mobSwitch, IOptions<AppSettings> appSettings)
    {
      this.mobSwitch = mobSwitch;
      this.appSettings = appSettings;
    }

    [Option(Template = "-s|--Stay", Description = "Stays on mob session branch")]
    public bool Stay { get; }

    public Task<int> OnExecute()
    {
      if (Stay)
      {
        appSettings.Value.UseStayNext = true;
      }

      this.mobSwitch.Next();
      return Task.FromResult(0);
    }
  }
}