// <copyright file="ITermsAndConditionsService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The TermsAndConditionsService interface.
    /// </summary>
    public interface ITermsAndConditionsService
    {
        /// <summary>
        /// The get latest version async.
        /// </summary>
        /// <param name="tenantId">
        /// The tenant id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TermsAndConditions> GetLatestVersionAsync(int tenantId);

        /// <summary>
        /// The accept by user.
        /// </summary>
        /// <param name="termsAndConditions">
        /// The terms and conditions.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> AcceptByUser(UserTermsAndConditionsViewModel termsAndConditions, int userId);
    }
}
