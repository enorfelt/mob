namespace MobSwitcher.CommandLine.Commands;

public class StartCommand : Command
{
  public StartCommand() : base("start", "Show status of mob session")
  {
    this.AddAlias("s");
    this.AddArgument(new Argument<int>(
      "Time",
      () => 15,
      "Time in minuts (default 15)"));

    this.AddHandler<StartCommand.Handler>();
  }

  public new class Handler : ICommandHandler
  {
    private readonly IMobSwitchService mobSwitchService;
    private readonly ITimerService timer;

    public int Time { get; set; }

    public Handler(IMobSwitchService mobSwitchService, ITimerService timer)
    {
      this.mobSwitchService = mobSwitchService;
      this.timer = timer;
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
      mobSwitchService.Start();

      if (Time > 0)
      {
        timer.Start(Time);
      }
    }
  }
}
