namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The email template repository.
    /// </summary>
    public class EmailTemplateRepository : GenericElfhRepository<EmailTemplate>, IEmailTemplateRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public EmailTemplateRepository(ElfhHubDbContext dbContext, ILogger<EmailTemplate> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<EmailTemplate> GetByTypeAndTenantAsync(int emailTemplateTypeId, int tenantId)
        {
            return await this.DbContext.EmailTemplate.AsNoTracking().FirstOrDefaultWithNoLockAsync(et => et.EmailTemplateTypeId == emailTemplateTypeId && et.TenantId == tenantId);
        }
    }
}
