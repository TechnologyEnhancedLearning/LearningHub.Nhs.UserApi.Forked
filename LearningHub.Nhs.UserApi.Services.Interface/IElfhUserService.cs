// <copyright file="IElfhUserService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Dto;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.OpenAthens;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Shared;

    /// <summary>
    /// The Elfh UserService interface.
    /// </summary>
    public interface IElfhUserService
    {
        /// <summary>
        /// The sync lh user async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SyncLHUserAsync(int id, string userName);

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="includeCollections">The include Collections.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<User> GetByIdAsync(int id, UserIncludeCollectionsEnum[] includeCollections = null);

        /// <summary>
        /// The get basic profile by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserBasic> GetBasicProfileByIdAsync(int id);

        /// <summary>
        /// The get by username async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="includeCollections">The include Collections.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<User> GetByUsernameAsync(string userName, UserIncludeCollectionsEnum[] includeCollections = null);

        /// <summary>
        /// The get user id by username async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<int> GetUserIdByUsernameAsync(string userName);

        /// <summary>
        /// The get user details for the authenticat by username.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserAuthenticateDto> GetUserDetailForAuthenticateAsync(string userName);

        /// <summary>
        /// The get by open athens id.
        /// </summary>
        /// <param name="openAthensId">The open athens id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserBasic> GetByOpenAthensIdAsync(string openAthensId);

        /// <summary>
        /// The get user role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<string> GetUserRoleAsync(int id);

        /// <summary>
        /// The record successful signin async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RecordSuccessfulSigninAsync(int id, CancellationToken token = default);

        /// <summary>
        /// The record unsuccessful signin async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="token">The <see cref="CancellationToken"/> token.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RecordUnsuccessfulSigninAsync(int id, CancellationToken token = default);

        /// <summary>
        /// The user already exists.
        /// </summary>
        /// <param name="medicalCouncilId">
        /// The medical council id.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> UserAlreadyExistsAsync(int medicalCouncilId, string medicalCouncilNumber, string emailAddress);

        /// <summary>
        /// The create user name.
        /// </summary>
        /// <param name="medicalCouncilId">
        /// The medical council id.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <param name="lastname">
        /// The lastname.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<string> CreateUserNameAsync(int medicalCouncilId, string medicalCouncilNumber, string lastname);

        /// <summary>
        /// The update current user password.
        /// </summary>
        /// <param name="newPassword">
        /// The new password.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateCurrentUserPassword(string newPassword, int currentUserId);

        /// <summary>
        /// The update login wizard flag.
        /// </summary>
        /// <param name="updateLoginWizardInProgress">
        /// The update login wizard in progress.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateLoginWizardFlag(bool updateLoginWizardInProgress, int currentUserId);

        /// <summary>
        /// The update user security questions.
        /// </summary>
        /// <param name="userSecurityQuestions">
        /// The user security questions.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateUserSecurityQuestions(List<UserSecurityQuestionViewModel> userSecurityQuestions, int currentUserId);

        /// <summary>
        /// The update personal details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdatePersonalDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update user's first name.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateFirstName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update user's last name details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateLastName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update user's preferred name.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdatePreferredName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// GetPersonalDetailsAsync.
        /// </summary>
        /// <param name="currentUserId">currentUserId.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<PersonalDetailsViewModel> GetPersonalDetailsAsync(int currentUserId);

            /// <summary>
            /// The does email address exist async.
            /// </summary>
            /// <param name="emailAddress">
            /// The email address.
            /// </param>
            /// <returns>
            /// The <see cref="Task"/>.
            /// </returns>
        Task<bool> DoesEmailAddressExistAsync(string emailAddress);

        /// <summary>
        /// The create open athens user with basic info async.
        /// </summary>
        /// <param name="newUserDetails">
        /// The new user details.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> CreateOpenAthensUserWithBasicInfoAsync(CreateOpenAthensLinkToLhUser newUserDetails);

        /// <summary>
        /// The create open athens link to user async.
        /// </summary>
        /// <param name="linkDetails">
        /// The link details.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> CreateOpenAthensLinkToUserAsync(OpenAthensToElfhUserLinkDetails linkDetails);

        /// <summary>
        /// Returns a list of users - filtered, sorted and paged as required.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<PagedResultSet<UserAdminBasicViewModel>> GetUserAdminBasicPageAsync(int page, int pageSize, string sortColumn = "", string sortDirection = "", string filter = "");

        /// <summary>
        /// Returns a user detail view model for the supplied id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>The <see cref="UserAdminDetailViewModel"/>.</returns>
        Task<UserAdminDetailViewModel> GetUserAdminDetailByIdAsync(int id);

        /// <summary>
        /// The get all by user id async.
        /// </summary>
        /// <param name="userId">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<TenantDescription> GetTenantDescriptionByUserId(int userId);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="userAdminDetailViewModel">
        /// The user admin detail view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<(LearningHubValidationResult result, bool permissionNotification)> UpdateUserAsync(UserAdminDetailViewModel userAdminDetailViewModel, int currentUserId);

        /// <summary>
        /// Sends an admin password reset email.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> SendAdminPasswordResetEmail(int userId);

        /// <summary>
        /// The send forgot password email.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task SendForgotPasswordEmail(string emailAddress);

        /// <summary>
        /// Sends email to user.
        /// </summary>
        /// <param name="vm">
        /// The user contact view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task SendEmailToUserAsync(UserContactViewModel vm);

        /// <summary>
        /// The has multiple users for email async.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> HasMultipleUsersForEmailAsync(string emailAddress);

        /// <summary>
        /// Link Employment Record to User.
        /// </summary>
        /// <param name="userId">
        /// The userId.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task LinkEmploymentRecordToUser(int userId);

        /// <summary>
        /// The update email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateEmailDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update primary email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdatePrimaryEmail(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update secondary email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateSecondaryEmail(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update location details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateLocationDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update country details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateCountryDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// The update region details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateRegionDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId);

        /// <summary>
        /// Get User Role Upgrade.
        /// </summary>
        /// <param name="userId">the userId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<UserRoleUpgrade> GetUserRoleUpgrade(int userId);

        /// <summary>
        /// The create user role upgrade.
        /// </summary>
        /// <param name="userRoleUpgrade">
        /// The emailaddress.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateUserRoleUpgrade(UserRoleUpgrade userRoleUpgrade);

        /// <summary>
        /// The update user role upgrade.
        /// </summary>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateUserRoleUpgrade(int currentUserId);

        /// <summary>
        /// The update user role upgrade.
        /// </summary>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateUserPrimaryEmailAsync(int currentUserId, string email);

        /// <summary>
        /// To check the status of user role upgrade.
        /// </summary>
        /// <param name="currentPrimaryEmail">
        /// The current primary email address.
        /// </param>
        /// <param name="newPrimaryEmail">New email address.</param>
        /// <param name="currentUserId">currentUserId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> ValidateUserRoleUpgrade(string currentPrimaryEmail, string newPrimaryEmail, int currentUserId);

        /// <summary>
        /// To check same primary email is pending to validate..
        /// </summary>
        /// <param name="secondaryEmail">
        /// The current secondary email address.
        /// </param>
        /// <param name="currentUserId">currentUserId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> CheckSamePrimaryemailIsPendingToValidate(string secondaryEmail, int currentUserId);
  }
}