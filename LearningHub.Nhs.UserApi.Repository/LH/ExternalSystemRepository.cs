namespace LearningHub.Nhs.UserApi.Repository.LH
{
    using System.Linq;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface.LH;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The external system repository.
    /// </summary>
    public class ExternalSystemRepository : GenericLHRepository<ExternalSystem>, Interface.LH.IExternalSystemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="tzOffsetManager">The Timezone offset manager.</param>
        public ExternalSystemRepository(LearningHubDbContext dbContext, ITimezoneOffsetManager tzOffsetManager)
            : base(dbContext, tzOffsetManager)
        {
        }

        /// <inheritdoc/>
        public async Task<ExternalSystem> GetByCode(string clientCode)
        {
            return await this.DbContext.ExternalSystem
                            .Where(t => t.Code == clientCode)
                            .AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task<ExternalSystem> GetExtSystemById(int id)
        {
            return await this.DbContext.ExternalSystem
                            .Include(n => n.AmendUser)
                            .Include(n => n.CreateUser)
                            .Where(t => t.Id == id)
                            .AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public IQueryable<ExternalSystem> GetExternalSystems()
        {
            return this.DbContext.Set<ExternalSystem>()
                .Include(n => n.AmendUser)
                .Include(n => n.CreateUser)
                .AsNoTracking();
        }
    }
}
