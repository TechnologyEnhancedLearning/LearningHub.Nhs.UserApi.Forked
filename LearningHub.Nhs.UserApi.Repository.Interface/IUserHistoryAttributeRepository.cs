// <copyright file="IUserHistoryAttributeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The User History Attribute Repository interface.
    /// </summary>
    public interface IUserHistoryAttributeRepository
    {
        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userHistoryAttribute">The user history attribute.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<int> CreateAsync(int userId, UserHistoryAttribute userHistoryAttribute);
    }
}