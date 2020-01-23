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
    private readonly IToastService toast;
    private readonly ISayService sayService;
    private bool isStopped = false;

    public TimerToastService(IConsole console, IToastService toast, ISayService sayService)
    {
      this.console = console;
      this.toast = toast;
      this.sayService = sayService;
    }

    public void Start(int minutes)
    {
      if (minutes < 1)
      {
        minutes = 0;
      }

      console.CancelKeyPress += Console_CancelKeyPress;

      var maxTickCount = minutes * 60;
      var ticks = 1;
      var progressBar = new ToastProgressBar(maxTickCount);
      progressBar.ClearHistory();
      if (minutes > 0)
      {
        sayService.Say("Mobing in progress... (Ctrl+C exits timer)");
        progressBar.Show();
        while (!isStopped && ticks <= maxTickCount)
        {
          Thread.Sleep(1000);
          progressBar.Tick(ticks++);
        }
      }

      if (!isStopped)
      {
        Thread.Sleep(1000);
        toast.Toast("Time is up!", "mob next OR mob done");
      }
      else
      {
        progressBar.ClearHistory();
      }

      console.CancelKeyPress -= Console_CancelKeyPress;
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      Stop();
    }

    public void Stop()
    {
      isStopped = true;
    }
  }
}
