// <copyright file="UserUserGroupRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user user group repository.
    /// </summary>
    public class UserUserGroupRepository : GenericElfhRepository<UserUserGroup>, IUserUserGroupRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUserGroupRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserUserGroupRepository(ElfhHubDbContext dbContext, ILogger<UserUserGroup> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int userId, int userUserGroupId)
        {
            var userUserGroup = this.DbContext.UserUserGroup
                    .Where(uug => uug.Id == userUserGroupId && !uug.Deleted)
                    .SingleOrDefault();
            this.SetAuditFieldsForDelete(userId, userUserGroup);
            await this.DbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public Task<bool> HasUserBeenAssignedToUserGroup(int userId, int userGroupId)
        {
            return this.DbContext.UserUserGroup
                .Where(uug => uug.UserId == userId && uug.UserGroupId == userGroupId && !uug.Deleted)
                .AnyAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteUserGroupAsync(int userId, int userGroupId)
        {
            var userUserGroup = this.DbContext.UserUserGroup
                    .Where(uug => uug.UserId == userId && uug.UserGroupId == userGroupId && !uug.Deleted)
                    .SingleOrDefault();
            if (userUserGroup != null)
            {
                this.SetAuditFieldsForDelete(userId, userUserGroup);
                await this.DbContext.SaveChangesAsync();
            }
        }
    }
}
