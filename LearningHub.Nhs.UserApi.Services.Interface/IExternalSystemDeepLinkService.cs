// <copyright file="IExternalSystemDeepLinkService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// The External system deeplink service interface.
    /// </summary>
    public interface IExternalSystemDeepLinkService
    {
        /// <summary>
        /// Get external system deep link entity by code.
        /// </summary>
        /// <param name="code">The end client code.</param>
        /// <returns>The <see cref="ExternalSystemDeepLink"/>.</returns>
        Task<ExternalSystemDeepLink> GetByCodeAsync(string code);
    }
}