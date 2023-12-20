// <copyright file="IUserApiHttpClient.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Interfaces
{
    using System.Net.Http;

    /// <summary>
    /// The UserApiHttpClient interface.
    /// </summary>
    public interface IUserApiHttpClient
    {
        /// <summary>
        /// The get client.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpClient"/>.
        /// </returns>
        HttpClient GetClient();
    }
}
