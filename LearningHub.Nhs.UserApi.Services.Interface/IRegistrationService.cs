// <copyright file="IRegistrationService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The RegistrationService interface.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// The get registration status.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <param name="ipAddress">The user ip address.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<EmailRegistrationStatus> GetRegistrationStatus(string emailAddress, string ipAddress);

        /// <summary>
        /// Link existing user to sso.
        /// </summary>
        /// <param name="userId">Existing user id.</param>
        /// <param name="externalSystemId">External system id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task LinkExistingUserToSso(int userId, int externalSystemId);

        /// <summary>
        /// Sync sso external user to elfh.
        /// </summary>
        /// <param name="userId">Existing user id.</param>
        /// <param name="externalSystemCode">External system code.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SyncSsoUsertoElfh(int userId, string externalSystemCode);

        /// <summary>
        /// The register user.
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        /// <returns>The <see cref="LearningHubValidationResult"/>.</returns>
        Task<LearningHubValidationResult> RegisterUser(RegistrationRequestViewModel registrationRequest);

        /// <summary>
        /// The get email status.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> GetEmailStatus(string emailAddress);
    }
}
