namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The TenantRepository interface.
    /// </summary>
    public interface ITenantRepository : IGenericElfhRepository<Tenant>
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="includeTenantUrls">
        /// The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Tenant> GetByIdAsync(
            int id,
            bool includeTenantUrls = false);

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="includeTenantUrls">
        /// The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<Tenant>> GetAllAsync(bool includeTenantUrls = false);

        /// <summary>
        /// The get all by user id async.
        /// </summary>
        /// <param name="userId">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TenantDescription> GetTenantDescriptionByUserId(int userId);
    }
}
