// <copyright file="UserGroupRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user group repository.
    /// </summary>
    public class UserGroupRepository : GenericElfhRepository<UserGroup>, IUserGroupRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserGroupRepository(ElfhHubDbContext dbContext, ILogger<UserGroup> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<List<UserGroup>> GetByUserAsync(int userId)
        {
            return await this.DbContext.UserUserGroup
                .Where(uug => uug.UserId == userId && uug.Deleted == false)
                .Include(r => r.UserGroup)
                .Select(uug => uug.UserGroup)
                .AsNoTracking()
                .ToListWithNoLockAsync();
        }
    }
}
