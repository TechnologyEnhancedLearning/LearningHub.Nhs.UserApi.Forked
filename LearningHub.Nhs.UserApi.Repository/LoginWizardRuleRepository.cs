namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The login wizard rule repository.
    /// </summary>
    public class LoginWizardRuleRepository : GenericElfhRepository<LoginWizardRule>, ILoginWizardRuleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWizardRuleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public LoginWizardRuleRepository(ElfhHubDbContext dbContext, ILogger<LoginWizardRule> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public IEnumerable<LoginWizardRule> GetActive()
        {
            return this.DbContext.LoginWizardRule.AsNoTracking().Where(l => l.Active).OrderBy(l => l.Id);
        }
    }
}
