using ElasticSearchSharp.Services.Services.Elastic;
using Microsoft.Extensions.DependencyInjection;
using SharedDomain.Configuration;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using System.Security.Principal;
using Scrutor;

namespace ElasticSearchSharp.Services.DependencyInjection
{
    public static class DependencyExtension
    {
        /// <summary>
        /// Adds Elasticsearch framework services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to which the Elasticsearch services will be added.</param>
        /// <param name="config">An action to configure the <see cref="ElasticConfig"/> settings.</param>
        /// <returns>The original <see cref="IServiceCollection"/> with the Elasticsearch services added.</returns>
        /// <remarks>
        /// This method performs the following operations:
        /// 1. Scans the provided assemblies for classes that implement the <see cref="IElasticContext"/> interface and registers them with the scoped lifetime.
        /// 2. Adds the <see cref="ElasticConfig"/> configuration to the service collection as a singleton.
        /// </remarks>
        public static IServiceCollection AddElasticFramework(this IServiceCollection services, Action<ElasticConfig> config)
        {
            var elasticConfig = new ElasticConfig();
            config.Invoke(elasticConfig);
            services.Scan(s =>
                s.FromAssemblies(elasticConfig.assemblies.ToArray())
                 .AddClasses(c => c.AssignableTo(typeof(IElasticContext)))
                 .AsImplementedInterfaces()
                 .WithScopedLifetime());
            services.AddSingleton(sp =>{return elasticConfig;});
            return services;
        }

    }
}
