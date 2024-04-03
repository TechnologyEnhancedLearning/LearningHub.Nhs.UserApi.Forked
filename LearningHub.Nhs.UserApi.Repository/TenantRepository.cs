namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The tenant repository.
    /// </summary>
    public class TenantRepository : GenericElfhRepository<Tenant>, ITenantRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TenantRepository(ElfhHubDbContext dbContext, ILogger<Tenant> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<Tenant> GetByIdAsync(int id, bool includeTenantUrls = false)
        {
            if (includeTenantUrls)
            {
                return await this.DbContext.Tenant.Include(i => i.TenantUrl).AsNoTracking()
                           .FirstOrDefaultWithNoLockAsync(f => f.Id == id && !f.Deleted);
            }

            return await this.DbContext.Tenant.AsNoTracking().FirstOrDefaultWithNoLockAsync(t => t.Id == id && !t.Deleted);
        }

        /// <inheritdoc/>
        public async Task<List<Tenant>> GetAllAsync(bool includeTenantUrls = false)
        {
            if (includeTenantUrls)
            {
                return await this.DbContext.Tenant.Include(i => i.TenantUrl).AsNoTracking().Where(w => !w.Deleted)
                           .AsNoTracking().ToListWithNoLockAsync();
            }

            return await this.DbContext.Tenant.Where(w => !w.Deleted).AsNoTracking().ToListWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task<TenantDescription> GetTenantDescriptionByUserId(int userId)
        {
            var tenant = new TenantDescription();

            var results = await (from t in this.DbContext.Tenant
                                 join tu in this.DbContext.TenantUrl on t.Id equals tu.TenantId
                                 join u in this.DbContext.User on t.Id equals u.PreferredTenantId
                                 where u.Id == userId && !u.Deleted
                                 orderby tu.Id ascending
                                 select new
                                 {
                                     description = t.Description,
                                     url = tu.UrlHostName,
                                 })
                      .ToListWithNoLockAsync();

            if (results != null)
            {
                tenant.Description = results.First().description;
                tenant.Url = results.First().url;
            }

            return tenant;
        }
    }
}
