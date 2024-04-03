namespace LearningHub.Nhs.UserApi
{
    using System;
    using System.IO;
    using System.Reflection;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Models.Extensions;
    using LearningHub.Nhs.UserApi.Services;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// ServiceCollectionExtension.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// ConfigureServices.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="configuration">IConfiguration.</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddApplicationInsightsTelemetry();

            services.AddMappings(configuration);

            services.Configure<Settings>(configuration.GetSection("Settings"));

            var swaggerTitle = configuration["Swagger:Title"];
            var swaggerVersion = configuration["Swagger:Version"];
            var swaggerDescription = $"Build Number: {configuration["Swagger:BuildNumber"]}";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerTitle, new OpenApiInfo { Title = swaggerTitle, Version = swaggerVersion, Description = swaggerDescription });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML"));
                c.CustomSchemaIds(type => type.ToString());
            });

            services.AddMvc();

            var environment = configuration.GetValue<EnvironmentEnum>("Environment");
            var envPrefix = environment.GetAbbreviation();
            if (environment == EnvironmentEnum.Local)
            {
                envPrefix += $"_{Environment.MachineName}";
            }

            services.AddDistributedCache(option =>
            {
                option.RedisConnectionString = configuration.GetConnectionString("LearningHubRedis");
                option.KeyPrefix = envPrefix;
                option.DefaultExpiryInMinutes = 60;
            });

            var elfhCacheOptions = Options.Create(new Microsoft.Extensions.Caching.Redis.RedisCacheOptions
            {
                Configuration = configuration.GetConnectionString("ElfhRedis"),
            });
            var elfhCache = new ElfhRedisCache(elfhCacheOptions);
            services.AddSingleton<IElfhRedisCache>(elfhCache);

            services.AddSingleton(configuration);
            services.AddApplicationInsightsTelemetry();
        }
    }
}
