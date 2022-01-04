using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Application.Services;
using ERP.Furacao.Domain.Settings;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using ERP.Furacao.Infrastructure.Data.Contexts;
using ERP.Furacao.Infrastructure.Data.Repositories;
using ERP.Furacao.Infrastructure.Data.Repositoriess;
using ERP.Furacao.Infrastructure.Data.Services;
using ERP.Furacao.Infrastructure.Data.Settings;
using ERP.Furacao.Infrastructure.Identity.Contexts;
using ERP.Furacao.Infrastructure.Identity.Models;
using ERP.Furacao.Infrastructure.Identity.Repositories;
using ERP.Furacao.Infrastructure.Identity.Services;
using ERP.Furacao.WebApi.Setup;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\ERP.Furacao.WebAPI.xml", System.AppDomain.CurrentDomain.BaseDirectory));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Open Banking - WebApi",
                    Description = "API"
                });
                c.OperationFilter<SwaggerJsonIgnore>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });


        }

        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            {
                services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }

            IdentityModelEventSource.ShowPII = true;

            services.AddIdentity<IdentityApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };

                    ILogService _logService = DependencyInjection.ServiceProvider.GetService<ILogService>();
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/plain";
                            _logService.RecLog(LogEventLevel.Information, context.Exception.ToString(), null, nameof(o.Events.OnAuthenticationFailed));
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "text/plain";
                            _logService.RecLog(LogEventLevel.Information, context.Response.StatusCode.ToString(), null, nameof(o.Events.OnForbidden));
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
        }

        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositories
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IEmpresaDapperRepository, EmpresaDapperRepository>();
            services.AddScoped<IConvenioDapperRepository, ConvenioDapperRepository>();
            services.AddScoped<IPagamentoDapperRepository, PagamentoDapperRepository>();
            services.AddScoped<ILogDapperRepository, LogDapperRepository>();
            services.AddSingleton(typeof(IEntityRepository<>), typeof(EntityBaseRepository<>));
            services.AddTransient(typeof(IDapperRepository<>), typeof(DapperBaseRepository<>));
            services.AddTransient(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));
            services.AddTransient<IFornecedorItemRepository, FornecedorItemRepository>();
            #endregion

            #region Services
            var excelReaderSettings = new ExcelReaderSettings();
            configuration.GetSection("ExcelReaderSettings").Bind(excelReaderSettings);

            services.AddSingleton(excelReaderSettings);
            services.AddTransient<IExcelReaderService, ExcelReaderService>();
            #endregion

        }
    }
}
