namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The MedicalCouncilRepository interface.
    /// </summary>
    public interface IMedicalCouncilRepository : IGenericElfhRepository<MedicalCouncil>
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<MedicalCouncil> GetByIdAsync(int id);
    }
}
