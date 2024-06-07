namespace LearningHub.Nhs.UserApi.Repository.Interface.LH
{
    using System.Linq;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// IExternalSystemRepository.
    /// </summary>
    public interface IExternalSystemRepository : IGenericLHRepository<ExternalSystem>
    {
        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<ExternalSystem> GetByCode(string clientCode);

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<ExternalSystem> GetExtSystemById(int id);

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        IQueryable<ExternalSystem> GetExternalSystems();
    }
}
