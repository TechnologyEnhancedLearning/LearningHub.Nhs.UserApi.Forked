namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The StaffGroupService interface.
    /// </summary>
    public interface IStaffGroupService
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

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<StaffGroup> GetAll();
    }
}
