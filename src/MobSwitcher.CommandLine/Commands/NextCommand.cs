namespace MobSwitcher.CommandLine.Commands;

public class NextCommand : Command
{
  public NextCommand(IOptions<AppSettings> appSettings) : base("next", "Hand over to next typist")
  {
    AddAlias("n");
    // AddOption(new Option<bool>(
    //   new[] { "-s", "--stay" },
    //   () => appSettings.Value.UseStayNext,
    //   $"Stays on mob session branch (default {appSettings.Value.UseStayNext})"));

    this.AddHandler<NextCommand.Handler>();
  }

  public new class Handler : ICommandHandler
  {
    private readonly IMobSwitchService mobSwitchService;

    public Handler(IMobSwitchService mobSwitchService)
    {
      this.mobSwitchService = mobSwitchService;
    }

    public bool Stay { get; set; }

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
      mobSwitchService.Next(Stay);
    }
  }
}