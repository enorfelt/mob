using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MobSwitcher.Windows
{
  public class TimerToastService : ITimerService
  {
    private readonly IConsole console;
    private readonly ISayService sayService;
    private ToastProgressBar toastProgressBar;

    public TimerToastService(IConsole console, ISayService sayService)
    {
      this.console = console;
      this.sayService = sayService;
    }

    public void Start(int minutes)
    {
      if (minutes < 1)
      {
        minutes = 0;
      }

      console.CancelKeyPress += Console_CancelKeyPress;

      var durationInSeconds = minutes * 60;
      toastProgressBar = new ToastProgressBar(durationInSeconds);
      sayService.Say("Mobing in progress... (Ctrl+C exits timer)");
      toastProgressBar.Start();

      console.CancelKeyPress -= Console_CancelKeyPress;
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      toastProgressBar?.Stop();
    }
  }
}
