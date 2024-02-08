namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The TenantSmtpRepository interface.
    /// </summary>
    public interface ITenantSmtpRepository : IGenericElfhRepository<TenantSmtp>
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
        Task<TenantSmtp> GetByIdAsync(int id);
    }
}
