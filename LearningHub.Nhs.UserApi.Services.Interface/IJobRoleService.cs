namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The JobRoleService interface.
    /// </summary>
    public interface IJobRoleService
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
        Task<JobRoleBasicViewModel> GetByIdAsync(int id);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<JobRole> GetAll();

        /// <summary>
        /// The get filtered with staff group async.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<JobRoleBasicViewModel>> GetFilteredWithStaffGroupAsync(string filter);

        /// <summary>
        /// The get job roles by staff group id.
        /// </summary>
        /// <param name="staffGroupId">The staffGroupId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<JobRoleBasicViewModel>> GetByStaffGroupIdAsync(int staffGroupId);
    }
}
