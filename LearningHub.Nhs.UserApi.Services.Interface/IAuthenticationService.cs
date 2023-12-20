// <copyright file="IAuthenticationService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Common;
    using Login = elfhHub.Nhs.Models.Common.Login;

    /// <summary>
    /// The AuthenticationService interface.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// The authenticate async.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<LoginResultInternal> AuthenticateAsync(Login login);

        /// <summary>
        /// The check user credentials sso async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The <see cref="LoginResultInternal"/>.</returns>
        Task<LoginResultInternal> CheckUserCredentialsSsoAsync(int userId);

        /// <summary>
        /// The check user credentials.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LoginResultInternal> CheckUserCredentialsAsync(Login login);
    }
}