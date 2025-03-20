namespace LearningHub.Nhs.Auth
{
    using System;
    using System.Collections.Concurrent;
    using System.Security.Cryptography.X509Certificates;
    using Azure.Identity;
    using IdentityServer4;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Helpers;
    using LearningHub.Nhs.Auth.Middleware;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Models.Extensions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using StackExchange.Redis;

    /// <summary>
    /// The ServiceCollectionExtension.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// ConfigureServices.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="configuration">IConfiguration.</param>
        /// <param name="env">IWebHostEnvironment.</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddApplicationInsightsTelemetry();

            services.Configure<OpenAthensLearningHubClientDictionary>(configuration.GetSection("OaLhClients"));
            services.Configure<WebSettings>(configuration.GetSection(nameof(WebSettings)));
            services.Configure<LearningHubAuthConfig>(configuration.GetSection(nameof(LearningHubAuthConfig)));

            var azureDataProtection = configuration.GetSection("AzureDataProtection").Get<AzureDataProtectionConfig>();

            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-3.1
            services.AddDataProtection()
                .PersistKeysToAzureBlobStorage(
                    azureDataProtection.StorageConnectionString,
                    azureDataProtection.StorageContainerName,
                    "Auth-DataProtection-Keys")
                .ProtectKeysWithAzureKeyVault(
                    new Uri(azureDataProtection.VaultKeyIdentifier),
                    new ClientSecretCredential(azureDataProtection.TenantId, azureDataProtection.ClientId, azureDataProtection.ClientSecret))
                .SetApplicationName("LearningHub.Nhs.Auth");

            services.AddServiceMappings(configuration, env);
            services.AddMvc();

            var redisOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
            redisOptions.ClientName = configuration["ClientName"];
            var redisConn = ConnectionMultiplexer.Connect(redisOptions);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn.Configuration;
            });

            var openAthensConfig = configuration.GetSection(nameof(OpenAthensConfig)).Get<OpenAthensConfig>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(
                "oidc_oa",
                options =>
                {
                    options.Authority = openAthensConfig.Authority;
                    options.ClientId = openAthensConfig.ClientId;
                    options.ClientSecret = openAthensConfig.ClientSecret;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ResponseType = "code id_token";
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.ClaimActions.MapUniqueJsonKey("eduPersonTargetedID", "eduPersonTargetedID");
                    options.ClaimActions.MapUniqueJsonKey("eduPersonScopedAffiliation", "eduPersonScopedAffiliation");
                    options.ClaimActions.MapUniqueJsonKey("username", "username");
                    options.CallbackPath = new PathString(openAthensConfig.RedirectUri);

                    // Tell Identity Server to treat this provider as external to itself.
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });

            var environment = configuration.GetValue<EnvironmentEnum>("Environment");
            var envPrefix = environment.GetAbbreviation();
            if (environment == EnvironmentEnum.Local)
            {
                envPrefix += $"_{Environment.MachineName}";
            }

            // https://github.com/AliBazzi/IdentityServer4.Contrib.RedisStore
            var isBuilder = services.AddIdentityServer()
                .AddLearningHubUserStore()
                .AddOperationalStore(options =>
                {
                    options.RedisConnectionMultiplexer = redisConn;
                    options.KeyPrefix = $"{envPrefix}_id4_opstore";
                })
                .AddRedisCaching(options =>
                {
                    options.RedisConnectionMultiplexer = redisConn;
                    options.KeyPrefix = $"{envPrefix}_id4";
                })
                .AddResourceStoreCache<LearningHubResourceStore>()
                .AddClientStoreCache<LearningHubClientStore>();

            if (env.IsDevelopment())
            {
                isBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                var webSettings = configuration.GetSection(nameof(WebSettings)).Get<WebSettings>();

                // User Certificate
                X509Certificate2 cert = null;
                using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    certStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certCollection = certStore.Certificates.Find(
                        X509FindType.FindByThumbprint,
                        webSettings.X509Certificate2Thumbprint, // Thumbprint of certificate
                        false);
                    //// Get the first cert with the thumbprint
                    if (certCollection.Count > 0)
                    {
                        cert = certCollection[0];
                    }
                }

                isBuilder.AddSigningCredential(cert);
            }

            // Set up redis caching.
            services.AddDistributedCache(opt =>
            {
                opt.RedisConnectionString = configuration.GetConnectionString("Redis");
                opt.KeyPrefix = $"{envPrefix}_Auth";
                opt.DefaultExpiryInMinutes = 60;
            });
        }
    }
}
