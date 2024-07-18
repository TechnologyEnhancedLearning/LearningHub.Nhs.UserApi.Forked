namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The terms and conditions repository.
    /// </summary>
    public class TermsAndConditionsRepository : GenericElfhRepository<TermsAndConditions>, ITermsAndConditionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TermsAndConditionsRepository(ElfhHubDbContext dbContext, ILogger<TermsAndConditions> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<TermsAndConditions> LatestVersionAsync(int tenantId)
        {
            return await this.DbContext.TermsAndConditions.Where(t => t.TenantId == tenantId).OrderByDescending(t => t.Id).AsNoTracking().FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task<TermsAndConditions> LatestVersionAcceptedAsync(int userId, int learningHubTenantId)
        {
            return await this.DbContext.UserTermsAndConditions
                .Include(utc => utc.TermsAndConditions)
                .Where(utc => utc.UserId == userId
                        && utc.TermsAndConditions.TenantId == learningHubTenantId
                        && utc.TermsAndConditions.Active)
                .OrderByDescending(utc => utc.TermsAndConditionsId)
                .Select(utc => utc.TermsAndConditions).AsNoTracking()
                .FirstOrDefaultWithNoLockAsync();
        }
    }
}