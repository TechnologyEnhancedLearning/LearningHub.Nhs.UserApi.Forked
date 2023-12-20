// <copyright file="IExternalSystemDeepLinkRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface.LH
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// The External System DeepLink Repository interface.
    /// </summary>
    public interface IExternalSystemDeepLinkRepository : IGenericLHRepository<ExternalSystemDeepLink>
    {
        /// <summary>
        /// Get external system deep link entity by code.
        /// </summary>
        /// <param name="code">The end client code.</param>
        /// <returns>The <see cref="ExternalSystemDeepLink"/>.</returns>
        Task<ExternalSystemDeepLink> GetByCodeAsync(string code);
    }
}