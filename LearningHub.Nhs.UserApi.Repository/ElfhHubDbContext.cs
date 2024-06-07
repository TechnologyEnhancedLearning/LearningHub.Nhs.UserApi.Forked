namespace LearningHub.Nhs.UserApi.Repository
{
    using elfhHub.Nhs.Models.Dto;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Dashboard;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The elfh hub db context.
    /// </summary>
    public partial class ElfhHubDbContext : DbContext
    {
        private readonly ElfhHubDbContextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhHubDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ElfhHubDbContext(ElfhHubDbContextOptions options)
            : base(options.Options)
        {
            this.options = options;
        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public virtual DbSet<Country> Country { get; set; }

        /// <summary>
        /// Gets or sets the email template.
        /// </summary>
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the user email upgrade.
        /// </summary>
        public virtual DbSet<UserRoleUpgrade> UserRoleUpgrade { get; set; }

        /// <summary>
        /// Gets or sets the ExternalSystem.
        /// </summary>
        public virtual DbSet<ExternalSystem> ExternalSystem { get; set; }

        /// <summary>
        /// Gets or sets the user external system.
        /// </summary>
        public virtual DbSet<UserExternalSystem> UserExternalSystem { get; set; }

        /// <summary>
        /// Gets or sets the gdc register.
        /// </summary>
        public virtual DbSet<GdcRegister> GdcRegister { get; set; }

        /// <summary>
        /// Gets or sets the gmc lrmp.
        /// </summary>
        public virtual DbSet<GmcLrmp> GmcLrmp { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        public virtual DbSet<Grade> Grade { get; set; }

        /// <summary>
        /// Gets or sets the job role.
        /// </summary>
        public virtual DbSet<JobRole> JobRole { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public virtual DbSet<Location> Location { get; set; }

        /// <summary>
        /// Gets or sets the ip country lookup.
        /// </summary>
        public virtual DbSet<IpCountryLookup> IpCountryLookup { get; set; }

        /// <summary>
        /// Gets or sets the medical council.
        /// </summary>
        public virtual DbSet<MedicalCouncil> MedicalCouncil { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public virtual DbSet<Region> Region { get; set; }

        /// <summary>
        /// Gets or sets the login wizard stage activity.
        /// </summary>
        public virtual DbSet<LoginWizardStageActivity> LoginWizardStageActivity { get; set; }

        /// <summary>
        /// Gets or sets the login wizard rule.
        /// </summary>
        public virtual DbSet<LoginWizardRule> LoginWizardRule { get; set; }

        /// <summary>
        /// Gets or sets the specialty.
        /// </summary>
        public virtual DbSet<Specialty> Specialty { get; set; }

        /// <summary>
        /// Gets or sets the staff group.
        /// </summary>
        public virtual DbSet<StaffGroup> StaffGroup { get; set; }

        /// <summary>
        /// Gets or sets the system setting.
        /// </summary>
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }

        /// <summary>
        /// Gets or sets the tenant.
        /// </summary>
        public virtual DbSet<Tenant> Tenant { get; set; }

        /// <summary>
        /// Gets or sets the tenant smtp.
        /// </summary>
        public virtual DbSet<TenantSmtp> TenantSmtp { get; set; }

        /// <summary>
        /// Gets or sets the tenant url.
        /// </summary>
        public virtual DbSet<TenantUrl> TenantUrl { get; set; }

        /// <summary>
        /// Gets or sets the terms and conditions.
        /// </summary>
        public virtual DbSet<TermsAndConditions> TermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual DbSet<User> User { get; set; }

        /// <summary>
        /// Gets or sets the user attribute.
        /// </summary>
        public virtual DbSet<UserAttribute> UserAttribute { get; set; }

        /// <summary>
        /// Gets or sets the user employment.
        /// </summary>
        public virtual DbSet<UserEmployment> UserEmployment { get; set; }

        /// <summary>
        /// Gets or sets the user group type input validation.
        /// </summary>
        public virtual DbSet<UserGroupTypeInputValidation> UserGroupTypeInputValidation { get; set; }

        /// <summary>
        /// Gets or sets the user history.
        /// </summary>
        public virtual DbSet<UserHistory> UserHistory { get; set; }

        /// <summary>
        /// Gets or sets the user history stored proc results.
        /// </summary>
        public virtual DbSet<UserHistoryStoredProcResult> UserHistoryStoredProcResult { get; set; }

        /// <summary>
        /// Gets or sets the user history attribute.
        /// </summary>
        public virtual DbSet<UserHistoryAttribute> UserHistoryAttribute { get; set; }

        /// <summary>
        /// Gets or sets the user password validation token.
        /// </summary>
        public virtual DbSet<UserPasswordValidationToken> UserPasswordValidationToken { get; set; }

        /// <summary>
        /// Gets or sets the user terms and conditions.
        /// </summary>
        public virtual DbSet<UserTermsAndConditions> UserTermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets the user user group.
        /// </summary>
        public virtual DbSet<UserUserGroup> UserUserGroup { get; set; }

        /// <summary>
        /// Gets or sets the UserAuthentiate Dto.
        /// </summary>
        public virtual DbSet<UserAuthenticateDto> UserAuthenticateDto { get; set; }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserExternalSystem>().Ignore(x => x.Deleted);
            modelBuilder.Entity<ExternalSystem>().Ignore(x => x.Deleted);

            foreach (var mapping in this.options.Mappings)
            {
                mapping.Map(modelBuilder);
            }
        }
    }
}