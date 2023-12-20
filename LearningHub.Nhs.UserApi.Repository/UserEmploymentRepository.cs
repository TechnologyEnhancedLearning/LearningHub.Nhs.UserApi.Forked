// <copyright file="UserEmploymentRepository.cs" company="HEE.nhs.uk">
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
    /// The user employment repository.
    /// </summary>
    public class UserEmploymentRepository : GenericElfhRepository<UserEmployment>, IUserEmploymentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEmploymentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserEmploymentRepository(ElfhHubDbContext dbContext, ILogger<UserEmployment> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<UserEmployment> GetByIdAsync(int id)
        {
            return await this.DbContext.UserEmployment
                .Where(ue => ue.Id == id).AsNoTracking()
                .FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public override async Task<int> CreateAsync(int userId, UserEmployment userEmployment)
        {
            userEmployment.Archived = false;
            return await base.CreateAsync(userId, userEmployment);
        }

        /// <inheritdoc/>
        public IQueryable<UserEmployment> GetAllWithUser()
        {
            return this.DbContext.Set<UserEmployment>().Include(u => u.User).AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<UserEmployment> GetPrimaryForUser(int userId)
        {
            return await this.DbContext.UserEmployment
                .Include(ue => ue.User)
                .Include(ue => ue.Location)
                .Where(ue => ue.UserId == userId && ue.Id == ue.User.PrimaryUserEmploymentId && !ue.Archived).AsNoTracking()
                .FirstOrDefaultWithNoLockAsync();
        }
    }
}
