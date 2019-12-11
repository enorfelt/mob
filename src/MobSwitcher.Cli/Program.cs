using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MobSwitcher.Cli.Commands;
using MobSwitcher.Cli.Extensions;
using MobSwitcher.Cli.Services;
using MobSwitcher.Core;
using MobSwitcher.Core.Services;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch;
using MobSwitcher.Core.Services.Shell;
using MobSwitcher.Windows;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MobSwitcher.Cli
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration);
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                return await new Startup().Run(args).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error");
                throw;
            }
        }
    }
}
