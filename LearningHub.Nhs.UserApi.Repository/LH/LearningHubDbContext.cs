// <copyright file="LearningHubDbContext.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.LH
{
    using LearningHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.Models.Entities.Messaging;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The learning hub db context.
    /// </summary>
    public partial class LearningHubDbContext : DbContext
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly LearningHubDbContextOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public LearningHubDbContext(LearningHubDbContextOptions options)
            : base(options.Options)
        {
            this.options = options;
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual DbSet<User> User { get; set; }

        /// <summary>
        /// Gets or sets the ExternalSystem.
        /// </summary>
        public virtual DbSet<ExternalSystem> ExternalSystem { get; set; }

        /// <summary>
        /// Gets or sets the ExternalSystemUser.
        /// </summary>
        public virtual DbSet<ExternalSystemUser> ExternalSystemUser { get; set; }

        /// <summary>
        /// Gets or sets the ExternalSystemDeepLink.
        /// </summary>
        public virtual DbSet<ExternalSystemDeepLink> ExternalSystemDeepLink { get; set; }

        /// <summary>
        /// Gets or sets the email template.
        /// </summary>
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var mapping in this.options.Mappings)
            {
                mapping.Map(modelBuilder);
            }

            modelBuilder.Entity<User>().Ignore(r => r.UserUserGroup);
            modelBuilder.Entity<User>().Ignore(r => r.UserNotificationAmendUser);
            modelBuilder.Entity<User>().Ignore(r => r.UserNotificationCreateUser);
            modelBuilder.Entity<User>().Ignore(r => r.UserNotificationUser);
            modelBuilder.Entity<User>().Ignore(r => r.ResourceVersion);
            modelBuilder.Entity<User>().Ignore(r => r.ResourceVersionEvent);
            modelBuilder.Entity<User>().Ignore(r => r.HierarchyEdit);
            modelBuilder.Entity<User>().Ignore(r => r.UserProvider);
        }
    }
}