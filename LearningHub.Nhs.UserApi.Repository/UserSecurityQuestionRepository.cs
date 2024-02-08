namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user security question repository.
    /// </summary>
    public class UserSecurityQuestionRepository : GenericElfhRepository<UserSecurityQuestion>, IUserSecurityQuestionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSecurityQuestionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserSecurityQuestionRepository(ElfhHubDbContext dbContext, ILogger<UserSecurityQuestion> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public IQueryable<UserSecurityQuestion> GetByUserId(int userId)
        {
            return this.DbContext.Set<UserSecurityQuestion>()
                .Include(uq => uq.SecurityQuestion)
                .Where(q => q.UserId == userId)
                .AsNoTracking().OrderBy(q => q.Id);
    }
    }
}
