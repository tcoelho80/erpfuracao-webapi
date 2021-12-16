using AutoMapper.EquivalencyExpression;
using ERP.Furacao.Application.Mappings;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using ERP.Furacao.Infrastructure.Crosscutting.Services.Logs;
using ERP.Furacao.Infrastructure.Data.Repositories;
using ERP.Furacao.Infrastructure.Data.Settings;
using ERP.Furacao.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace ERP.Furacao.WebApi.Setup
{
    public static class DependencyInjection
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityInfrastructure(configuration);
            services.AddSharedInfrastructure(configuration);
            services.AddPersistenceInfrastructure(configuration);
            services.AddApiVersioning();
            services.AddSwaggerExtension();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            });
            services.Configure<DapperDbSettings>
                (options => configuration.GetSection(nameof(DapperDbSettings)).Bind(options));
            services.AddSingleton(opt => ServiceProvider.GetRequiredService<IOptions<DapperDbSettings>>().Value);
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<ILogDapperRepository, LogDapperRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
