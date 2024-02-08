namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The gdc register repository.
    /// </summary>
    public class IpCountryLookupRepository : IIpCountryLookupRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IpCountryLookupRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public IpCountryLookupRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<bool> IsUKIpAddress(long ipAddress)
        {
            return await this.DbContext.IpCountryLookup.AsNoTracking()
                        .AnyAsync(i => i.Country == "GB" && ipAddress >= i.FromInt && ipAddress <= i.ToInt);
        }
    }
}
