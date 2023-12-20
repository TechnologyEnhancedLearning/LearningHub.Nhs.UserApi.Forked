// <copyright file="ExternalSystemUserRepository.cs" company="HEE.nhs.uk">
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
    /// The external system user repository.
    /// </summary>
    public class ExternalSystemUserRepository : GenericLHRepository<ExternalSystemUser>, Interface.LH.IExternalSystemUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemUserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="tzOffsetManager">The Timezone offset manager.</param>
        public ExternalSystemUserRepository(LearningHubDbContext dbContext, ITimezoneOffsetManager tzOffsetManager)
            : base(dbContext, tzOffsetManager)
        {
        }

        /// <inheritdoc/>
        public async Task<ExternalSystemUser> GetByIdAsync(int userId, int externalSystemId)
        {
            return await this.DbContext.ExternalSystemUser
                            .Where(t => t.UserId == userId && t.ExternalSystemId == externalSystemId)
                            .AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync();
        }
    }
}
