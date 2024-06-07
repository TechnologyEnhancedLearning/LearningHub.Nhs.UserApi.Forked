namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The tenant smtp repository.
    /// </summary>
    public class TenantSmtpRepository : GenericElfhRepository<TenantSmtp>, ITenantSmtpRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantSmtpRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TenantSmtpRepository(ElfhHubDbContext dbContext, ILogger<TenantSmtp> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<TenantSmtp> GetByIdAsync(int id)
        {
            return await this.DbContext.TenantSmtp.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
