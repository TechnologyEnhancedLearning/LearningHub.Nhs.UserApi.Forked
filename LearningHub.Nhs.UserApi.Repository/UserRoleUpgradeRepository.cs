// <copyright file="UserRoleUpgradeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user user group repository.
    /// </summary>
    public class UserRoleUpgradeRepository : GenericElfhRepository<UserRoleUpgrade>, IUserRoleUpgradeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleUpgradeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserRoleUpgradeRepository(ElfhHubDbContext dbContext, ILogger<UserRoleUpgrade> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public IQueryable<UserRoleUpgrade> GetByUserIdAsync(int userId)
        {
            return this.DbContext.Set<UserRoleUpgrade>()
                .Where(n => n.UserId == userId && n.Deleted == false)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<UserRoleUpgrade> GetByEmailAddressAsync(string emailAddress, int userId)
        {
            return this.DbContext.Set<UserRoleUpgrade>()
                .Where(n => n.UserId == userId && n.EmailAddress == emailAddress && n.Deleted == false && n.UpgradeDate == null)
              .AsNoTracking();
        }
  }
}
