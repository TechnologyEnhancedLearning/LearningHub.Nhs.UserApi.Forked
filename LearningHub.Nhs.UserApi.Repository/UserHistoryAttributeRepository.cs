namespace LearningHub.Nhs.UserApi.Repository
{
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user history attribute repository.
    /// </summary>
    public class UserHistoryAttributeRepository : GenericElfhRepository<UserHistoryAttribute>, IUserHistoryAttributeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserHistoryAttributeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="logger">The logger.</param>
        public UserHistoryAttributeRepository(ElfhHubDbContext dbContext, ILogger<UserHistoryAttribute> logger)
            : base(dbContext, logger)
        {
        }
    }
}