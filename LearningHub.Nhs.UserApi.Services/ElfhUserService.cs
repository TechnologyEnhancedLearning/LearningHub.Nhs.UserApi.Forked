// <copyright file="ElfhUserService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Dto;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.OpenAthens;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Repository.Interface.LH;
    using LearningHub.Nhs.UserApi.Services.Helpers;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Services.Models;
    using LearningHub.Nhs.UserApi.Shared;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using UserValidator = elfhHub.Nhs.Models.Validation.UserValidator;

    /// <summary>
    /// The Elfh User Service.
    /// </summary>
    public class ElfhUserService : IElfhUserService
    {
        private const int READONLYUSERGROUPID = 3066;

        private readonly IElfhUserRepository elfhUserRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUserRoleUpgradeRepository userRoleUpgradeRepository;
        private readonly IUserUserGroupRepository userUserGroupRepository;
        private readonly IMedicalCouncilRepository medicalCouncilRepository;
        private readonly IUserSecurityQuestionRepository userSecurityQuestionRepository;
        private readonly IUserRepository lhUserRepository;
        private readonly IUserAttributeRepository userAttributeRepository;
        private readonly IUserEmploymentRepository userEmploymentRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly ITenantSmtpRepository tenantSmtpRepository;
        private readonly ISystemSettingRepository systemSettingRepository;
        private readonly IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository;
        private readonly Repository.Interface.IEmailTemplateRepository emailTemplateRepository;
        private readonly IEmailLogRepository emailLogRepository;
        private readonly IUserHistoryService userHistoryService;
        private readonly IPasswordManagerService passwordManagerService;
        private readonly IUserGroupTypeInputValidationRepository userGroupTypeInputValidationRepository;
        private readonly ISecurityService securityService;
        private readonly IOptions<Settings> settings;
        private readonly IElfhRedisCache elfhCache;
        private readonly IMapper mapper;
        private readonly ILogger<ElfhUserService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhUserService"/> class.
        /// </summary>
        /// <param name="elfhUserRepository">The user repository.</param>
        /// <param name="userGroupRepository">The user group repository.</param>
        /// <param name="userUserGroupRepository">The user usergroup repository.</param>
        /// <param name="userRoleUpgradeRepository">The user role upgarde repository.</param>
        /// <param name="lhUserRepository">The LH user repository.</param>
        /// <param name="userAttributeRepository">The user atribute repository.</param>
        /// <param name="userEmploymentRepository">The user employment repository.</param>
        /// <param name="medicalCouncilRepository">The medical council repository.</param>
        /// <param name="userSecurityQuestionRepository">The userSecurityQuestion repository.</param>
        /// <param name="tenantRepository">The tenant repository.</param>
        /// <param name="tenantSmtpRepository">The tenant smtp repository.</param>
        /// <param name="systemSettingRepository">The systemSetting repository.</param>
        /// <param name="userPasswordValidationTokenRepository">The userPasswordValidationToken repository.</param>
        /// <param name="emailTemplateRepository">The emailTemplate repository.</param>
        /// <param name="emailLogRepository">The emailLog repository.</param>
        /// <param name="userGroupTypeInputValidationRepository">The userGroupTypeInputValidation Repository.</param>
        /// <param name="userHistoryService">The user history service.</param>
        /// <param name="passwordManagerService">The password manager service.</param>
        /// <param name="securityService">securityService.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="elfhCache">The ELFH cache.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ElfhUserService(
            IElfhUserRepository elfhUserRepository,
            IUserGroupRepository userGroupRepository,
            IUserUserGroupRepository userUserGroupRepository,
            IUserRoleUpgradeRepository userRoleUpgradeRepository,
            IUserRepository lhUserRepository,
            IUserAttributeRepository userAttributeRepository,
            IUserEmploymentRepository userEmploymentRepository,
            IMedicalCouncilRepository medicalCouncilRepository,
            IUserSecurityQuestionRepository userSecurityQuestionRepository,
            ITenantRepository tenantRepository,
            ITenantSmtpRepository tenantSmtpRepository,
            ISystemSettingRepository systemSettingRepository,
            IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository,
            Repository.Interface.IEmailTemplateRepository emailTemplateRepository,
            IEmailLogRepository emailLogRepository,
            IUserGroupTypeInputValidationRepository userGroupTypeInputValidationRepository,
            IUserHistoryService userHistoryService,
            IPasswordManagerService passwordManagerService,
            ISecurityService securityService,
            IOptions<Settings> settings,
            IElfhRedisCache elfhCache,
            IMapper mapper,
            ILogger<ElfhUserService> logger)
        {
            this.elfhUserRepository = elfhUserRepository;
            this.userGroupRepository = userGroupRepository;
            this.userUserGroupRepository = userUserGroupRepository;
            this.userRoleUpgradeRepository = userRoleUpgradeRepository;
            this.lhUserRepository = lhUserRepository;
            this.userAttributeRepository = userAttributeRepository;
            this.userEmploymentRepository = userEmploymentRepository;
            this.medicalCouncilRepository = medicalCouncilRepository;
            this.userSecurityQuestionRepository = userSecurityQuestionRepository;
            this.tenantRepository = tenantRepository;
            this.tenantSmtpRepository = tenantSmtpRepository;
            this.systemSettingRepository = systemSettingRepository;
            this.userPasswordValidationTokenRepository = userPasswordValidationTokenRepository;
            this.emailTemplateRepository = emailTemplateRepository;
            this.emailLogRepository = emailLogRepository;
            this.userGroupTypeInputValidationRepository = userGroupTypeInputValidationRepository;
            this.userHistoryService = userHistoryService;
            this.passwordManagerService = passwordManagerService;
            this.securityService = securityService;
            this.settings = settings;
            this.elfhCache = elfhCache;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<UserBasic> GetBasicProfileByIdAsync(int id)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(id);

            return user?.ToBasicUser();
        }

        /// <inheritdoc/>
        public async Task<User> GetByIdAsync(int id, UserIncludeCollectionsEnum[] includeCollections = null)
        {
            return await this.elfhUserRepository.GetByIdAsync(id, includeCollections);
        }

        /// <inheritdoc/>
        public async Task<UserBasic> GetByOpenAthensIdAsync(string openAthensId)
        {
            if (string.IsNullOrEmpty(openAthensId))
            {
                return null;
            }

            var user = await this.elfhUserRepository.GetByOpenAthensIdAsync(openAthensId);

            if (user != null)
            {
                await this.SyncLHUserAsync(user.Id, user.UserName);
            }

            return user?.ToBasicUser();
        }

        /// <inheritdoc/>
        public async Task<User> GetByUsernameAsync(string userName, UserIncludeCollectionsEnum[] includeCollections = null)
        {
            return await this.elfhUserRepository.GetByUsernameAsync(userName, includeCollections);
        }

        /// <summary>
        /// The get user id by username async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task<int> GetUserIdByUsernameAsync(string userName)
        {
            return await this.elfhUserRepository.GetUserIdByUsernameAsync(userName);
        }

        /// <inheritdoc/>
        public async Task<UserAuthenticateDto> GetUserDetailForAuthenticateAsync(string userName)
        {
            return await this.elfhUserRepository.GetUserDetailForAuthentication(userName);
        }

        /// <inheritdoc/>
        public async Task<string> GetUserRoleAsync(int id)
        {
            var usergroups = await this.userGroupRepository.GetByUserAsync(id);

            if (usergroups.Exists(ug => ug.Id == 2))
            {
                // Administrator
                return "Administrator";
            }
            else if (usergroups.Exists(ug => ug.Id == 1070) && !usergroups.Exists(ug => ug.Id == 3066))
            {
                // Blue User
                return "BlueUser";
            }
            else if (usergroups.Exists(ug => ug.Id == 1170) && !usergroups.Exists(ug => ug.Id == 1070))
            {
                // Basic User
                return "BasicUser";
            }
            else if (usergroups.Exists(ug => ug.Id == 3066))
            {
                // Read Only
                return "ReadOnly";
            }
            else if (usergroups.Exists(ug => ug.Id == 1170))
            {
                // Basic User (Universal Access) - not eIntegrity
                return "BasicUser";
            }
            else
            {
                return "none";
            }
        }

        /// <inheritdoc/>
        public async Task RecordSuccessfulSigninAsync(int id, CancellationToken token = default)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(id);

            user.LoginTimes++;
            user.PasswordLifeCounter = 0;
            user.SecurityLifeCounter = 0;

            await this.elfhUserRepository.UpdateAsync(id, user);

            await this.InvalidateElfhUserCacheAsync(user.Id, user.UserName, token);
        }

        /// <inheritdoc/>
        public async Task RecordUnsuccessfulSigninAsync(int id, CancellationToken token = default)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(id);

            user.LoginTimes++;
            user.PasswordLifeCounter++;

            await this.elfhUserRepository.UpdateAsync(id, user);

            await this.InvalidateElfhUserCacheAsync(user.Id, user.UserName, token);
        }

        /// <inheritdoc/>
        public async Task SyncLHUserAsync(int userId, string userName)
        {
            var lhUser = await this.lhUserRepository.GetByIdAsync(userId);

            if (lhUser == null)
            {
                var newLhUser = new LearningHub.Nhs.Models.Entities.User
                {
                    Id = userId,
                    UserName = userName,
                    Deleted = false,
                };

                await this.lhUserRepository.CreateAsync(userId, newLhUser);
            }
            else if (userName != lhUser.UserName)
            {
                lhUser.UserName = userName;
                await this.lhUserRepository.UpdateAsync(userId, lhUser);
            }
        }

        /// <inheritdoc/>
        public async Task InvalidateElfhUserCacheAsync(int userId, string userName, CancellationToken cancellationToken)
        {
            if (userId == 0 || string.IsNullOrWhiteSpace(userName))
            {
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await Task.WhenAll(
                this.elfhCache.RemoveAsync(
                $"{this.settings.Value.ElfhCacheSettings.ElfhRedisKeyPrefix}{this.settings.Value.ElfhCacheSettings.ElfhUserLoadByUserIdKey}{userId}",
                cancellationToken),
                this.elfhCache.RemoveAsync(
                $"{this.settings.Value.ElfhCacheSettings.ElfhRedisKeyPrefix}{this.settings.Value.ElfhCacheSettings.ElfhUserLoadByUserNameKey}{userName}",
                cancellationToken));
        }

        /// <inheritdoc/>
        public async Task<bool> UserAlreadyExistsAsync(int medicalCouncilId, string medicalCouncilNumber, string emailAddress)
        {
            string medicalCouncilPrifix = string.Empty;
            if (medicalCouncilId > 0)
            {
                var medicalCouncil = await this.medicalCouncilRepository.GetByIdAsync(medicalCouncilId);
                medicalCouncilPrifix = medicalCouncil.UploadPrefix;
            }

            return this.elfhUserRepository.DoesUserExist(medicalCouncilId, medicalCouncilPrifix, medicalCouncilNumber, emailAddress);
        }

        /// <inheritdoc/>
        public async Task<string> CreateUserNameAsync(int medicalCouncilId, string medicalCouncilNumber, string lastName)
        {
            if (medicalCouncilId == 0 || string.IsNullOrEmpty(medicalCouncilNumber))
            {
                // Generate random username using Lastname
                string usernameStart = lastName.StripUnicodeCharactersFromString().Trim();
                if (usernameStart.Length > 5)
                {
                    usernameStart = usernameStart.Substring(0, 4);
                }

                bool validUserName = false;
                string username = string.Empty;
                while (!validUserName)
                {
                    username = usernameStart;
                    while (username.Length < 9)
                    {
                        Random random = new Random();
                        int number = random.Next(0, 9);
                        username = username + number.ToString();
                    }

                    // If username does not exist set validUserName to true
                    if (!this.elfhUserRepository.DoesUserNameExist(username))
                    {
                        validUserName = true;
                    }
                }

                return username.ToUpper();
            }
            else
            {
                var medicalCouncil = await this.medicalCouncilRepository.GetByIdAsync(medicalCouncilId);
                return (medicalCouncil.UploadPrefix.Trim() + medicalCouncilNumber.StripUnicodeCharactersFromString()).ToUpper();
            }
        }

        /// <inheritdoc/>
        public async Task UpdateCurrentUserPassword(string newPassword, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(currentUserId);
            user.PasswordHash = newPassword;
            user.PasswordLifeCounter = 0;
            user.SecurityLifeCounter = 0;
            user.MustChangeNextLogin = false;
            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task UpdateLoginWizardFlag(bool loginWizardInProgress, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(currentUserId);
            user.LoginWizardInProgress = !loginWizardInProgress;
            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task UpdateUserSecurityQuestions(List<UserSecurityQuestionViewModel> userSecurityQuestions, int currentUserId)
        {
            var questions = this.mapper.Map<IList<UserSecurityQuestion>>(userSecurityQuestions);
            foreach (var question in questions)
            {
                if (question.Id == 0)
                {
                    if (question.UserId == 0)
                    {
                        question.UserId = currentUserId;
                    }

                    await this.userSecurityQuestionRepository.CreateAsync(currentUserId, question);
                }
                else if (question.SecurityQuestionAnswerHash != "********")
                {
                    await this.userSecurityQuestionRepository.UpdateAsync(currentUserId, question);
                }
            }
        }

        /// <inheritdoc/>
        public async Task UpdatePersonalDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.FirstName = personalDetailsViewModel.FirstName;
            user.LastName = personalDetailsViewModel.LastName;
            user.PreferredName = personalDetailsViewModel.PreferredName;
            user.CountryId = personalDetailsViewModel.CountryId;
            user.RegionId = personalDetailsViewModel.RegionId;
            user.AltEmailAddress = personalDetailsViewModel.SecondaryEmailAddress;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task UpdateFirstName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.FirstName = personalDetailsViewModel.FirstName;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task UpdateLastName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.LastName = personalDetailsViewModel.LastName;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task UpdatePreferredName(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.PreferredName = personalDetailsViewModel.PreferredName;
            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task<PersonalDetailsViewModel> GetPersonalDetailsAsync(int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(currentUserId);
            UserRoleUpgrade userRoleUpgrade = await this.userRoleUpgradeRepository.GetByUserIdAsync(currentUserId).Where(n => n.UpgradeDate == null).FirstOrDefaultWithNoLockAsync();
            var upgradedEmailAddress = string.Empty;
            if (userRoleUpgrade != null)
            {
                upgradedEmailAddress = userRoleUpgrade.EmailAddress;
            }

            PersonalDetailsViewModel personalDetailsViewModel = new PersonalDetailsViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CountryId = user.CountryId,
                RegionId = user.RegionId,
                PreferredName = user.PreferredName,
                PrimaryEmailAddress = user.EmailAddress,
                SecondaryEmailAddress = user.AltEmailAddress,
                LastUpdated = user.AmendDate,
                PasswordHash = user.PasswordHash,
                NewPrimaryEmailAddress = upgradedEmailAddress,
            };

            return personalDetailsViewModel;
        }

        /// <inheritdoc/>
        public async Task<bool> DoesEmailAddressExistAsync(string emailAddress)
        {
            return await this.elfhUserRepository.DoesEmailExistAsync(emailAddress);
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> CreateOpenAthensUserWithBasicInfoAsync(CreateOpenAthensLinkToLhUser newUserDetails)
        {
            var retVal = await this.ValidateAsync(newUserDetails);
            if (retVal.IsValid)
            {
                if (string.IsNullOrWhiteSpace(newUserDetails.LastName) || string.IsNullOrWhiteSpace(newUserDetails.EmailAddress) ||
                string.IsNullOrWhiteSpace(newUserDetails.OaUserId) || string.IsNullOrWhiteSpace(newUserDetails.OaOrganisationId))
                {
                    retVal.IsValid = false;
                    retVal.Details.Add("Cannot create user, some parameters are missing or empty.");
                    return retVal;
                }

                var newUser = new User();

                newUser.UserName = await this.CreateUserNameAsync(0, string.Empty, newUserDetails.LastName);
                newUser.FirstName = string.IsNullOrEmpty(newUserDetails.FirstName) ? "oa-First-Name" : newUserDetails.FirstName;
                newUser.LastName = newUserDetails.LastName;
                newUser.EmailAddress = newUserDetails.EmailAddress;
                newUser.RestrictToSso = true;
                newUser.PasswordHash = this.passwordManagerService.Base64MD5HashDigest(this.passwordManagerService.Generate());
                newUser.Active = true;
                newUser.ActiveFromDate = DateTimeOffset.UtcNow;
                newUser.MustChangeNextLogin = false;
                newUser.CreatedDate = DateTimeOffset.Now;

                // var userId = await _userRepository.CreateOpenAthensUser(newUser);
                var userId = await this.elfhUserRepository.CreateAsync(0, newUser);

                if (userId < 1)
                {
                    this.logger.LogError($"OpenAthens user could not create a new ELFH account. Email: [{newUserDetails.EmailAddress}] - openAthensId [{newUserDetails.OaUserId}]");
                    retVal.IsValid = false;
                    retVal.Details.Add("OpenAthens user could not create an ELFH account.");
                    return retVal;
                }

                retVal.CreatedId = userId;

                if (!await this.userAttributeRepository.LinkOpenAthensAccountToElfhUser(new OpenAthensToElfhUserLinkDetails
                {
                    UserId = userId,
                    OaUserId = newUserDetails.OaUserId,
                    OaOrgId = newUserDetails.OaUserId,
                }))
                {
                    this.logger.LogError("OpenAthens ELFH user did not link to OpenAthens user attribute entries correctly. UserId: [{lhuserid}] - OpenAthens userId [{oaUserId}]", userId, newUserDetails.OaUserId);
                    retVal.IsValid = false;
                    retVal.Details.Add("OpenAthens ELFH user linking to OpenAthens user attributes failure.");
                    return retVal;
                }

                // Add UserHistory entry
                UserHistoryViewModel userHistory = new UserHistoryViewModel()
                {
                    UserId = userId,
                    Detail = "User created by OpenAthens registration",
                    UserHistoryTypeId = (int)UserHistoryType.SystemEvent,
                };
                var uhCreateResult = await this.userHistoryService.CreateAsync(userHistory, userId);

                if (!uhCreateResult.IsValid)
                {
                    return uhCreateResult;
                }

                // Add user to UserGroup
                UserUserGroup userUserGroup = new UserUserGroup()
                {
                    UserId = userId,
                    UserGroupId = 1070, // "1070 e-LfH Standard User Type - Blue"
                };
                await this.userUserGroupRepository.CreateAsync(userId, userUserGroup);

                // If we don't have an existing Employment record, create a dummy
                UserEmployment userEmployment = await this.userEmploymentRepository.GetPrimaryForUser(userId);

                if (userEmployment == null)
                {
                    int userEmploymentId = await this.userEmploymentRepository.CreateAsync(userId, new UserEmployment()
                    {
                        UserId = userId,
                        LocationId = 1,
                        AmendUserId = userId,
                        StartDate = DateTime.Now,
                    });

                    newUser.PrimaryUserEmploymentId = userEmploymentId;
                    await this.elfhUserRepository.UpdateAsync(newUser.Id, newUser);
                }
            }

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<bool> CreateOpenAthensLinkToUserAsync(OpenAthensToElfhUserLinkDetails linkDetails)
        {
            if (!await this.userAttributeRepository.LinkOpenAthensAccountToElfhUser(linkDetails))
            {
                this.logger.LogError("User did not link to OpenAthens user attribute entries correctly. UserId: [{lhuserid}] - OpenAthens userId [{oaUserId}]", linkDetails.UserId, linkDetails.OaUserId);
                throw new Exception("User linking to OpenAthens user attributes failure.");
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<PagedResultSet<UserAdminBasicViewModel>> GetUserAdminBasicPageAsync(int page, int pageSize, string sortColumn = "", string sortDirection = "", string filter = "")
        {
            var filterCriteria = JsonConvert.DeserializeObject<List<PagingColumnFilter>>(filter);

            PagedResultSet<UserAdminBasicViewModel> result = new PagedResultSet<UserAdminBasicViewModel>();

            var items = this.elfhUserRepository.GetAll().Where(u => u.Deleted == false);

            items = this.FilterItems(items, filterCriteria);

            result.TotalItemCount = await items.CountWithNoLockAsync();

            items = this.OrderItems(items, sortColumn, sortDirection);

            items = items.Skip((page - 1) * pageSize).Take(pageSize);

            result.Items = await this.mapper.ProjectTo<UserAdminBasicViewModel>(items).ToListWithNoLockAsync();

            return result;
        }

        /// <inheritdoc/>
        public async Task<UserAdminDetailViewModel> GetUserAdminDetailByIdAsync(int id)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(id, new[] { UserIncludeCollectionsEnum.UserRoleUpgrade });

            var userModel = this.mapper.Map<UserAdminDetailViewModel>(user);
            userModel.ReadOnlyUser = this.elfhUserRepository.IsLearningHubReadOnlyUser(id);
            userModel.RoleName = await this.GetUserRoleAsync(id);
            return userModel;
        }

        /// <inheritdoc/>
        public async Task<TenantDescription> GetTenantDescriptionByUserId(int userId)
        {
            return await this.tenantRepository.GetTenantDescriptionByUserId(userId);
        }

        /// <inheritdoc/>
        public async Task<(LearningHubValidationResult result, bool permissionNotification)> UpdateUserAsync(UserAdminDetailViewModel userAdminDetailViewModel, int currentUserId)
        {
            var permissionNotification = false;
            var userValidator = new UserValidator();
            var validationResult = await userValidator.ValidateAsync(userAdminDetailViewModel);
            var retVal = new LearningHubValidationResult(validationResult);
            if (retVal.IsValid)
            {
                User user = await this.elfhUserRepository.GetByIdAsync(userAdminDetailViewModel.Id);
                user.FirstName = userAdminDetailViewModel.FirstName;
                user.LastName = userAdminDetailViewModel.LastName;
                user.EmailAddress = userAdminDetailViewModel.EmailAddress;
                user.ActiveToDate = userAdminDetailViewModel.ActiveToDate;
                user.PasswordLifeCounter = userAdminDetailViewModel.FailedLoginCount;
                user.AltEmailAddress = userAdminDetailViewModel.AltEmailAddress;
                user.Active = userAdminDetailViewModel.Active;

                bool isReadOnlyUser = this.elfhUserRepository.IsLearningHubReadOnlyUser(userAdminDetailViewModel.Id);
                if (userAdminDetailViewModel.ReadOnlyUser && !isReadOnlyUser)
                {
                    await this.userUserGroupRepository.CreateAsync(currentUserId, new UserUserGroup() { UserId = userAdminDetailViewModel.Id, UserGroupId = READONLYUSERGROUPID });
                    permissionNotification = true;
                }
                else if (!userAdminDetailViewModel.ReadOnlyUser && isReadOnlyUser)
                {
                    var userUserGroup = this.userUserGroupRepository.GetAll().Single(x => x.UserId == userAdminDetailViewModel.Id && x.UserGroupId == READONLYUSERGROUPID && !x.Deleted);
                    await this.userUserGroupRepository.DeleteAsync(currentUserId, userUserGroup.Id);
                    permissionNotification = true;
                }

                await this.elfhUserRepository.UpdateAsync(currentUserId, user);
            }

            return (retVal, permissionNotification);
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> SendAdminPasswordResetEmail(int userId)
        {
            User user = await this.GetByIdAsync(userId);
            if (user == null)
            {
                return new LearningHubValidationResult(false, $"userId {userId} not found.");
            }

            // Send registration confirmation email
            /* This code was copied from the RegistrationService.RegisterUser method and tweaked for this email template.
             * The GenerateUserPasswordValidationToken method (and its child methods) are all direct copies of those found in
             * the RegistrationService. This is definitely a candidate for turning into a shared component after RR. */
            var expiryMinutes = (int)(await this.systemSettingRepository.GetByIdAsync((int)SystemSettingEnum.PasswordValidationExpiryAdminDefault)).IntValue;
            UserPasswordValidationTokenExtended userPasswordValidationToken = this.GenerateUserPasswordValidationToken(expiryMinutes, userId);
            await this.userPasswordValidationTokenRepository.CreateAsync(userId, userPasswordValidationToken);

            string websiteEmailsFrom = (await this.tenantSmtpRepository.GetByIdAsync(this.settings.Value.LearningHubTenantId)).From;
            EmailTemplate emailTemplate = await this.emailTemplateRepository.GetByTypeAndTenantAsync((int)EmailTemplateTypeEnum.AdminPasswordValidateEmail, this.settings.Value.LearningHubTenantId);
            string bodyText = emailTemplate.Body;
            bodyText = bodyText.Replace("[FullName]", (user.FirstName + " " + user.LastName).ToTitleCase());
            bodyText = bodyText.Replace("[FirstName]", user.FirstName.ToTitleCase());
            bodyText = bodyText.Replace("[UserName]", user.UserName);
            bodyText = bodyText.Replace("[PasswordValidateUrl]", userPasswordValidationToken.ValidateUrl);
            bodyText = bodyText.Replace("[TimeLimit]", $"{expiryMinutes / 60} hours");
            bodyText = bodyText.Replace("[TenantUrl]", this.settings.Value.LearningHubUrl);

            Tenant tenant = await this.tenantRepository.GetByIdAsync(this.settings.Value.LearningHubTenantId);
            if (!string.IsNullOrEmpty(tenant.SupportFormUrl))
            {
                bodyText = bodyText.Replace("[SupportFormUrl]", tenant.SupportFormUrl);
            }

            EmailLog emailLog = new EmailLog()
            {
                EmailTemplateId = emailTemplate.Id,
                FromEmailAddress = websiteEmailsFrom,
                ToUserId = userId,
                ToEmailAddress = user.EmailAddress,
                Subject = emailTemplate.Subject,
                Body = bodyText,
                TenantId = this.settings.Value.LearningHubTenantId,
                Priority = 1,
            };
            await this.emailLogRepository.CreateAsync(userId, emailLog);

            return new LearningHubValidationResult(true);
        }

        /// <inheritdoc/>
        public async Task SendForgotPasswordEmail(string emailAddress)
        {
            var user = this.elfhUserRepository.GetAll().SingleOrDefault(x => x.EmailAddress == emailAddress);
            if (user == null)
            {
                return;
            }

            // Copied from the above SendAdminPasswordResetEmail
            var expiryMinutes = (int)(await this.systemSettingRepository.GetByIdAsync((int)SystemSettingEnum.PasswordValidationExpiryAdminDefault)).IntValue;
            UserPasswordValidationTokenExtended userPasswordValidationToken = this.GenerateUserPasswordValidationToken(expiryMinutes, user.Id);
            await this.userPasswordValidationTokenRepository.CreateAsync(user.Id, userPasswordValidationToken);

            string websiteEmailsFrom = (await this.tenantSmtpRepository.GetByIdAsync(this.settings.Value.LearningHubTenantId)).From;
            EmailTemplate emailTemplate = await this.emailTemplateRepository.GetByTypeAndTenantAsync((int)EmailTemplateTypeEnum.AdminPasswordValidateEmail, this.settings.Value.LearningHubTenantId);
            string bodyText = emailTemplate.Body;
            bodyText = bodyText.Replace("[FullName]", (user.FirstName + " " + user.LastName).ToTitleCase());
            bodyText = bodyText.Replace("[FirstName]", user.FirstName.ToTitleCase());
            bodyText = bodyText.Replace("[UserName]", user.UserName);
            bodyText = bodyText.Replace("[PasswordValidateUrl]", userPasswordValidationToken.ValidateUrl);
            bodyText = bodyText.Replace("[TimeLimit]", (expiryMinutes / 60).ToString() + " hours");
            bodyText = bodyText.Replace("[TenantUrl]", this.settings.Value.LearningHubUrl);

            Tenant tenant = await this.tenantRepository.GetByIdAsync(this.settings.Value.LearningHubTenantId);
            if (!string.IsNullOrEmpty(tenant.SupportFormUrl))
            {
                bodyText = bodyText.Replace("[SupportFormUrl]", tenant.SupportFormUrl);
            }

            EmailLog emailLog = new EmailLog()
            {
                EmailTemplateId = emailTemplate.Id,
                FromEmailAddress = websiteEmailsFrom,
                ToUserId = user.Id,
                ToEmailAddress = user.EmailAddress,
                Subject = emailTemplate.Subject,
                Body = bodyText,
                TenantId = this.settings.Value.LearningHubTenantId,
                Priority = 1,
            };
            await this.emailLogRepository.CreateAsync(user.Id, emailLog);
        }

        /// <inheritdoc/>
        public async Task SendEmailToUserAsync(UserContactViewModel vm)
        {
            var tenantId = this.settings.Value.LearningHubTenantId;
            var template = await this.emailTemplateRepository.GetByTypeAndTenantAsync(22, tenantId);
            var emailBody = template.Body.Replace("[Body]", vm.Body);
            var emailSubject = template.Subject.Replace("[Subject]", vm.Subject);
            string websiteEmailsFrom = (await this.tenantSmtpRepository.GetByIdAsync(this.settings.Value.LearningHubTenantId)).From;
            var emailLog = new EmailLog
            {
                Body = emailBody,
                EmailTemplateId = template.Id,
                FromEmailAddress = websiteEmailsFrom,
                ToUserId = vm.UserId,
                ToEmailAddress = vm.EmailAddress,
                Subject = emailSubject,
                Priority = 1,
                TenantId = tenantId,
            };
            await this.emailLogRepository.CreateAsync(vm.UserId, emailLog);
        }

        /// <inheritdoc/>
        public async Task<bool> HasMultipleUsersForEmailAsync(string emailAddress)
        {
            var count = await this.elfhUserRepository.NumberOfUsersForEmailAsync(emailAddress);
            return count > 1;
        }

        /// <inheritdoc/>
        public async Task LinkEmploymentRecordToUser(int userId)
        {
            await this.elfhUserRepository.LinkEmploymentRecordToUser(userId);
        }

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
        public async Task UpdateEmailDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.EmailAddress = personalDetailsViewModel.PrimaryEmailAddress;
            user.AltEmailAddress = personalDetailsViewModel.SecondaryEmailAddress;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

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
        public async Task UpdatePrimaryEmail(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.EmailAddress = personalDetailsViewModel.PrimaryEmailAddress;
            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

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
        public async Task UpdateSecondaryEmail(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.AltEmailAddress = personalDetailsViewModel.SecondaryEmailAddress;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

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
        public async Task UpdateLocationDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.CountryId = personalDetailsViewModel.CountryId;
            user.RegionId = personalDetailsViewModel.RegionId;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

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
        public async Task UpdateCountryDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.CountryId = personalDetailsViewModel.CountryId;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

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
        public async Task UpdateRegionDetails(PersonalDetailsViewModel personalDetailsViewModel, int currentUserId)
        {
            User user = await this.elfhUserRepository.GetByIdAsync(personalDetailsViewModel.UserId);
            user.RegionId = personalDetailsViewModel.RegionId;

            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
        }

        /// <inheritdoc/>
        public async Task<UserRoleUpgrade> GetUserRoleUpgrade(int userId)
        {
            return await this.userRoleUpgradeRepository.GetByUserIdAsync(userId).Where(n => n.UpgradeDate == null).FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task CreateUserRoleUpgrade(UserRoleUpgrade userRoleUpgrade)
        {
            await this.userRoleUpgradeRepository.CreateAsync(userRoleUpgrade.UserId, userRoleUpgrade);
        }

        /// <inheritdoc/>
        public async Task UpdateUserRoleUpgrade(int currentUserId)
        {
            var userRoleUpgrade = await this.userRoleUpgradeRepository?.GetByUserIdAsync(currentUserId).Where(n => n.UpgradeDate == null).FirstOrDefaultWithNoLockAsync();
            if (userRoleUpgrade != null)
            {
                userRoleUpgrade.Deleted = true;
                await this.userRoleUpgradeRepository.UpdateAsync(currentUserId, userRoleUpgrade);
            }
        }

        /// <inheritdoc/>
        public async Task UpdateUserPrimaryEmailAsync(int currentUserId, string email)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(currentUserId);
            user.EmailAddress = email;
            await this.elfhUserRepository.UpdateAsync(currentUserId, user);
            var userRoleUpgrade = await this.userRoleUpgradeRepository?.GetByUserIdAsync(currentUserId).Where(n => n.UpgradeDate == null).FirstOrDefaultWithNoLockAsync();
            if (userRoleUpgrade != null)
            {
                userRoleUpgrade.UpgradeDate = DateTimeOffset.Now;
                await this.userRoleUpgradeRepository.UpdateAsync(currentUserId, userRoleUpgrade);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ValidateUserRoleUpgrade(string currentPrimaryEmail, string newPrimaryEmail, int currentUserId)
        {
            // Is the user a valid Blue users (based on email address)
            bool isFulltimeUser = this.userGroupRepository.GetByUserAsync(currentUserId).Result.Any(ug => ug.Id == 1070);
            bool isValidWorkEmail = this.userGroupTypeInputValidationRepository.IsEmailValidForUserGroup(newPrimaryEmail, 1070); // 1070 = e-LfH Standard User Type - Blue
            bool isUserRoleUpgrade = false;

            if (!isFulltimeUser && isValidWorkEmail)
            {
                isUserRoleUpgrade = true;
            }

            return isUserRoleUpgrade;
        }

        /// <inheritdoc/>
        public async Task<bool> CheckSamePrimaryemailIsPendingToValidate(string secondaryEmail, int currentUserId)
        {
          var userRoleUpgrades = this.userRoleUpgradeRepository.GetByEmailAddressAsync(secondaryEmail, currentUserId);

          if (userRoleUpgrades.Count() > 0)
          {
                return true;
          }

          return false;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(CreateOpenAthensLinkToLhUser newUserDetails)
        {
            var registrationValidator = new CreateLinkedOpenAthensUserValidator();
            var validationResult = await registrationValidator.ValidateAsync(newUserDetails);

            var retVal = new LearningHubValidationResult(validationResult);

            return retVal;
        }

        private IQueryable<User> FilterItems(IQueryable<User> items, List<PagingColumnFilter> filterCriteria)
        {
            if (filterCriteria == null || filterCriteria.Count == 0)
            {
                return items;
            }

            foreach (var filter in filterCriteria)
            {
                switch (filter.Column.ToLower())
                {
                    case "id":
                        int enteredId = 0;
                        int.TryParse(filter.Value, out enteredId);
                        items = items.Where(x => x.Id == enteredId);
                        break;
                    case "username":
                        items = items.Where(x => x.UserName.Contains(filter.Value));
                        break;
                    case "firstname":
                        items = items.Where(x => x.FirstName.Contains(filter.Value));
                        break;
                    case "lastname":
                        items = items.Where(x => x.LastName.Contains(filter.Value));
                        break;
                    case "emailaddress":
                        items = items.Where(x => x.EmailAddress.Contains(filter.Value) || x.AltEmailAddress.Contains(filter.Value));
                        break;
                    default:
                        break;
                }
            }

            return items;
        }

        private IQueryable<User> OrderItems(IQueryable<User> items, string sortColumn, string sortDirection)
        {
            switch (sortColumn.ToLower())
            {
                case "username":
                    if (sortDirection == "D")
                    {
                        items = items.OrderByDescending(x => x.UserName);
                    }
                    else
                    {
                        items = items.OrderBy(x => x.UserName);
                    }

                    break;
                case "firstname":
                    if (sortDirection == "D")
                    {
                        items = items.OrderByDescending(x => x.FirstName);
                    }
                    else
                    {
                        items = items.OrderBy(x => x.FirstName);
                    }

                    break;
                case "lastname":
                    if (sortDirection == "D")
                    {
                        items = items.OrderByDescending(x => x.LastName);
                    }
                    else
                    {
                        items = items.OrderBy(x => x.LastName);
                    }

                    break;
                case "emailaddress":
                    if (sortDirection == "D")
                    {
                        items = items.OrderByDescending(x => x.EmailAddress);
                    }
                    else
                    {
                        items = items.OrderBy(x => x.EmailAddress);
                    }

                    break;
                default:
                    if (sortDirection == "D")
                    {
                        items = items.OrderByDescending(x => x.Id);
                    }
                    else
                    {
                        items = items.OrderBy(x => x.Id);
                    }

                    break;
            }

            return items;
        }

        private UserPasswordValidationTokenExtended GenerateUserPasswordValidationToken(int expiryMinutes, int userId)
        {
            string salt;
            var userToken = Convert.ToBase64String(this.UniqueId().ToByteArray());
            var lookupToken = Convert.ToBase64String(this.UniqueId().ToByteArray());
            var hashedToken = this.SecureHash(userToken, out salt);
            var passwordValidationExpiryToken = new UserPasswordValidationTokenExtended()
            {
                HashedToken = hashedToken,
                Salt = salt,
                Lookup = lookupToken,
                Expiry = DateTimeOffset.Now.AddMinutes(expiryMinutes),
                TenantId = this.settings.Value.LearningHubTenantId,
                UserId = userId,
            };
            passwordValidationExpiryToken.ValidateUrl = this.settings.Value.LearningHubUrl +
                                                            string.Format(
                                                                "validate-password?token={0}&loctoken={1}",
                                                                HttpUtility.UrlEncode(userToken),
                                                                HttpUtility.UrlEncode(passwordValidationExpiryToken.Lookup));
            passwordValidationExpiryToken.LogOnUrl = this.settings.Value.LearningHubUrl + "account/logon";
            passwordValidationExpiryToken.PasswordResetUrl = this.settings.Value.LearningHubUrl + "forgotten-password";
            return passwordValidationExpiryToken;
        }

        private Guid UniqueId()
        {
            var bytes = this.Random(16);
            bytes[7] = (byte)((bytes[7] & 0x0F) | 0x40);
            bytes[8] = (byte)((bytes[8] & 0x0F) | (0x80 + (this.Random(1)[0] % 4)));

            return new Guid(bytes);
        }

        private byte[] Random(int bytes)
        {
            RandomNumberGenerator randomSource = RandomNumberGenerator.Create();

            var ret = new byte[bytes];
            lock (randomSource)
            {
                randomSource.GetBytes(ret);
            }

            return ret;
        }

        private string SecureHash(string value, out string salt)
        {
            salt = this.GenerateSalt();
            return this.Hash(value, salt);
        }

        private string GenerateSalt()
        {
            var bytes = this.Random(16);
            var iterations = 1000;
            return iterations + "." + Convert.ToBase64String(bytes);
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