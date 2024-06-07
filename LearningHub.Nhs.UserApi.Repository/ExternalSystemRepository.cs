namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// ExternalSystemRepository.
    /// </summary>
    public class ExternalSystemRepository : GenericElfhRepository<ExternalSystem>, IExternalSystemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ExternalSystemRepository(ElfhHubDbContext dbContext, ILogger<ExternalSystem> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public ExternalSystem GetByCode(string externalSystemCode)
        {
            return this.DbContext.ExternalSystem.Where(x => x.Code == externalSystemCode).Select(x =>
            new ExternalSystem
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).SingleOrDefault();
        }
    }
}
