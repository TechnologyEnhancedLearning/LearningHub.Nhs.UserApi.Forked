namespace LearningHub.Nhs.UserApi.Repository
{
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The login wizard stage repository.
    /// </summary>
    public class LoginWizardStageRepository : GenericElfhRepository<LoginWizardStage>, ILoginWizardStageRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWizardStageRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public LoginWizardStageRepository(ElfhHubDbContext dbContext, ILogger<LoginWizardStage> logger)
            : base(dbContext, logger)
        {
        }
    }
}