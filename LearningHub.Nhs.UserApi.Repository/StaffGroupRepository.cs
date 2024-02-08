namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The staff group repository.
    /// </summary>
    public class StaffGroupRepository : GenericElfhRepository<StaffGroup>, IStaffGroupRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffGroupRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public StaffGroupRepository(ElfhHubDbContext dbContext, ILogger<StaffGroup> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<StaffGroup> GetByIdAsync(int id)
        {
            return await this.DbContext.StaffGroup.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
