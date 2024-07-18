namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The job role repository.
    /// </summary>
    public class JobRoleRepository : GenericElfhRepository<JobRole>, IJobRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobRoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public JobRoleRepository(ElfhHubDbContext dbContext, ILogger<JobRole> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<JobRole> GetByIdAsync(int id)
        {
            return await this.DbContext.JobRole
                        .Include(jr => jr.StaffGroup)
                        .Include(jr => jr.MedicalCouncil).AsNoTracking()
                        .FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public IQueryable<JobRole> GetAllWithStaffGroup()
        {
            return this.DbContext.Set<JobRole>()
                .Include(jr => jr.StaffGroup)
                .Include(jr => jr.MedicalCouncil).AsNoTracking();
        }
    }
}
