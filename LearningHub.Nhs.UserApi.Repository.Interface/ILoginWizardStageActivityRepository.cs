// <copyright file="ILoginWizardStageActivityRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The LoginWizardStageActivityRepository interface.
    /// </summary>
    public interface ILoginWizardStageActivityRepository
    {
        /// <summary>
        /// The get by user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<LoginWizardStageActivity> GetByUser(int userId);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="loginWizardStageActivity">
        /// The login wizard stage activity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> CreateAsync(LoginWizardStageActivity loginWizardStageActivity);
    }
}
