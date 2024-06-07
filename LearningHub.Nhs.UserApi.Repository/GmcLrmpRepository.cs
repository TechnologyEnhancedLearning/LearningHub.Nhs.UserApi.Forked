namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The gmc lrmp repository.
    /// </summary>
    public class GmcLrmpRepository : IGmcLrmpRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GmcLrmpRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public GmcLrmpRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<GmcLrmp> GetByLastNameAndGMCNumber(string lastname, string medicalCouncilNumber)
        {
            return await this.DbContext.GmcLrmp.AsNoTracking().FirstOrDefaultWithNoLockAsync(g => g.Surname == lastname && g.GmcRefNo == medicalCouncilNumber);
        }
    }
}
