// <copyright file="SecurityService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The security service.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private const int StandardUserGroupId = 1070;

        private const int GeneralUserGroupId = 1170;

        private readonly IUserHistoryService userHistoryService;

        /// <summary>
        /// The user password validation token repository.
        /// </summary>
        private IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private IElfhUserRepository elfhUserRepository;

        /// <summary>
        /// userUserGroupRepository.
        /// </summary>
        private IUserUserGroupRepository userUserGroupRepository;

        /// <summary>
        /// settings.
        /// </summary>
        private IOptions<Settings> settings;

        private IUserGroupTypeInputValidationRepository groupTypeInputValidationRepository;

        private IUserRoleUpgradeRepository userRoleUpgradeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityService"/> class.
        /// </summary>
        /// <param name="userHistoryService">The user user History Service.</param>
        /// <param name="userPasswordValidationTokenRepository">The user password validation token repository.</param>
        /// <param name="elfhUserRepository">The user repository.</param>
        /// <param name="userUserGroupRepository">userUserGroupRepository.</param>
        /// <param name="groupTypeInputValidationRepository">groupTypeInputValidationRepository.</param>
        /// <param name="userRoleUpgradeRepository">userRoleUpgradeRepository.</param>
        /// <param name="settings">settings.</param>
        public SecurityService(
         IUserHistoryService userHistoryService,
         IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository,
         IElfhUserRepository elfhUserRepository,
         IUserUserGroupRepository userUserGroupRepository,
         IUserGroupTypeInputValidationRepository groupTypeInputValidationRepository,
         IUserRoleUpgradeRepository userRoleUpgradeRepository,
         IOptions<Settings> settings)
        {
            this.userHistoryService = userHistoryService;
            this.userPasswordValidationTokenRepository = userPasswordValidationTokenRepository;
            this.elfhUserRepository = elfhUserRepository;
            this.userUserGroupRepository = userUserGroupRepository;
            this.groupTypeInputValidationRepository = groupTypeInputValidationRepository;
            this.userRoleUpgradeRepository = userRoleUpgradeRepository;
            this.settings = settings;
        }

        /// <inheritdoc/>
        public async Task<PasswordValidationTokenResult> ValidateTokenAsync(string token, string loctoken)
        {
            PasswordValidationTokenResult tokenResult = new PasswordValidationTokenResult() { Valid = false, TokenIssue = string.Empty };

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(loctoken))
            {
                tokenResult.TokenIssue = "Invalid";
            }

            var userPasswordValidationToken = await this.userPasswordValidationTokenRepository.GetByToken(loctoken);
            if (userPasswordValidationToken == null)
            {
                tokenResult.TokenIssue = "Invalid";
            }
            else
            {
                var hashedToken = this.SecureHash(token, userPasswordValidationToken.Salt);

                if (!userPasswordValidationToken.HashedToken.Equals(hashedToken))
                {
                    tokenResult.TokenIssue = "Invalid";
                }
                else if (userPasswordValidationToken.HashedToken.Equals(hashedToken) && userPasswordValidationToken.Expiry < DateTimeOffset.Now)
                {
                    tokenResult.TokenIssue = "Expired";
                    await this.userPasswordValidationTokenRepository.ExpireUserPasswordValidationToken(userPasswordValidationToken.Lookup);
                }
                else
                {
                    tokenResult.UserName = userPasswordValidationToken.User.UserName;
                    tokenResult.Valid = true;
                }
            }

            return tokenResult;
        }

        /// <inheritdoc/>
        public async Task<bool> SetInitialPasswordAsync(PasswordCreateModel passwordCreateModel)
        {
            var result = await this.ValidateTokenAsync(passwordCreateModel.Token, passwordCreateModel.Loctoken);

            if (!result.Valid)
            {
                return false;
            }
            else
            {
                var user = await this.elfhUserRepository.GetByUsernameAsync(result.UserName);
                if (user != null)
                {
                    user.PasswordHash = passwordCreateModel.PasswordHash;
                    user.PasswordLifeCounter = 0;
                    user.SecurityLifeCounter = 0;
                    user.MustChangeNextLogin = false;
                    user.AmendUserId = user.Id;

                    await this.elfhUserRepository.UpdateAsync(user.Id, user);

                    await this.userPasswordValidationTokenRepository.ExpireUserPasswordValidationToken(passwordCreateModel.Loctoken);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// UpgradeAsFullAccessUser.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <param name="email">email.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> UpgradeAsFullAccessUser(int userId, string email)
        {
            elfhHub.Nhs.Models.Entities.User user = await this.elfhUserRepository.GetByIdAsync(userId);
            user.EmailAddress = email;
            await this.elfhUserRepository.UpdateAsync(userId, user);
            var userRoleUpgrade = await this.userRoleUpgradeRepository.GetByUserIdAsync(userId).Where(n => n.UpgradeDate == null).FirstOrDefaultWithNoLockAsync();
            userRoleUpgrade.UpgradeDate = DateTimeOffset.Now;
            await this.userRoleUpgradeRepository.UpdateAsync(userId, userRoleUpgrade);

            // If the user is currently a green user, check if the new email address qualifies them as a blue user.
            bool isBlueUser = await this.userUserGroupRepository.HasUserBeenAssignedToUserGroup(userId, StandardUserGroupId); // 1070 = e-LfH Standard User Type - Blue
            if (!isBlueUser)
            {
                bool isBlueUserFromEmail = this.groupTypeInputValidationRepository.IsEmailValidForUserGroup(email, StandardUserGroupId);
                if (isBlueUserFromEmail)
                {
                    var entity = new elfhHub.Nhs.Models.Entities.UserUserGroup { UserId = userId, UserGroupId = StandardUserGroupId };
                    await this.userUserGroupRepository.CreateAsync(userId, entity);
                    await this.userUserGroupRepository.DeleteUserGroupAsync(userId, GeneralUserGroupId);

                    // Add UserHistory entry
                    UserHistoryViewModel userHistory = new UserHistoryViewModel()
                    {
                        UserId = userId,
                        Detail = "User role upgraded.",
                        UserHistoryTypeId = (int)UserHistoryType.UserRoleUpgarde,
                    };
                    var uhCreateResult = await this.userHistoryService.CreateAsync(userHistory, userId);
                }
            }

            return true;
        }

        private string SecureHash(string value, string salt)
        {
            return this.Hash(value, salt);
        }

        private string Hash(string value, string salt)
        {
            var i = salt.IndexOf('.');
            var iterations = int.Parse(salt.Substring(0, i), System.Globalization.NumberStyles.HexNumber);
            salt = salt.Substring(i + 1);

            using (var pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(value), Convert.FromBase64String(salt), iterations))
            {
                var key = pbkdf2.GetBytes(24);

                return Convert.ToBase64String(key);
            }
        }
    }
}