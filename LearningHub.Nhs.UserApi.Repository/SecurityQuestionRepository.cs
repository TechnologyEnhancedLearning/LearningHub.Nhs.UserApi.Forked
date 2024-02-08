namespace LearningHub.Nhs.UserApi.Repository
{
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The security question repository.
    /// </summary>
    public class SecurityQuestionRepository : GenericElfhRepository<SecurityQuestion>, ISecurityQuestionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityQuestionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SecurityQuestionRepository(ElfhHubDbContext dbContext, ILogger<SecurityQuestion> logger)
            : base(dbContext, logger)
        {
        }
    }
}
