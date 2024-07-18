namespace LearningHub.Nhs.UserApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.OpenAthens;
    using LearningHub.Nhs.UserApi.Helpers;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;
    using Login = elfhHub.Nhs.Models.Common.Login;

    /// <summary>
    /// Api for interacting with ELFH Users.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class ElfhUserController : ApiControllerBase
    {
        private readonly IElfhUserService elfhUserService;
        private readonly IAuthenticationService authenticationService;
        private readonly IExternalSystemUserService extSysUserSvc;
        private readonly IRegistrationService registrationService;
        private readonly ILogger<ElfhUserController> logger;
        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhUserController"/> class.
        /// </summary>
        /// <param name="elfhUserService">The elfh user service.</param>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="extSysUserSvc">The external system service.</param>
        /// <param name="registrationService">The registration service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="securityService">securityService.</param>
        public ElfhUserController(
            IElfhUserService elfhUserService,
            IAuthenticationService authenticationService,
            IExternalSystemUserService extSysUserSvc,
            IRegistrationService registrationService,
            ILogger<ElfhUserController> logger,
            ISecurityService securityService)
        : base(elfhUserService)
        {
            this.elfhUserService = elfhUserService;
            this.authenticationService = authenticationService;
            this.extSysUserSvc = extSysUserSvc;
            this.registrationService = registrationService;
            this.logger = logger;
            this.securityService = securityService;
        }

        /// <summary>
        /// The get basic by user id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetBasicByUserId/{id}")]
        public async Task<IActionResult> GetBasicByUserId(int id)
        {
            var user = await this.elfhUserService.GetBasicProfileByIdAsync(id);

            return this.Ok(user);
        }

        /// <summary>
        /// Get e-LfH Hub user by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetByUserId/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var user = await this.elfhUserService.GetByIdAsync(id);

            return this.Ok(user);
        }

        /// <summary>
        /// Get e-LfH Hub user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetByUsername/{*username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await this.elfhUserService.GetByUsernameAsync(username);

            return this.Ok(user);
        }

        /// <summary>
        /// Get e-LfH Hub user bu username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetUserIdByUsername/{*username}")]
        public async Task<IActionResult> GetUserIdByUsername(string username)
        {
            var userId = await this.elfhUserService.GetUserIdByUsernameAsync(username);

            return this.Ok(userId);
        }

        /// <summary>
        /// Get e-LfH Hub user by OpenAthens id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetByOpenAthensId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByOpenAthensId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest();
            }

            var user = await this.elfhUserService.GetByOpenAthensIdAsync(id);

            return this.Ok(user);
        }

        /// <summary>
        /// Get e-LfH Hub user role name by user id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetUserRoleName/{id}")]
        public async Task<IActionResult> GetUserRoleName(int id)
        {
            var roleName = await this.elfhUserService.GetUserRoleAsync(id);

            return this.Ok(roleName);
        }

        /// <summary>
        /// Link existing user to sso.
        /// </summary>
        /// <param name="request">Link user to sso request.</param>
        /// <returns>The <see cref="LoginResultInternal"/>.</returns>
        [HttpPost]
        [Route("LinkUserToSso")]
        public async Task<IActionResult> LinkExistingUserToSso([FromBody] LinkUserToSsoRequestViewModel request)
        {
            var authResult = await this.authenticationService.CheckUserCredentialsAsync(new Login { Username = request.Username, Password = request.Password });

            if (!authResult.IsAuthenticated)
            {
                return this.Ok(authResult);
            }

            var extSystemUser = await this.extSysUserSvc.GetByIdAsync(authResult.UserId, request.ExternalSystemId);

            await this.registrationService.SyncSsoUsertoElfh(authResult.UserId, request.ExternalSystemCode);

            if (extSystemUser != null)
            {
                return this.Ok(authResult);
            }

            await this.registrationService.LinkExistingUserToSso(authResult.UserId, request.ExternalSystemId);

            return this.Ok(authResult);
        }

        // POST api/ElfhUser/RegisterUser

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registrationRequest">The registration request.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationRequestViewModel registrationRequest)
        {
            return this.Ok(await this.registrationService.RegisterUser(registrationRequest));
        }

        // GET api/ElfhUser/GetCurrentUserBasicDetails/

        /// <summary>
        /// Get current user's basic details.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpGet]
        [Route("GetCurrentUserBasicDetails")]
        public async Task<IActionResult> GetCurrentUserBasicDetails()
        {
            var user = await this.elfhUserService.GetBasicProfileByIdAsync(this.CurrentUserId);

            return this.Ok(user);
        }

        // GET api/user/GetCurrentUser

        /// <summary>
        /// Get current e-LfH Hub user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await this.GetCurrentUserAsync();
            return this.Ok(user);
        }

        // GET api/ElfhUser/GetRegistrationStatus/emailAddress

        /// <summary>
        /// Get registration status of an email address.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <param name="ipAddress">
        /// The ip address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetRegistrationStatus/{emailAddress}/{ipAddress}")]
        public async Task<IActionResult> GetRegistrationStatus(string emailAddress, string ipAddress)
        {
            var status = await this.registrationService.GetRegistrationStatus(emailAddress, ipAddress);

            return this.Ok(status);
        }

        /// <summary>
        /// Get the status of user role upgrade.
        /// </summary>
        /// <param name="currentPrimaryEmail">
        /// The current primary email address.
        /// </param>
        /// <param name="newPrimaryEmail">New email address.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("ValidateUserRoleUpgrade/{currentPrimaryEmail}")]
        public async Task<IActionResult> ValidateUserRoleUpgrade(string currentPrimaryEmail, string newPrimaryEmail)
        {
            var status = await this.elfhUserService.ValidateUserRoleUpgrade(currentPrimaryEmail, newPrimaryEmail, this.CurrentUserId);

            return this.Ok(status);
        }

        /// <summary>
        /// To check same primary email is pending to validate.
        /// </summary>
        /// <param name="secondaryEmail">
        /// The current secondary email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("CheckSamePrimaryemailIsPendingToValidate/{secondaryEmail}")]
        public async Task<IActionResult> CheckSamePrimaryemailIsPendingToValidate(string secondaryEmail)
        {
            var status = await this.elfhUserService.CheckSamePrimaryemailIsPendingToValidate(secondaryEmail, this.CurrentUserId);

            return this.Ok(status);
        }

        /// <summary>
        /// Get Email Status.
        /// </summary>
        /// <param name="emailAddress">
        /// The current primary email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetEmailStatus/{emailAddress}")]
        public async Task<IActionResult> GetEmailStatus(string emailAddress)
        {
            var status = await this.registrationService.GetEmailStatus(emailAddress);

            return this.Ok(status);
        }

        /// <summary>
        /// Link existing user to sso.
        /// </summary>
        /// <param name="request">Link user to sso request.</param>
        /// <returns>The <see cref="LoginResultInternal"/>.</returns>
        [HttpPost]
        [Route("SyncSsoUsertoElfh")]
        public async Task<IActionResult> SyncSsoUsertoElfh([FromBody] LinkUserToSsoRequestViewModel request)
        {
            var authResult = await this.authenticationService.CheckUserCredentialsAsync(new Login { Username = request.Username, Password = request.Password });
            await this.registrationService.SyncSsoUsertoElfh(authResult.UserId, request.ExternalSystemCode);

            return this.Ok();
        }

        // PUT api/ElfhUser/UpdateCurrentUserPassword

        /// <summary>
        /// Update the password of the current user.
        /// </summary>
        /// <param name="passwordUpdateModel">
        /// The password update model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateCurrentUserPassword")]
        public async Task<IActionResult> UpdateCurrentUserPassword([FromBody] PasswordUpdateModel passwordUpdateModel)
        {
            await this.elfhUserService.UpdateCurrentUserPassword(passwordUpdateModel.PasswordHash, this.CurrentUserId);
            return this.Ok();
        }

        // PUT api/ElfhUser/UpdateLoginWizardFlag

        /// <summary>
        /// Switch the login wizard flag.
        /// </summary>
        /// <param name="updateLoginWizardInProgress">
        /// The update login wizard in progress.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateLoginWizardFlag")]
        public async Task<IActionResult> UpdateLoginWizardFlag([FromBody] bool updateLoginWizardInProgress)
        {
            await this.elfhUserService.UpdateLoginWizardFlag(updateLoginWizardInProgress, this.CurrentUserId);
            return this.Ok();
        }

        // PUT api/ElfhUser/UpdateUserSecurityQuestions

        /// <summary>
        /// The update user security questions.
        /// </summary>
        /// <param name="userSecurityQuestions">
        /// The user security questions.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateUserSecurityQuestions")]
        public async Task<IActionResult> UpdateUserSecurityQuestions([FromBody] List<UserSecurityQuestionViewModel> userSecurityQuestions)
        {
            await this.elfhUserService.UpdateUserSecurityQuestions(userSecurityQuestions, this.CurrentUserId);
            return this.Ok();
        }

        // PUT api/ElfhUser/UpdatePersonalDetails

        /// <summary>
        /// Update the user's personal details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPut]
        [Route("UpdatePersonalDetails")]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdatePersonalDetails(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's first name details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The update user's first name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateFirstName")]
        public async Task<IActionResult> UpdateFirstName([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateFirstName(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's last name details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The update user's last name.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateLastName")]
        public async Task<IActionResult> UpdateLastName([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateLastName(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's preferred name details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdatePreferredName")]
        public async Task<IActionResult> UpdatePreferredName([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdatePreferredName(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's primary email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdatePrimaryEmail")]
        public async Task<IActionResult> UpdatePrimaryEmail([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdatePrimaryEmail(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's secondary email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateSecondaryEmail")]
        public async Task<IActionResult> UpdateSecondaryEmail([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateSecondaryEmail(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// GetPersonalDetails.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Authorize]
        [HttpGet]
        [Route("GetPersonalDetails")]
        public async Task<IActionResult> GetPersonalDetails()
        {
            var result = await this.elfhUserService.GetPersonalDetailsAsync(this.CurrentUserId);
            return this.Ok(result);
        }

        // GET /api/ElfhUser/DoesEmailExistForUser

        /// <summary>
        /// Check an email is already registered against a user.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("DoesEmailExistForUser/{email}")]
        public async Task<IActionResult> DoesEmailExistForUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return this.NotFound();
            }

            var result = await this.elfhUserService.DoesEmailAddressExistAsync(email);
            return this.Ok(result);
        }

        // POST /api/ElfhUser/CreateElfhAccountWithLinkedOpenAthens

        /// <summary>
        /// Create an ELFH user account linked to OpenAthens user attributes.
        /// </summary>
        /// <param name="newUserDetails">
        /// The new user details.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("CreateElfhAccountWithLinkedOpenAthens")]
        public async Task<IActionResult> CreateElfhAccountWithLinkedOpenAthens([FromBody] CreateOpenAthensLinkToLhUser newUserDetails)
        {
            if (string.IsNullOrWhiteSpace(newUserDetails.LastName) || string.IsNullOrWhiteSpace(newUserDetails.EmailAddress) ||
                string.IsNullOrWhiteSpace(newUserDetails.OaUserId) || string.IsNullOrWhiteSpace(newUserDetails.OaOrganisationId))
            {
                throw new Exception("Cannot create user, some parameters are missing or empty.");
            }

            var vr = await this.elfhUserService.CreateOpenAthensUserWithBasicInfoAsync(newUserDetails);

            if (vr.IsValid)
            {
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                this.logger.LogError("ElfhUserController - CreateElfhAccountWithLinkedOpenAthens: " + string.Join(",", vr.Details.ToArray()));
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }

        // POST /api/ElfhUser/CreateOpenAthensLinkToUser

        /// <summary>
        /// Link an existing user account linked to OpenAthens user attributes.
        /// </summary>
        /// <param name="linkDetails">
        /// The link details.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("CreateOpenAthensLinkToUser")]
        public async Task<IActionResult> CreateOpenAthensLinkToUser([FromBody] OpenAthensToElfhUserLinkDetails linkDetails) // int lhUserId, string openAthensUserId, string openAthensOrgId)
        {
            if (linkDetails.UserId == 0 || string.IsNullOrWhiteSpace(linkDetails.OaUserId) || string.IsNullOrWhiteSpace(linkDetails.OaOrgId))
            {
                throw new Exception("Cannot link user, some parameters are missing or empty.");
            }

            var result = await this.elfhUserService.CreateOpenAthensLinkToUserAsync(linkDetails);

            if (result)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        //// Moved From User Controller

        /// <summary>
        /// Get a filtered page of User records.
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
        [HttpGet]
        [Route("GetUserAdminBasicFilteredPage/{page}/{pageSize}/{sortColumn}/{sortDirection}/{*filter}")]
        public async Task<IActionResult> GetUserAdminBasicFilteredPage(int page, int pageSize, string sortColumn, string sortDirection, string filter)
        {
            filter = HttpUtility.UrlDecode(filter);
            PagedResultSet<UserAdminBasicViewModel> pagedResultSet = await this.ElfhUserService.GetUserAdminBasicPageAsync(page, pageSize, sortColumn, sortDirection, filter);
            return this.Ok(pagedResultSet);
        }

        /// <summary>
        /// Returns the UserAdminDetail model for a particular user id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetUserAdminDetailById/{id}")]
        public async Task<IActionResult> GetUserAdminDetailById(int id)
        {
            var user = await this.ElfhUserService.GetUserAdminDetailByIdAsync(id);

            return this.Ok(user);
        }

        /// <summary>
        /// Returns the tentants for a given user id.
        /// </summary>
        /// <param name="userId">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetTenantDescriptionByUserId/{userId}")]
        public async Task<IActionResult> GetTenantDescriptionByUserId(int userId)
        {
            var tenant = await this.ElfhUserService.GetTenantDescriptionByUserId(userId);

            return this.Ok(tenant);
        }

        /// <summary>
        /// Update an existing User.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPut("{user}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserAdminDetailViewModel user)
        {
            var (result, permissionNotification) = await this.ElfhUserService.UpdateUserAsync(user, this.CurrentUserId);
            if (result.IsValid)
            {
                return this.Ok(new ApiResponse(true, result));
            }
            else
            {
                return this.BadRequest(new ApiResponse(false, result));
            }
        }

        /// <summary>
        /// Send Admin Password Reset Email to the user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpPost]
        [Route("SendAdminPasswordResetEmail")]
        public async Task<IActionResult> SendAdminPasswordResetEmail([FromBody] int userId)
        {
            var vr = await this.ElfhUserService.SendAdminPasswordResetEmail(userId);

            if (vr.IsValid)
            {
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }

        /// <summary>
        /// Sends an email to the user.
        /// </summary>
        /// <param name="vm">
        /// The view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("SendEmailToUser")]
        public async Task<IActionResult> SendEmailToUser(UserContactViewModel vm)
        {
            await this.ElfhUserService.SendEmailToUserAsync(vm);

            // TODO: update the contacthistory table
            return this.Ok();
        }

        /// <summary>
        /// The has multiple users for email.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <param name="ipAddress">
        /// The ip address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("HasMultipleUsersForEmail/{emailAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> HasMultipleUsersForEmail(string emailAddress, string ipAddress)
        {
            var result = await this.ElfhUserService.HasMultipleUsersForEmailAsync(emailAddress);
            return this.Ok(result);
        }

        /// <summary>
        /// The forgot password.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            await this.ElfhUserService.SendForgotPasswordEmail(model.EmailAddress);
            return this.Ok(new ApiResponse(true));
        }

        /// <summary>
        /// Link Employment Record to User.
        /// </summary>
        /// <returns>
        /// Nothing.
        /// </returns>
        [Authorize]
        [HttpGet]
        [Route("LinkEmploymentRecordToUser")]
        public async Task LinkEmploymentRecordToUser()
        {
            await this.ElfhUserService.LinkEmploymentRecordToUser(this.CurrentUserId);
        }

        /// <summary>
        /// Update the user's email details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateEmailDetails")]
        public async Task<IActionResult> UpdateEmailDetails([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateEmailDetails(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's location details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateLocationDetails")]
        public async Task<IActionResult> UpdateLocationDetails([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateLocationDetails(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's Country details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateCountryDetails")]
        public async Task<IActionResult> UpdateCountryDetails([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateCountryDetails(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's region details.
        /// </summary>
        /// <param name="personalDetailsViewModel">
        /// The personal details view model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateRegionDetails")]
        public async Task<IActionResult> UpdateRegionDetails([FromBody] PersonalDetailsViewModel personalDetailsViewModel)
        {
            await this.elfhUserService.UpdateRegionDetails(personalDetailsViewModel, this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Get User role upgarde details.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetUserRoleUpgrade/{userId}")]
        public async Task<IActionResult> GetUserRoleUpgrade(int userId)
        {
            var userRoleUpgardeModel = await this.elfhUserService.GetUserRoleUpgrade(userId);
            return this.Ok(userRoleUpgardeModel);
        }

        /// <summary>
        /// Create the user's users role upgrade details.
        /// </summary>
        /// <param name="userRoleUpgrade">
        /// The userRoleUpgrade.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("CreateUserRoleUpgrade")]
        public async Task<IActionResult> CreateUserRoleUpgrade([FromBody] UserRoleUpgrade userRoleUpgrade)
        {
            await this.elfhUserService.CreateUserRoleUpgrade(userRoleUpgrade);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's region details.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateUserRoleUpgrade")]
        public async Task<IActionResult> UpdateUserRoleUpgrade()
        {
            await this.elfhUserService.UpdateUserRoleUpgrade(this.CurrentUserId);
            return this.Ok();
        }

        /// <summary>
        /// Update the user's role upgrade date and email details.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        [Route("UpdateUserPrimaryEmail/{email}")]
        public async Task<IActionResult> UpdateUserPrimaryEmail(string email)
        {
            await this.elfhUserService.UpdateUserPrimaryEmailAsync(this.CurrentUserId, email);
            return this.Ok();
        }

        /// <summary>
        /// Upgrade to full access.
        /// </summary>
        /// <param name="userId">
        /// The userId.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("UpgradeAsFullAccessUser/{userId}/{email}")]
        public async Task<IActionResult> UpgradeAsFullAccessUser(int userId, string email)
        {
            var result = await this.securityService.UpgradeAsFullAccessUser(userId, email);
            return this.Ok(result);
        }
    }
}