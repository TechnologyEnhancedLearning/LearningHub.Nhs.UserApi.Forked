// <copyright file="ExternalSystemDeepLinkRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.LH
{
    using System.Linq;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface.LH;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The external system deeplink repository.
    /// </summary>
    public class ExternalSystemDeepLinkRepository : GenericLHRepository<ExternalSystemDeepLink>, IExternalSystemDeepLinkRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemDeepLinkRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="tzOffsetManager">The Timezone offset manager.</param>
        public ExternalSystemDeepLinkRepository(LearningHubDbContext dbContext, ITimezoneOffsetManager tzOffsetManager)
            : base(dbContext, tzOffsetManager)
        {
        }

        /// <inheritdoc/>
        public async Task<ExternalSystemDeepLink> GetByCodeAsync(string code)
        {
            return await this.DbContext.ExternalSystemDeepLink
                            .Where(t => t.Code == code)
                            .AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync();
        }
    }
}