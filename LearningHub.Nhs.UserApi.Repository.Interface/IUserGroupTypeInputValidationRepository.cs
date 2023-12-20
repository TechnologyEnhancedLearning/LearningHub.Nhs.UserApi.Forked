// <copyright file="IUserGroupTypeInputValidationRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserGroupTypeInputValidationRepository interface.
    /// </summary>
    public interface IUserGroupTypeInputValidationRepository : IGenericElfhRepository<UserGroupTypeInputValidation>
    {
        /// <summary>
        /// The is email valid for user group.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <param name="userGroupId">
        /// The user group id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsEmailValidForUserGroup(string emailAddress, int userGroupId);
    }
}
