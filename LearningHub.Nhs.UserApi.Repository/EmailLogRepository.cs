// <copyright file="EmailLogRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The email log repository.
    /// </summary>
    public class EmailLogRepository : GenericElfhRepository<EmailLog>, IEmailLogRepository
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailLogRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public EmailLogRepository(ElfhHubDbContext dbContext, ILogger<EmailLog> logger, IOptions<Settings> settings)
            : base(dbContext, logger)
        {
            this.settings = settings.Value;
        }

        /// <inheritdoc/>
        public override async Task<int> CreateAsync(int userId, EmailLog emailLog)
        {
            if (!string.IsNullOrEmpty(this.settings.WebsiteEmailsTo))
            {
                emailLog.ToEmailAddress = this.settings.WebsiteEmailsTo;
            }

            emailLog.CreatedDate = DateTimeOffset.Now;
            return await base.CreateAsync(userId, emailLog);
        }
    }
}
