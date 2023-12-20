// <copyright file="RegistrationService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Enums;
    using elfhHub.Nhs.Models.Validation;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Helpers;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Services.Models;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The registration service.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        private const string LocalHostIpAddress = "127.0.0.1";
        private readonly IElfhUserService userService;
        private readonly IMedicalCouncilService medicalCouncilService;
        private readonly ILoginWizardService loginWizardService;
        private readonly IUserHistoryService userHistoryService;
        private readonly IElfhUserRepository userRepository;
        private readonly Repository.Interface.LH.IExternalSystemUserRepository externalSystemUserRepository;
        private readonly IIpCountryLookupRepository ipCountryLookupRepository;
        private readonly IUserExternalSystemRepository userExternalSystemRepository;
        private readonly IExternalSystemRepository externalSystemRepository;
        private readonly IJobRoleRepository jobRoleRepository;
        private readonly IMedicalCouncilRepository medicalCouncilRepository;
        private readonly IUserEmploymentRepository userEmploymentRepository;
        private readonly IUserGroupRepository userGroupRepository;
        private readonly IUserUserGroupRepository userUserGroupRepository;
        private readonly ISystemSettingRepository systemSettingRepository;
        private readonly IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository;
        private readonly ITenantRepository tenantRepository;
        private readonly ITenantSmtpRepository tenantSmtpRepository;
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IEmailLogRepository emailLogRepository;
        private readonly IUserGroupTypeInputValidationRepository userGroupTypeInputValidationRepository;
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="medicalCouncilService">The medical council service.</param>
        /// <param name="loginWizardService">The login wizard service.</param>
        /// <param name="userHistoryService">The user history service.</param>
        /// <param name="elfhUserRepository">The elfh user repository.</param>
        /// <param name="externalSystemUserRepository">The external system user repository.</param>
        /// <param name="ipCountryLookupRepository">the ip country lookup repository.</param>
        /// <param name="userExternalSystemRepository">The userExternalSystemRepository.</param>
        /// <param name="externalSystemRepository">The externalSystemRepository.</param>
        /// <param name="jobRoleRepository">The job role repository.</param>
        /// <param name="medicalCouncilRepository">The medical council repository.</param>
        /// <param name="userEmploymentRepository">The user employment repository.</param>
        /// <param name="userGroupRepository">The user group repository.</param>
        /// <param name="userUserGroupRepository">The user user group repository.</param>
        /// <param name="systemSettingRepository">The system settings repository.</param>
        /// <param name="userPasswordValidationTokenRepository">The user password validation token repository.</param>
        /// <param name="tenantRepository">The tenant repository.</param>
        /// <param name="tenantSmtpRepository">The tenant smtp repository.</param>
        /// <param name="emailTemplateRepository">The email template repository.</param>
        /// <param name="emailLogRepository">The email log repository.</param>
        /// <param name="userGroupTypeInputValidationRepository">The userGroupType Input Validation repository.</param>
        /// <param name="settings">The settings.</param>
        public RegistrationService(
             IElfhUserRepository elfhUserRepository,
             IUserGroupTypeInputValidationRepository userGroupTypeInputValidationRepository,
             IJobRoleRepository jobRoleRepository,
             IUserUserGroupRepository userUserGroupRepository,
             IUserEmploymentRepository userEmploymentRepository,
             IEmailTemplateRepository emailTemplateRepository,
             IEmailLogRepository emailLogRepository,
             ISystemSettingRepository systemSettingRepository,
             IUserPasswordValidationTokenRepository userPasswordValidationTokenRepository,
             ITenantRepository tenantRepository,
             ITenantSmtpRepository tenantSmtpRepository,
             IMedicalCouncilService medicalCouncilService,
             IElfhUserService userService,
             IUserHistoryService userHistoryService,
             IOptions<Settings> settings,
             ILoginWizardService loginWizardService,
             IMedicalCouncilRepository medicalCouncilRepository,
             IUserGroupRepository userGroupRepository,
             IExternalSystemRepository externalSystemRepository,
             IUserExternalSystemRepository userExternalSystemRepository,
             Repository.Interface.LH.IExternalSystemUserRepository externalSystemUserRepository,
             IIpCountryLookupRepository ipCountryLookupRepository)
        {
            this.userService = userService;
            this.medicalCouncilService = medicalCouncilService;
            this.loginWizardService = loginWizardService;
            this.userHistoryService = userHistoryService;
            this.userRepository = elfhUserRepository;
            this.externalSystemUserRepository = externalSystemUserRepository;
            this.userExternalSystemRepository = userExternalSystemRepository;
            this.externalSystemRepository = externalSystemRepository;
            this.jobRoleRepository = jobRoleRepository;
            this.medicalCouncilRepository = medicalCouncilRepository;
            this.userEmploymentRepository = userEmploymentRepository;
            this.userGroupRepository = userGroupRepository;
            this.userUserGroupRepository = userUserGroupRepository;
            this.systemSettingRepository = systemSettingRepository;
            this.userPasswordValidationTokenRepository = userPasswordValidationTokenRepository;
            this.tenantRepository = tenantRepository;
            this.tenantSmtpRepository = tenantSmtpRepository;
            this.emailTemplateRepository = emailTemplateRepository;
            this.emailLogRepository = emailLogRepository;
            this.userGroupTypeInputValidationRepository = userGroupTypeInputValidationRepository;
            this.ipCountryLookupRepository = ipCountryLookupRepository;
            this.settings = settings.Value;
        }

        /// <inheritdoc/>
        public async Task<EmailRegistrationStatus> GetRegistrationStatus(string emailAddress, string ipAddress)
        {
            // Does email address belong to an existing user?
            bool emailExists = await this.userRepository.DoesEmailExistAsync(emailAddress);
            bool isBlueUserFromRole = false;
            if (emailExists)
            {
                var users = this.userRepository.GetUsersForEmail(emailAddress);
                var userIds = users.Select(x => x.Id).ToListWithNoLock();

                // The account could have a non-blueuser email address but still have a blue user role
                isBlueUserFromRole = userIds.Any(id => this.userGroupRepository.GetByUserAsync(id).Result.Any(ug => ug.Id == 1070));
            }

            // Is the user a valid Blue users (based on email address)
            bool isBlueUserFromEmail = this.userGroupTypeInputValidationRepository.IsEmailValidForUserGroup(emailAddress, 1070); // 1070 = e-LfH Standard User Type - Blue
            EmailRegistrationStatus status;
            var isBlueUser = isBlueUserFromEmail || isBlueUserFromRole;

            if (!isBlueUser && ipAddress != null && ipAddress != LocalHostIpAddress)
            {
                var ipNumber = this.ConvertFromIpAddressToInteger(ipAddress);

                var isUKIpAddress = await this.ipCountryLookupRepository.IsUKIpAddress(ipNumber);

                if (!isUKIpAddress)
                {
                    return EmailRegistrationStatus.NonUKLocation;
                }
            }

            if (emailExists)
            {
                status = EmailRegistrationStatus.ExistingUserIsEligible;
            }
            else if (!emailExists && isBlueUser)
            {
                status = EmailRegistrationStatus.NewUserIsEligible;
            }
            else if (!emailExists && !isBlueUser)
            {
                status = EmailRegistrationStatus.NewGeneralUserIsEligible;
            }
            else
            {
                status = EmailRegistrationStatus.NewUserNotEligible;
            }

            return status;
        }

        /// <inheritdoc/>
        public async Task LinkExistingUserToSso(int userId, int externalSystemId)
        {
            var userExternalSystem = new ExternalSystemUser
            {
                UserId = userId,
                ExternalSystemId = externalSystemId,
            };

            await this.externalSystemUserRepository.CreateAsync(userId, userExternalSystem);
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> RegisterUser(RegistrationRequestViewModel registrationRequest)
        {
            var retVal = await this.ValidateAsync(registrationRequest);
            if (retVal.IsValid)
            {
                string errorMessage;
                JobRole jobRole = await this.jobRoleRepository.GetByIdAsync(registrationRequest.JobRoleId);
                int medicalCouncilId = jobRole.MedicalCouncilId ?? 0;

                // Validate that medicalCouncilId is correct for JobRole and MedicalCouncil Number is valid
                switch (medicalCouncilId)
                {
                    case 0:
                    case 8:
                        errorMessage = string.Empty; // no Medical Council associated with this JobRole
                        break;
                    case 1: // GMC
                        errorMessage = await this.medicalCouncilService.ValidateGMCNumber(registrationRequest.LastName, registrationRequest.MedicalCouncilNumber, null, string.Empty);
                        break;
                    case 2: // NMC
                        errorMessage = this.medicalCouncilService.ValidateNMCNumber(registrationRequest.MedicalCouncilNumber);
                        break;
                    case 3: // GDC
                        errorMessage = await this.medicalCouncilService.ValidateGDCNumber(registrationRequest.LastName, registrationRequest.MedicalCouncilNumber);
                        break;
                    default:
                        errorMessage = "Unrecognised Medical Council";
                        break;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    retVal.IsValid = false;
                    retVal.Details.Add(errorMessage);
                    return retVal;
                }

                // Check that the user doesn't already exist
                if (await this.userService.UserAlreadyExistsAsync(medicalCouncilId, registrationRequest.MedicalCouncilNumber, registrationRequest.EmailAddress))
                {
                    retVal.IsValid = false;
                    MedicalCouncil medicalCouncil = await this.medicalCouncilRepository.GetByIdAsync(medicalCouncilId);
                    if (medicalCouncil != null)
                    {
                        retVal.Details.Add($"This {medicalCouncil.Code} number you entered is already registered to an existing account.");
                    }
                    else
                    {
                        retVal.Details.Add($"This email address {registrationRequest.EmailAddress} you entered is already registered to an existing account.");
                    }

                    return retVal;
                }

                // Create new user
                User newUser = new User()
                {
                    UserName = await this.userService.CreateUserNameAsync(medicalCouncilId, registrationRequest.MedicalCouncilNumber, registrationRequest.LastName),
                    EmailAddress = registrationRequest.EmailAddress,
                    FirstName = registrationRequest.FirstName,
                    LastName = registrationRequest.LastName,
                    AltEmailAddress = registrationRequest.SecondaryEmailAddress,
                    PreferredName = registrationRequest.PreferredName,
                    CountryId = registrationRequest.CountryId,
                    RegionId = registrationRequest.RegionId,
                    Active = true,
                    PreferredTenantId = this.settings.LearningHubTenantId,
                    LastLoginWizardCompleted = DateTimeOffset.Now,
                    CreatedDate = DateTimeOffset.Now,
                    RestrictToSso = registrationRequest.IsExternalUser,
                };
                int userId = await this.userRepository.CreateAsync(0, newUser);
                retVal.CreatedId = userId;

                // Create JobRole, PlaceOfWork and Personal Details LoginWizard Stage Activity, so it doesn't ask again for self registration
                if (registrationRequest.SelfRegistration && registrationRequest.IsExternalUser == false)
                {
                    var lwsJobRoleActivityResult = await this.loginWizardService.CreateStageActivity((int)LoginWizardStageEnum.JobRole, userId);

                    if (!lwsJobRoleActivityResult.IsValid)
                    {
                        return lwsJobRoleActivityResult;
                    }

                    var lwsPlaceOfWorkActivityResult = await this.loginWizardService.CreateStageActivity((int)LoginWizardStageEnum.PlaceOfWork, userId);

                    if (!lwsJobRoleActivityResult.IsValid)
                    {
                        return lwsPlaceOfWorkActivityResult;
                    }

                    var lwsPersonalDetailsActivityResult = await this.loginWizardService.CreateStageActivity((int)LoginWizardStageEnum.PersonalDetails, userId);

                    if (!lwsJobRoleActivityResult.IsValid)
                    {
                        return lwsPersonalDetailsActivityResult;
                    }
                }

                // Add UserEmployment
                int userEmploymentId = await this.userEmploymentRepository.CreateAsync(userId, new UserEmployment()
                {
                    UserId = userId,
                    JobRoleId = registrationRequest.JobRoleId > 0 ? registrationRequest.JobRoleId : null,
                    SpecialtyId = registrationRequest.SpecialtyId > 0 ? registrationRequest.SpecialtyId : null,
                    GradeId = registrationRequest.GradeId > 0 ? registrationRequest.GradeId : null,
                    LocationId = registrationRequest.LocationId > 0 ? registrationRequest.LocationId : 1,
                    MedicalCouncilId = jobRole.MedicalCouncilId > 0 ? jobRole.MedicalCouncilId : null,
                    MedicalCouncilNo = registrationRequest.MedicalCouncilNumber,
                    StartDate = registrationRequest.LocationStartDate != DateTimeOffset.MinValue ? registrationRequest.LocationStartDate : null,
                });

                newUser.PrimaryUserEmploymentId = userEmploymentId;
                await this.userRepository.UpdateAsync(userId, newUser);

                // Add UserHistory entry
                UserHistoryViewModel userHistory = new UserHistoryViewModel()
                {
                    UserId = userId,
                    Detail = "User created by self-registration",
                    UserHistoryTypeId = (int)UserHistoryType.SystemEvent,
                };
                var uhCreateResult = await this.userHistoryService.CreateAsync(userHistory, userId);

                if (!uhCreateResult.IsValid)
                {
                    return uhCreateResult;
                }

                bool isBlueUserFromEmail = this.userGroupTypeInputValidationRepository.IsEmailValidForUserGroup(newUser.EmailAddress, 1070); // 1070 = e-LfH Standard User Type - Blue

                // Add user to UserGroup
                UserUserGroup userUserGroup = new UserUserGroup()
                {
                    UserId = userId,
                    UserGroupId = isBlueUserFromEmail ? 1070 : 1170, // "1070 e-LfH Standard User Type - Blue" "1170 Public User Type"
                };
                await this.userUserGroupRepository.CreateAsync(userId, userUserGroup);

                if (registrationRequest.IsExternalUser)
                {
                    var userExternalSystem = new ExternalSystemUser
                    {
                        UserId = userId,
                        ExternalSystemId = registrationRequest.ExternalSystemId.Value,
                    };

                    await this.externalSystemUserRepository.CreateAsync(userId, userExternalSystem);
                }

                if (registrationRequest.IsExternalUser == false)
                {
                    // Send registration confirmation email
                    /* This code was duplicated in the elfhHub.Nhs.Services.UserService.SendAdminPasswordResetEmail method, but tweaked for that email template.
                     * The GenerateUserPasswordValidationToken method (and its child methods) were all directly copied into the UserService. This is
                     * definitely a candidate for turning into a shared component after RR. */
                    var expiryMinutes = (int)(await this.systemSettingRepository.GetByIdAsync((int)SystemSettingEnum.PasswordValidationExpiryUserDefault)).IntValue;
                    UserPasswordValidationTokenExtended userPasswordValidationToken = this.GenerateUserPasswordValidationToken(expiryMinutes, userId);
                    await this.userPasswordValidationTokenRepository.CreateAsync(userId, userPasswordValidationToken);

                    string websiteEmailsFrom = (await this.tenantSmtpRepository.GetByIdAsync(this.settings.LearningHubTenantId)).From;
                    EmailTemplate emailTemplate = await this.emailTemplateRepository.GetByTypeAndTenantAsync((int)EmailTemplateTypeEnum.SuccessEmail_New, this.settings.LearningHubTenantId);
                    string bodyText = emailTemplate.Body;
                    bodyText = bodyText.Replace("[FullName]", (newUser.FirstName + " " + newUser.LastName).ToTitleCase());
                    bodyText = bodyText.Replace("[FirstName]", newUser.FirstName.ToTitleCase());
                    bodyText = bodyText.Replace("[PasswordValidateUrl]", userPasswordValidationToken.ValidateUrl);
                    string resetUrl = this.settings.LearningHubUrl + userPasswordValidationToken.HashedToken;
                    //// emailTemplate.Body = emailTemplate.Body.Replace("[LHValidateURL]", resetUrl);

                    bodyText = bodyText.Replace("[TimeLimit]", GenericHelper.GetPasswordTimeoutString(expiryMinutes));
                    bodyText = bodyText.Replace("[ResetPasswordUrl]", userPasswordValidationToken.PasswordResetUrl);
                    bodyText = bodyText.Replace("[LogInUrl]", userPasswordValidationToken.LogOnUrl);
                    bodyText = bodyText.Replace("[UserName]", newUser.UserName);
                    bodyText = bodyText.Replace("[TenantUrl]", this.settings.LearningHubUrl);

                    Tenant tenant = await this.tenantRepository.GetByIdAsync(this.settings.LearningHubTenantId);
                    bodyText = bodyText.Replace("[TenantUrl]", this.settings.LearningHubUrl);
                    if (!string.IsNullOrEmpty(tenant.QuickStartGuideUrl))
                    {
                        bodyText = bodyText.Replace("[QuickStartGuideUrl]", tenant.QuickStartGuideUrl);
                    }

                    if (!string.IsNullOrEmpty(tenant.SupportFormUrl))
                    {
                        bodyText = bodyText.Replace("[SupportFormUrl]", tenant.SupportFormUrl);
                    }

                    EmailLog emailLog = new EmailLog()
                    {
                        EmailTemplateId = emailTemplate.Id,
                        FromEmailAddress = websiteEmailsFrom,
                        ToUserId = userId,
                        ToEmailAddress = registrationRequest.EmailAddress,
                        Subject = emailTemplate.Subject,
                        Body = bodyText,
                        TenantId = this.settings.LearningHubTenantId,
                        Priority = 1,
                    };
                    await this.emailLogRepository.CreateAsync(userId, emailLog);
                }
            }

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<bool> GetEmailStatus(string emailAddress)
        {
            // Is the user a valid Blue users (based on email address)
            bool isBlueUserFromEmail = this.userGroupTypeInputValidationRepository.IsEmailValidForUserGroup(emailAddress, 1070); // 1070 = e-LfH Standard User Type - Blue

            if (isBlueUserFromEmail)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task SyncSsoUsertoElfh(int userId, string externalSystemCode)
        {
            var elfhExternalSystem = this.externalSystemRepository.GetByCode(externalSystemCode);

            var userExternalSystem = this.userExternalSystemRepository.GetUserExternalSystem(userId, elfhExternalSystem.Id);
            if (userExternalSystem == null)
            {
                userExternalSystem = new UserExternalSystem
                {
                    UserId = userId,
                    ExternalSystemId = elfhExternalSystem.Id,
                    Active = true,
                };
                await this.userExternalSystemRepository.CreateAsync(userId, userExternalSystem);
                return;
            }

            if (!userExternalSystem.Active)
            {
                userExternalSystem.Active = true;

                await this.userExternalSystemRepository.UpdateAsync(userId, userExternalSystem);
            }
        }

        private long ConvertFromIpAddressToInteger(string ipAddress)
        {
            byte[] bytes = IPAddress.Parse(ipAddress).GetAddressBytes();

            // flip big-endian(network order) to little-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        private async Task<LearningHubValidationResult> ValidateAsync(RegistrationRequestViewModel registrationRequest)
        {
            var registrationValidator = new RegistrationValidator();
            var validationResult = await registrationValidator.ValidateAsync(registrationRequest);

            var retVal = new LearningHubValidationResult(validationResult);

            return retVal;
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
                TenantId = this.settings.LearningHubTenantId,
                UserId = userId,
            };
            passwordValidationExpiryToken.ValidateUrl = this.settings.LearningHubUrl +
                                                            string.Format(
                                                                "validate-password?token={0}&loctoken={1}",
                                                                HttpUtility.UrlEncode(userToken),
                                                                HttpUtility.UrlEncode(passwordValidationExpiryToken.Lookup));
            passwordValidationExpiryToken.LogOnUrl = this.settings.LearningHubUrl + "account/logon";
            passwordValidationExpiryToken.PasswordResetUrl = this.settings.LearningHubUrl + "forgotten-password";
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
