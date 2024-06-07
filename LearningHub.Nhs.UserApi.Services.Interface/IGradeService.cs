namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The GradeService interface.
    /// </summary>
    public interface IGradeService
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
        Task<Grade> GetByIdAsync(int id);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Grade> GetAll();

        /// <summary>
        /// The get by job role.
        /// </summary>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<Grade>> GetByJobRole(int jobRoleId);
    }
}
