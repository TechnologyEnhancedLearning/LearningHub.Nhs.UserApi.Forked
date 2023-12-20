// <copyright file="IUserEmploymentRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserEmploymentRepository interface.
    /// </summary>
    public interface IUserEmploymentRepository : IGenericElfhRepository<UserEmployment>
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
        Task<UserEmployment> GetByIdAsync(int id);

        /// <summary>
        /// The get all with user.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<UserEmployment> GetAllWithUser();

        /// <summary>
        /// The get primary for user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserEmployment> GetPrimaryForUser(int userId);
    }
}
