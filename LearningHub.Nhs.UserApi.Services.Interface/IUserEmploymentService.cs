// <copyright file="IUserEmploymentService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The UserEmploymentService interface.
    /// </summary>
    public interface IUserEmploymentService
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
        /// The get primary by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserEmploymentViewModel> GetPrimaryForUser(int id);

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userEmployment">
        /// The user employment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> UpdateAsync(int userId, UserEmploymentViewModel userEmployment);
    }
}
