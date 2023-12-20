// <copyright file="IExternalSystemUserRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface.LH
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// The External System User Repository interface.
    /// </summary>
    public interface IExternalSystemUserRepository : IGenericLHRepository<ExternalSystemUser>
    {
        /// <summary>
        /// Get external system user entity by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="externalSystemId">The external system id.</param>
        /// <returns>The <see cref="ExternalSystemUser"/>.</returns>
        Task<ExternalSystemUser> GetByIdAsync(int userId, int externalSystemId);
    }
}
