using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobSwitcher.Cli.Extensions
{
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

            services.AddSingleton<TService>(implementationFactory);
        }
    }
}
