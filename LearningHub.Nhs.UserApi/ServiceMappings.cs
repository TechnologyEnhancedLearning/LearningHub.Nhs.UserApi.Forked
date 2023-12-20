// <copyright file="ServiceMappings.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi
{
    using AutoMapper;
    using elfhHub.Nhs.Models.Automapper;
    using IdentityServer4.AccessTokenValidation;
    using LearningHub.Nhs.Models.Automapper;
    using LearningHub.Nhs.UserApi.Authentication;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using ElfhMap = LearningHub.Nhs.UserApi.Repository.ElfhMap;
    using LHMap = LearningHub.Nhs.UserApi.Repository.LHMap;

    /// <summary>
    /// Extension class for <see cref="IServiceCollection"/> service mappings.
    /// </summary>
    public static class ServiceMappings
    {
        /// <summary>
        /// The add learning hub mappings.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddMappings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLearningHubDBMappings(configuration);

            services.AddElfhDBMappings(configuration);

            services.AddServices();

            services.AddAuthentication(configuration);

            services.AddAutomapper();
        }

        private static void AddElfhDBMappings(this IServiceCollection services, IConfiguration configuration)
        {
            var maxDatabaseRetryAttempts = configuration.GetValue<int>("Settings:MaxDatabaseRetryAttempts");

            services.AddSingleton(new DbContextOptionsBuilder<ElfhHubDbContext>()
                .UseSqlServer(configuration.GetConnectionString("ElfhHubDbConnection"), providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure(maxDatabaseRetryAttempts);
                    providerOptions.CommandTimeout(120);
                }).Options);

            services.AddSingleton<ElfhHubDbContextOptions>();

            services.AddDbContext<ElfhHubDbContext>();

            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.AttributeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.AttributeTypeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.CountryMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.EmailLogMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.EmailTemplateMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.EmailTemplateTypeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.ExternalSystemMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.GdcRegisterMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.GmcLrmpMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.GradeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.JobRoleMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.JobRoleGradeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.LocationMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.IpCountryLookupMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.LocationTypeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.LoginWizardRuleMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.LoginWizardStageActivityMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.LoginWizardStageMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.MedicalCouncilMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.RegionMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.SecurityQuestionMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.SpecialtyMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.StaffGroupMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.SystemSettingMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.TenantMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.TenantSmtpMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.TenantUrlMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.TenantUserGroupMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.TermsAndConditionsMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserAttributeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserEmploymentMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserExternalSystemMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserGroupMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserGroupTypeInputValidationMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserHistoryAttributeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserHistoryMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserPasswordValidationTokenMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserRoleUpgradeMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserSecurityQuestionMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserTermsAndConditionsMap>();
            services.AddSingleton<ElfhMap.IEntityTypeMap, ElfhMap.UserUserGroupMap>();

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IElfhUserRepository, ElfhUserRepository>();
            services.AddScoped<Repository.Interface.IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IExternalSystemRepository, ExternalSystemRepository>();
            services.AddScoped<IGdcRegisterRepository, GdcRegisterRepository>();
            services.AddScoped<IGmcLrmpRepository, GmcLrmpRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IJobRoleRepository, JobRoleRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IIpCountryLookupRepository, IpCountryLookupRepository>();
            services.AddScoped<ILoginWizardRuleRepository, LoginWizardRuleRepository>();
            services.AddScoped<ILoginWizardStageActivityRepository, LoginWizardStageActivityRepository>();
            services.AddScoped<ILoginWizardStageRepository, LoginWizardStageRepository>();
            services.AddScoped<IMedicalCouncilRepository, MedicalCouncilRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ISecurityQuestionRepository, SecurityQuestionRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            services.AddScoped<IStaffGroupRepository, StaffGroupRepository>();
            services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<ITenantSmtpRepository, TenantSmtpRepository>();
            services.AddScoped<ITermsAndConditionsRepository, TermsAndConditionsRepository>();
            services.AddScoped<IUserAttributeRepository, UserAttributeRepository>();
            services.AddScoped<IUserEmploymentRepository, UserEmploymentRepository>();
            services.AddScoped<IUserExternalSystemRepository, UserExternalSystemRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IUserGroupTypeInputValidationRepository, UserGroupTypeInputValidationRepository>();
            services.AddScoped<IUserHistoryAttributeRepository, UserHistoryAttributeRepository>();
            services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();
            services.AddScoped<IUserPasswordValidationTokenRepository, UserPasswordValidationTokenRepository>();
            services.AddScoped<IUserRoleUpgradeRepository, UserRoleUpgradeRepository>();
            services.AddScoped<IUserSecurityQuestionRepository, UserSecurityQuestionRepository>();
            services.AddScoped<IUserTermsAndConditionsRepository, UserTermsAndConditionsRepository>();
            services.AddScoped<IUserUserGroupRepository, UserUserGroupRepository>();

            var elfhCacheOptions = Options.Create(new Microsoft.Extensions.Caching.Redis.RedisCacheOptions
            {
                Configuration = configuration.GetConnectionString("ElfhRedis"),
            });
            var elfhCache = new ElfhRedisCache(elfhCacheOptions);
            services.AddSingleton<IElfhRedisCache>(elfhCache);
        }

        private static void AddLearningHubDBMappings(this IServiceCollection services, IConfiguration configuration)
        {
            var maxDatabaseRetryAttempts = configuration.GetValue<int>("Settings:MaxDatabaseRetryAttempts");

            services.AddSingleton(new DbContextOptionsBuilder<Repository.LH.LearningHubDbContext>()
                .UseSqlServer(configuration.GetConnectionString("LearningHubDbConnection"), providerOptions => { providerOptions.EnableRetryOnFailure(maxDatabaseRetryAttempts); })
                .Options);

            services.AddSingleton<Repository.LH.LearningHubDbContextOptions>();

            services.AddDbContext<Repository.LH.LearningHubDbContext>();

            services.AddSingleton<LHMap.IEntityTypeMap, LHMap.ExternalSystemMap>();
            services.AddSingleton<LHMap.IEntityTypeMap, LHMap.ExternalSystemDeepLinkMap>();
            services.AddSingleton<LHMap.IEntityTypeMap, LHMap.ExternalSystemUserMap>();
            services.AddSingleton<LHMap.IEntityTypeMap, LHMap.UserMap>();

            services.AddScoped<Repository.Interface.LH.IExternalSystemRepository, Repository.LH.ExternalSystemRepository>();
            services.AddScoped<Repository.Interface.LH.IExternalSystemDeepLinkRepository, Repository.LH.ExternalSystemDeepLinkRepository>();
            services.AddScoped<Repository.Interface.LH.IExternalSystemUserRepository, Repository.LH.ExternalSystemUserRepository>();
            services.AddScoped<Repository.Interface.LH.IMessageRepository, Repository.LH.MessageRepository>();
            services.AddScoped<Repository.Interface.LH.ITimezoneOffsetManager, Repository.LH.TimezoneOffsetManager>();
            services.AddScoped<Repository.Interface.LH.IUserRepository, Repository.LH.UserRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IElfhRedisCache, ElfhRedisCache>();
            services.AddScoped<IElfhUserService, ElfhUserService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IJobRoleService, JobRoleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILoginWizardService, LoginWizardService>();
            services.AddScoped<IMedicalCouncilService, MedicalCouncilService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPasswordManagerService, PasswordManagerService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IStaffGroupService, StaffGroupService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITermsAndConditionsService, TermsAndConditionsService>();
            services.AddScoped<IUserEmploymentService, UserEmploymentService>();
            services.AddScoped<IUserHistoryService, UserHistoryService>();

            services.AddScoped<IExternalSystemDeepLinkService, ExternalSystemDeepLinkService>();
            services.AddScoped<IExternalSystemService, ExternalSystemService>();
            services.AddScoped<IExternalSystemUserService, ExternalSystemUserService>();
        }

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = configuration.GetValue<string>("Settings:AuthenticationServiceUrl");
                  options.ApiName = "userapi";
              });

            services.AddSingleton<IAuthorizationHandler, AuthorizeOrCallFromLHHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizeOrCallFromLH", policy => policy.Requirements.Add(new AuthorizeOrCallFromLHRequirement()));
            });
        }

        private static void AddAutomapper(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
                mc.AddProfile(new ElfhMappingProfile());
            }).CreateMapper());
        }
    }
}