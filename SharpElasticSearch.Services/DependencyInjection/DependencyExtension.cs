using ElasticSearchSharp.Services.Services.Elastic;
using Microsoft.Extensions.DependencyInjection;
using SharedDomain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchSharp.Services.DependencyInjection
{
    public static class DependencyExtension
    {
        public static IServiceCollection AddElasticFramework(this IServiceCollection services, ElasticConfig config)
        {

            services.AddScoped<IElasticContext, ElasticContext>();
            return services;
        }
    }
}
