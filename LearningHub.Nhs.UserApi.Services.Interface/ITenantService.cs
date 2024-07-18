namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The TenantService interface.
    /// </summary>
    public interface ITenantService
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <param name="includeTenantUrls">
        ///     The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Tenant> GetByIdAsync(int id, bool includeTenantUrls);

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="includeTenantUrls">
        ///     The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<IList<Tenant>> GetAllAsync(bool includeTenantUrls);
    }
}