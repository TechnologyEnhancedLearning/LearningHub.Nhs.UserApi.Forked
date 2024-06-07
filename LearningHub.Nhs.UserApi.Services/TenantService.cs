namespace LearningHub.Nhs.UserApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The tenant service.
    /// </summary>
    public class TenantService : ITenantService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<TenantService> logger;

        /// <summary>
        /// The tenant repository.
        /// </summary>
        private readonly ITenantRepository tenantRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantService"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="tenantRepository">
        /// The tenant repository.
        /// </param>
        public TenantService(
            ILogger<TenantService> logger,
            ITenantRepository tenantRepository)
        {
            this.logger = logger;
            this.tenantRepository = tenantRepository;
        }

        /// <inheritdoc/>
        public async Task<Tenant> GetByIdAsync(int id, bool includeTenantUrls)
        {
            return await this.tenantRepository.GetByIdAsync(id, includeTenantUrls);
        }

        /// <inheritdoc/>
        public async Task<IList<Tenant>> GetAllAsync(bool includeTenantUrls)
        {
            return await this.tenantRepository.GetAllAsync(includeTenantUrls);
        }
    }
}