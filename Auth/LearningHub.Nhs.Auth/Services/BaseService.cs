// <copyright file="BaseService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Services
{
    using LearningHub.Nhs.Auth.Interfaces;

    /// <summary>
    /// The base service.
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Gets or sets the learning hub http client.
        /// </summary>
        protected IUserApiHttpClient UserApiHttpClient { get; set; }
    }
}
