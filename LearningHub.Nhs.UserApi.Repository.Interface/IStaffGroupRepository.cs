namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The StaffGroupRepository interface.
    /// </summary>
    public interface IStaffGroupRepository : IGenericElfhRepository<StaffGroup>
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
        Task<StaffGroup> GetByIdAsync(int id);
    }
}
