﻿namespace MobSwitcher.CommandLine.Extensions;
public static class GitPathConfigurationExtensions
{
  public static IConfigurationBuilder AddGitPath(this IConfigurationBuilder builder, IServiceCollection services)
  {
    var gitServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IGitService));

    if (gitServiceDescriptor == null)
      return builder;

    using var sp = services.BuildServiceProvider();
    var gitService = sp.GetService<IGitService>();

    if (gitService == null)
      return builder;

    var path = Path.Combine(gitService.GitDir, ".mob", "appsettings.json");

    return builder.AddJsonFile(path, true, true);
  }
}
