namespace MobSwitcher.CommandLine.Commands;

public class TimerCommand : Command
{
  public TimerCommand() : base("timer", "Creates a new timer in minutes (default 15)")
  {
    AddAlias("t");
    AddArgument(new Argument<int>(
      "Time",
      () => 15,
      "Time in minuts (default 15)"));
  }

  public new class Handler : ICommandHandler
  {
    private readonly ITimerService timerService;

    public int Time { get; set; }

    public Handler(ITimerService timerService)
    {
      this.timerService = timerService;
    }

    public int Invoke(InvocationContext context)
    {
      OnInvoke();
      return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
      OnInvoke();
      return Task.FromResult(0);
    }

    private void OnInvoke()
    {
      if (Time > 0)
      {
        timerService.Start(Time);
      }
    }
  }
}