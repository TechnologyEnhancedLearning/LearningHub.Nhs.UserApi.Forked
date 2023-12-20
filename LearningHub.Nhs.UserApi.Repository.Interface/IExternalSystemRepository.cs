// <copyright file="IExternalSystemRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// IExternalSystemRepository.
    /// </summary>
    public interface IExternalSystemRepository : IGenericElfhRepository<ExternalSystem>
    {
        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="externalSystemCode">externalSystemCode.</param>
        /// <returns>
        /// The ExternalSystem.
        /// </returns>
        ExternalSystem GetByCode(string externalSystemCode);
    }
}
