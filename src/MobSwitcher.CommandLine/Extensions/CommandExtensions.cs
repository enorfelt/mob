namespace MobSwitcher.CommandLine.Extensions;

public static class CommandExtensions
{
  public static void AddHandler<THandler>(this Command command)
  {
    CommandHandler.Create(typeof(THandler).GetMethod(nameof(ICommandHandler.InvokeAsync)));
  }
}