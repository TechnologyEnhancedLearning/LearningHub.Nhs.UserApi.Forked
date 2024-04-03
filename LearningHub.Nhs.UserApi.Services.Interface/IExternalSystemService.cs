namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The External system service interface.
    /// </summary>
    public interface IExternalSystemService
    {
        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<ExternalSystem> GetByCodeAsync(string clientCode);

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<ExternalSystem> GetExtSystemById(int id);

        /// <summary>
        /// Get external systems.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<PagedResultSet<ExternalSystem>> GetExternalSystems(int page, int pageSize, string sortColumn = "", string sortDirection = "", string filter = "");

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="externalSystem">
        /// The externalSystem.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> UpdateAsync(int userId, ExternalSystem externalSystem);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="externalSystem">
        /// The externalSystem.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> CreateAsync(int userId, ExternalSystem externalSystem);
    }
}
