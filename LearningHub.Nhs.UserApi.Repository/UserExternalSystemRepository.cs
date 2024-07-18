namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// UserExternalSystemRepository.
    /// </summary>
    public class UserExternalSystemRepository : GenericElfhRepository<UserExternalSystem>, IUserExternalSystemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserExternalSystemRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserExternalSystemRepository(ElfhHubDbContext dbContext, ILogger<UserExternalSystem> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public UserExternalSystem GetUserExternalSystem(int userId, int externalSystemId)
        {
            return this.DbContext.Set<UserExternalSystem>().SingleOrDefault(
                x => x.UserId == userId && x.ExternalSystemId == externalSystemId);
        }

        /// <inheritdoc/>
        public async Task<UserExternalSystem> GetUserExternalSystemByUserIdandCodeAsync(int userId, string code)
        {
            return await this.DbContext.UserExternalSystem
                            .Where(t => t.UserId == userId && t.ExternalSystem.Code == code)
                            .FirstOrDefaultWithNoLockAsync();
        }
    }
}
