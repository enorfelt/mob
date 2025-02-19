﻿namespace MobSwitcher.CommandLine.Commands;

public class StatusCommand : Command
{
  public StatusCommand() : base("status", "Show status of mob session")
  {
    this.AddHandler<StatusCommand.Handler>();
  }

  public new class Handler : ICommandHandler
  {
    private readonly IMobSwitchService mobSwitchService;

    public Handler(IMobSwitchService mobSwitchService)
    {
      this.mobSwitchService = mobSwitchService;
    }

    public int Invoke(InvocationContext context)
    {
      mobSwitchService.Status();
      return 0;
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
      mobSwitchService.Status();
      return Task.FromResult(0);
    }
  }
}
