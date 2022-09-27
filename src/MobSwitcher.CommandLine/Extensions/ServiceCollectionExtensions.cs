namespace MobSwitcher.CommandLine.Extensions;
public static class ServiceCollectionExtensions
{
  public static void AddSingletonIfNotExists<TService, TImplementation>(this IServiceCollection services)
      where TService : class
      where TImplementation : class, TService
  {
    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
    if (descriptor != null)
      return;

    services.AddSingleton<TService, TImplementation>();
  }

  public static void AddSingletonIfNotExists<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
  {
    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));
    if (descriptor != null)
      return;

    services.AddSingleton(implementationFactory);
  }

  public static IServiceCollection AddCliCommands(this IServiceCollection services)
  {
    Type mobCommandType = typeof(StatusCommand);
    Type commandType = typeof(Command);

    IEnumerable<Type> commands = mobCommandType
        .Assembly
        .GetExportedTypes()
        .Where(x => x.Namespace == mobCommandType.Namespace && commandType.IsAssignableFrom(x));

    foreach (Type command in commands)
    {
      services.AddSingleton(commandType, command);
    }

    // services.AddSingleton(sp =>
    // {
    //     return
    //        sp.GetRequiredService<IConfiguration>().GetSection("Deployment").Get<DeploymentOptions>()
    //        ?? throw new ArgumentException("Deployment configuration cannot be missing.");
    // });

    return services;
  }

  public static void AddCommandsToRootCommand(this IServiceCollection services, Command rootCommand)
  {

    // Type mobCommandType = typeof(StatusCommand);
    // Type commandType = typeof(Command);

    // IEnumerable<Type> commands = mobCommandType
    //     .Assembly
    //     .GetExportedTypes()
    //     .Where(x => x.Namespace == mobCommandType.Namespace && commandType.IsAssignableFrom(x));

    using var scope = services.BuildServiceProvider().CreateScope();
    var commands = scope.ServiceProvider.GetServices<Command>();
    foreach (var command in commands)
    {
      rootCommand.AddCommand(command);
    }



  }
}
