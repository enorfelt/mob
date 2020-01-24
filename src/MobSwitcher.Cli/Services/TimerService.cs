using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;
using ShellProgressBar;
using System;
using System.Threading;

namespace MobSwitcher.Cli.Services
{
  public class TimerService : ITimerService
  {
    private readonly IConsole console;
    private readonly IToastService toast;
    private bool isStopped = false;

    public TimerService(IConsole console, IToastService toast)
    {
      this.console = console;
      this.toast = toast;
    }

    public void Start(double minutes)
    {
      if (minutes < 1)
      {
        minutes = 0;
      }

      console.CancelKeyPress += Console_CancelKeyPress;

      var options = new ProgressBarOptions
      {
        ForegroundColor = ConsoleColor.Yellow,
        ForegroundColorDone = ConsoleColor.DarkGreen,
        BackgroundColor = ConsoleColor.DarkGray,
        BackgroundCharacter = '\u2593',
        EnableTaskBarProgress = true
      };


      var maxTickCount = minutes * 60;
      var ticks = 1;
      using var pbar = new ProgressBar(Convert.ToInt32(maxTickCount), $"typist time completed out of {minutes} min.", options);
      while (!isStopped && ticks <= maxTickCount)
      {
        Thread.Sleep(1000);
        pbar.Tick(ticks++);
      }

      if (!isStopped)
      {
        toast.Toast("Time is up!", "mob next OR mob done");
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
