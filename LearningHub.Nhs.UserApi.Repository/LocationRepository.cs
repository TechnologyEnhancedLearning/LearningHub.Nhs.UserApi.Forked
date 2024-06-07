namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The location repository.
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public LocationRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<Location> GetByIdAsync(int id)
        {
            return await this.DbContext.Location
                .Include(l => l.Type).AsNoTracking()
                .FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public IQueryable<Location> GetAll()
        {
            return this.DbContext.Set<Location>()
            .AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<Location> GetAllWithType()
        {
            return this.DbContext.Set<Location>()
            .Include(l => l.Type)
            .AsNoTracking();
        }
    }
}
