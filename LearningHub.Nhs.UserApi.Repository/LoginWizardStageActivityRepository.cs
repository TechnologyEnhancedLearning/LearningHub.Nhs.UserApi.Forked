namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The login wizard stage activity repository.
    /// </summary>
    public class LoginWizardStageActivityRepository : ILoginWizardStageActivityRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWizardStageActivityRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public LoginWizardStageActivityRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(LoginWizardStageActivity loginWizardStageActivity)
        {
            var amendDate = DateTimeOffset.Now;
            loginWizardStageActivity.ActivityDatetime = amendDate;

            await this.DbContext.LoginWizardStageActivity.AddAsync(loginWizardStageActivity);

            await this.DbContext.SaveChangesAsync();

            this.DbContext.Entry(loginWizardStageActivity).State = EntityState.Detached;

            return loginWizardStageActivity.Id;
        }

        /// <inheritdoc/>
        public IQueryable<LoginWizardStageActivity> GetByUser(int userId)
        {
            return this.DbContext.LoginWizardStageActivity.Where(l => l.UserId == userId)
                .Include(l => l.LoginWizardStage).AsNoTracking();
        }
    }
}
