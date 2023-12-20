// <copyright file="ITermsAndConditionsRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The TermsAndConditionsRepository interface.
    /// </summary>
    public interface ITermsAndConditionsRepository : IGenericElfhRepository<TermsAndConditions>
    {
        /// <summary>
        /// The latest version async.
        /// </summary>
        /// <param name="tenantId">
        /// The tenant id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TermsAndConditions> LatestVersionAsync(int tenantId);

        /// <summary>
        /// The latest version accepted async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="learningHubTenantId">
        /// The learning hub tenant id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TermsAndConditions> LatestVersionAcceptedAsync(int userId, int learningHubTenantId);
    }
}
