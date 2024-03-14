namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The SystemSettingRepository interface.
    /// </summary>
    public interface ISystemSettingRepository : IGenericElfhRepository<SystemSetting>
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
        Task<SystemSetting> GetByIdAsync(int id);
    }
}
