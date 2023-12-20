// <copyright file="IJobRoleRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The JobRoleRepository interface.
    /// </summary>
    public interface IJobRoleRepository : IGenericElfhRepository<JobRole>
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
        Task<JobRole> GetByIdAsync(int id);

        /// <summary>
        /// The get all with staff group.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<JobRole> GetAllWithStaffGroup();
    }
}
