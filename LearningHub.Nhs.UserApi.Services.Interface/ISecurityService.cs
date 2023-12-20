// <copyright file="ISecurityService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Common;

    /// <summary>
    /// The SecurityService interface.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// The validate token async.
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="loctoken">
        /// The loctoken.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<PasswordValidationTokenResult> ValidateTokenAsync(string token, string loctoken);

        /// <summary>
        /// The set initial password async.
        /// </summary>
        /// <param name="passwordCreateModel">
        /// The password create model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> SetInitialPasswordAsync(PasswordCreateModel passwordCreateModel);

        /// <summary>
        /// The set initial password async.
        /// </summary>
        /// <param name="userId">
        /// The puser Id.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> UpgradeAsFullAccessUser(int userId, string email);
    }
}
