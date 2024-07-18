namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The user password validation token repository.
    /// </summary>
    public class UserPasswordValidationTokenRepository : IUserPasswordValidationTokenRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPasswordValidationTokenRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public UserPasswordValidationTokenRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<UserPasswordValidationToken> GetByToken(string lookup)
        {
            return await this.DbContext.UserPasswordValidationToken
                .Include(vt => vt.User)
                .Where(vt => vt.Lookup == lookup).AsNoTracking()
                .FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(int userId, UserPasswordValidationToken userPasswordValidationToken)
        {
            await this.DbContext.Set<UserPasswordValidationToken>().AddAsync(userPasswordValidationToken);
            var createdDate = DateTimeOffset.Now;
            userPasswordValidationToken.CreatedUserId = userId;
            userPasswordValidationToken.CreatedDate = createdDate;

            await this.DbContext.SaveChangesAsync();
            this.DbContext.Entry(userPasswordValidationToken).State = EntityState.Detached;

            return userPasswordValidationToken.Id;
        }

        /// <inheritdoc/>
        public async Task ExpireUserPasswordValidationToken(string lookup)
        {
            var upvt = await this.GetByToken(lookup);
            this.DbContext.UserPasswordValidationToken.Remove(upvt);
            this.DbContext.SaveChanges();
        }
    }
}
