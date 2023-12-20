// <copyright file="IUserUserGroupRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserUserGroupRepository interface.
    /// </summary>
    public interface IUserUserGroupRepository : IGenericElfhRepository<UserUserGroup>
    {
        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userUserGroupId">
        /// The userUserGroup id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DeleteAsync(int userId, int userUserGroupId);

        /// <summary>
        /// The HasUserBeenAssignedToUserGroup.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userGroupId">
        /// The user group id.
        /// </param>
        /// <returns>A boolean indicating whether the user has been assigned to the user group.</returns>
        Task<bool> HasUserBeenAssignedToUserGroup(int userId, int userGroupId);

        /// <summary>
        /// The delete users user group async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userGroupId">
        /// The user group id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DeleteUserGroupAsync(int userId, int userGroupId);
    }
}
