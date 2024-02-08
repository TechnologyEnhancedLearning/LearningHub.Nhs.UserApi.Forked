namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The system setting repository.
    /// </summary>
    public class SystemSettingRepository : GenericElfhRepository<SystemSetting>, ISystemSettingRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SystemSettingRepository(ElfhHubDbContext dbContext, ILogger<SystemSetting> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<SystemSetting> GetByIdAsync(int id)
        {
            return await this.DbContext.SystemSetting.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
