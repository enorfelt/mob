using MobSwitcher.Core.Services;
using System;

namespace MobSwitcher.Windows
{
  public class TimerToastService : ITimerService
  {
    private readonly ISayService sayService;
    private ToastProgressBar toastProgressBar;

    public TimerToastService(ISayService sayService)
    {
      this.sayService = sayService;
    }

    public void Start(double minutes)
    {
      if (minutes < 0)
      {
        minutes = 0;
      }
      Console.CancelKeyPress += Console_CancelKeyPress;

      var durationInSeconds = Convert.ToInt32(minutes * 60);
      toastProgressBar = new ToastProgressBar(sayService, durationInSeconds);
      sayService.Say("Mobing in progress... (Ctrl+C exits timer)");
      toastProgressBar.Start();
      
      Console.CancelKeyPress -= Console_CancelKeyPress;
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      toastProgressBar?.Stop();
    }
  }
}
