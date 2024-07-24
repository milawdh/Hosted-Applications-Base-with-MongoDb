using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using ElmahCore.Mvc;
using ElmahCore;
using Newtonsoft.Json;
using static Base.Domain.DomainExtentions.Permission.PermissionReloadExtentions;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Base.WebFrameWork.ExceptionHandeling;
using Base.ServiceLayer.Roles;
using Base.ServiceLayer.Sms;
using Base.ServiceLayer.Permissions;
using Base.ServiceLayer.Users;
using Base.Domain.RepositoriesApi.Context;
using Base.Domain.RepositoriesApi.Core;
using Base.Domain.Api;
using Base.Domain.ApplicationSettings.DataSeed;
using Base.Domain.RepositoriesApi.UserInfo;
using Base.Domain.Utils.Configuration;
using Base.Domain.ApplicationSettings.Security;
using Base.Domain.Audities;
using Base.Infrasucture.Repository.Impl.Context;
using Base.Infrasucture.Connections.Api;
using Base.Infrasucture.Connections.Impl;
using Base.Contract.Permission;
using Base.Contract.Sms;
using Base.Contract.User.Services;
using Base.Contract.User.Acoount;
using Base.Contract.Roles;
using Base.ServiceLayer.Users.UserInfo;
using Base.Domain.DomainExceptions;

namespace Base.WebFrameWork.BootStrapers.Base
{
    public static class BaseBootStraper
    {
        public static void AppendStartupConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            AppendSingleTons(configuration);
            services.AppendConfigs(configuration);
            services.AppendCustomDI();
            services.AppendSecurityConfigs(configuration);
            services.RegisterMapsterConfiguration();
            services.SeedData(configuration);
        }

        private static void SeedData(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = SingleTon<AppSettings>.Instance.Get<DataSeedSettings>();
            if (!settings.Enabeld)
                return;

            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(s => s.GetTypes())
                                    .Where(p => typeof(IDomainDataSeeds).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var core = scope.ServiceProvider.GetRequiredService<IBaseCore>();
                var seedContext = new DomainSeedContext(core);
                foreach (var item in handlers)
                {
                    var seeder = Activator.CreateInstance(item) as IDomainDataSeeds;

                    seeder.SeedData(seedContext);
                }

                var saveResult = core.SaveChange();
                if (saveResult.IsFailed)
                    throw new InvalidArgumentException(saveResult.Messages);
            }

        }

        private static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IHasCustomMap).IsAssignableFrom(p) && p.IsClass);

            foreach (var handler in handlers)
            {
                var handlerInstance = (IHasCustomMap)Activator.CreateInstance(handler);
                handlerInstance.ConfigMap();
            }


            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }

        private static void AppendSwaggerConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            //services.abp
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.UseInlineDefinitionsForEnums();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "COVison", Version = "v1" });

                // Add JWT authentication support
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

            });
        }

        private static void AppendSecurityConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AppendSwaggerConfigs(configuration);



            var jwtSettings = SingleTon<AppSettings>.Instance.Get<JwtSettings>();

            var cert = new X509Certificate2(jwtSettings.FilePath, jwtSettings.ExportKey);

            // Configure JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new X509SecurityKey(cert)
                };
                options.MapInboundClaims = false;

            });
        }

        private static void AppendCustomDI(this IServiceCollection services)
        {

            services.AddExceptionHandler<ApplicationExceptionHandler>();
            services.AddHttpContextAccessor();
            services.AddScoped<IBaseDbConnectionContext, BaseDbConnectionContext>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ITenantDbConnectionContext, TenantDbConnectionContext>();
            services.AddScoped<IBasePermissionService, UserPermissionService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IUserTokenStoreService, UserTokenStoreService>();
            services.AddScoped<IUserIdentitiedLoginServices, UserIdentitiedLoginServices>();
            services.AddScoped<IBaseUserInfoContext>(p => new BaseUserInfoContext(p.GetRequiredService<IBaseDbConnectionContext>().DataBase, p.GetRequiredService<IHttpContextAccessor>(), p.GetRequiredService<IUserTokenStoreService>()));
            services.AddScoped<IUserInfoContext>(p => new UserInfoContext(p.GetRequiredService<IBaseDbConnectionContext>().DataBase, p.GetRequiredService<IBaseUserInfoContext>()));
            services.AddScoped<ICustomerInfoContext>(p => new CustomerInfoContext(p.GetRequiredService<IBaseDbConnectionContext>().DataBase, p.GetRequiredService<IBaseUserInfoContext>()));
            services.AddScoped<DomainValidationContext>();

            services.AddScoped<IBaseCore, BaseCore>();
            services.AddScoped<ITenantCore, TenantCore>();

            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.Path = "Elmah/Errors";
                options.Filters = new[] { new CustomElmahErrorFilter() };
                options.LogRequestBody = true;
                options.LogPath = Environment.CurrentDirectory + @"\App_Data\ElmahLogs";
            });
        }

        private static void AppendSingleTons(IConfiguration configuration)
        {
            var singleTons = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ISingleton).IsAssignableFrom(p) && p.IsClass);

            foreach (var item in singleTons)
                BaseSingleTon.Add(item, Activator.CreateInstance(item));
        }

        private static void AppendConfigs(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = SingleTon<AppSettings>.Instance;
            var configs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ISetting).IsAssignableFrom(p) && p.IsClass);

            foreach (var config in configs)
            {
                var configObject = Activator.CreateInstance(config);
                configuration.GetSection(config.Name).Bind(configObject);
                appSettings.Add(config, configObject as ISetting);
            }


        }

    }
}