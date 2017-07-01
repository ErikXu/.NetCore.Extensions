using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Exz.DependencyInjection.Registry
{
    public static class RegistryServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonsBySuffix(this IServiceCollection services, Assembly assembly, string suffix)
        {
            services.AddSingletons(assembly, FiltrateBySuffix(suffix), FiltrateBySuffix(suffix));
            return services;
        }

        public static IServiceCollection AddScopedsBySuffix(this IServiceCollection services, Assembly assembly, string suffix)
        {
            services.AddScopeds(assembly, FiltrateBySuffix(suffix), FiltrateBySuffix(suffix));
            return services;
        }

        public static IServiceCollection AddTransientsBySuffix(this IServiceCollection services, Assembly assembly, string suffix)
        {
            services.AddTransients(assembly, FiltrateBySuffix(suffix), FiltrateBySuffix(suffix));
            return services;
        }

        public static IServiceCollection AddSingletons(this IServiceCollection services, Assembly assembly, Expression<Func<Type, bool>> implementationFilter = null, Expression<Func<Type, bool>> serviceFilter = null)
        {
            var implemenetations = assembly.GetTypes().Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface).AsQueryable();

            if (implementationFilter != null)
            {
                implemenetations = implemenetations.Where(implementationFilter);
            }

            foreach (var implemenetation in implemenetations)
            {
                var service = serviceFilter == null ? implemenetation.GetInterfaces().First() : implemenetation.GetInterfaces().AsQueryable().First(serviceFilter);
                services.AddSingleton(service, implemenetation);
            }

            return services;
        }

        public static IServiceCollection AddScopeds(this IServiceCollection services, Assembly assembly, Expression<Func<Type, bool>> implementationFilter = null, Expression<Func<Type, bool>> serviceFilter = null)
        {
            var implemenetations = assembly.GetTypes().Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface).AsQueryable();

            if (implementationFilter != null)
            {
                implemenetations = implemenetations.Where(implementationFilter);
            }

            foreach (var implemenetation in implemenetations)
            {
                var service = serviceFilter == null ? implemenetation.GetInterfaces().First() : implemenetation.GetInterfaces().AsQueryable().First(serviceFilter);
                services.AddScoped(service, implemenetation);
            }

            return services;
        }

        public static IServiceCollection AddTransients(this IServiceCollection services, Assembly assembly, Expression<Func<Type, bool>> implementationFilter = null, Expression<Func<Type, bool>> serviceFilter = null)
        {
            var implemenetations = assembly.GetTypes().Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface).AsQueryable();

            if (implementationFilter != null)
            {
                implemenetations = implemenetations.Where(implementationFilter);
            }

            foreach (var implemenetation in implemenetations)
            {
                var service = serviceFilter == null ? implemenetation.GetInterfaces().First() : implemenetation.GetInterfaces().AsQueryable().First(serviceFilter);
                services.AddTransient(service, implemenetation);
            }

            return services;
        }

        private static Expression<Func<Type, bool>> FiltrateBySuffix(string suffix)
        {
            return t => t.Name.EndsWith(suffix);
        }
    }
}
