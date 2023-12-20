// <copyright file="IElfhUserRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Dto;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Shared;

    /// <summary>
    /// The Elfh User Repository interface.
    /// </summary>
    public interface IElfhUserRepository : IGenericElfhRepository<User>
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="includeCollections">
        /// Columns to include.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<User> GetByIdAsync(int id, UserIncludeCollectionsEnum[] includeCollections = null);

        /// <summary>
        /// The get by username async.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="includeCollections">
        /// Columns to include.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<User> GetByUsernameAsync(string username, UserIncludeCollectionsEnum[] includeCollections = null);

        /// <summary>
        /// The get user id by username async.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> GetUserIdByUsernameAsync(string username);

        /// <summary>
        /// The get user detail for the authentication.
        /// </summary>
        /// <param name = "username">
        /// username.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserAuthenticateDto> GetUserDetailForAuthentication(string username);

        /// <summary>
        /// The get by open athens id.
        /// </summary>
        /// <param name="openAthensId">
        /// The open athens id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<User> GetByOpenAthensIdAsync(string openAthensId);

        /// <summary>
        /// The does email exist async.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> DoesEmailExistAsync(string emailAddress);

        /// <summary>
        /// The number of users for email async.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> NumberOfUsersForEmailAsync(string emailAddress);

        /// <summary>
        /// The does user name exist.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DoesUserNameExist(string username);

        /// <summary>
        /// The get users for email.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns>The queryable of users.</returns>
        IQueryable<User> GetUsersForEmail(string emailAddress);

        /// <summary>
        /// The does user exist.
        /// </summary>
        /// <param name="medicalCouncilId">
        /// The medical council id.
        /// </param>
        /// <param name="medicalCouncilPrifix">
        /// The medical council prifix.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool DoesUserExist(int medicalCouncilId, string medicalCouncilPrifix, string medicalCouncilNumber, string emailAddress);

        /// <summary>
        /// The get all with user employment.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<User> GetAllWithUserEmployment();

        /// <summary>
        /// The is e integrity user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsEIntegrityUser(int userId);

        /// <summary>
        /// The is basic user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsBasicUser(int userId);

        /// <summary>
        /// Returns indication of whether the user in an Admin.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsAdminUser(int userId);

        /// <summary>
        /// Returns indication of whether the user a Learning Hub read-only user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsLearningHubReadOnlyUser(int userId);

        /// <summary>
        /// The Link Employment Record to User async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// Nothing.
        /// </returns>
        Task LinkEmploymentRecordToUser(int userId);
    }
}