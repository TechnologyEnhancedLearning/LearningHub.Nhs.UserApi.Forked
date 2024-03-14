namespace LearningHub.Nhs.Auth.Configuration
{
    using System.Collections.Generic;
    using System.Net.Http;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.Services;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.Models.Common.Interfaces;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// The service mappings.
    /// </summary>
    public static class ServiceMappings
    {
        /// <summary>
        /// The add service mappings.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        public static void AddServiceMappings(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddScoped<IUserService, UserService>();
            if (env.IsDevelopment())
            {
                services.AddHttpClient<IUserApiHttpClient, UserApiHttpClient>()
                    .ConfigurePrimaryHttpMessageHandler(
                        () => new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                        });
            }
            else
            {
                services.AddHttpClient<IUserApiHttpClient, UserApiHttpClient>();
            }

            services.AddDistributedMemoryCache();
            services.AddScoped<IExternalSystemService, ExternalSystemService>();
            services.AddTransient<IRegistrationService, RegistrationService>();

            // web settings binding
            var webSettings = new WebSettings();
            configuration.Bind("WebSettings", webSettings);
            services.AddSingleton(configuration);
            services.AddSingleton(webSettings);
        }
    }
}