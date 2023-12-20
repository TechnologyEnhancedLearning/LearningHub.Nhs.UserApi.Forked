// <copyright file="IGradeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The GradeRepository interface.
    /// </summary>
    public interface IGradeRepository : IGenericElfhRepository<Grade>
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
        /// The get by job role.
        /// </summary>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<Grade> GetByJobRole(int jobRoleId);
    }
}
