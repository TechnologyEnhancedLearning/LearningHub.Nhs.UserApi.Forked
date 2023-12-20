// <copyright file="SsoController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using IdentityServer4;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using LearningHub.Nhs.Auth.Filters;
    using LearningHub.Nhs.Auth.Helpers;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.ViewModels.Sso;
    using LearningHub.Nhs.Models.Entities.External;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using UAParser;

    /// <summary>
    /// SSO Controller operations.
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class SsoController : Controller
    {
        private readonly IExternalSystemService externalSystemService;
        private readonly IRegistrationService registrationService;
        private readonly IUserService userService;
        private readonly IEventService eventService;
        private readonly ILogger<SsoController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SsoController"/> class.
        /// </summary>
        /// <param name="externalSystemService">External system service.</param>
        /// <param name="registrationService">Registration info service.</param>
        /// <param name="userService">User service.</param>
        /// <param name="eventService">Event service.</param>
        /// <param name="logger">The logger.</param>
        public SsoController(
            IExternalSystemService externalSystemService,
            IRegistrationService registrationService,
            IUserService userService,
            IEventService eventService,
            ILogger<SsoController> logger)
        {
            this.externalSystemService = externalSystemService;
            this.registrationService = registrationService;
            this.userService = userService;
            this.eventService = eventService;
            this.logger = logger;
        }

        /// <summary>
        /// SSO login.
        /// </summary>
        /// <param name="request">Single-sign-on login request.</param>
        /// <returns>Login view.</returns>
        [HttpGet("sso/login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            try
            {
                if (!this.ModelState.IsValid || request == null)
                {
                    this.logger.LogError($"SSO LoginAsync; Invalid request: {(request != null ? JsonConvert.SerializeObject(request) : "request is null")}");
                    return this.View("NotAuthenticated");
                }

                var client = await this.externalSystemService.GetExternalSystem(request.ClientCode);
                if (client == null)
                {
                    this.logger.LogError($"SSO LoginAsync; Can not find client, request: {JsonConvert.SerializeObject(request)}");
                    return this.View("NotAuthenticated");
                }

                var redirectUrl = request.EndClientUrl;

                if (!string.IsNullOrWhiteSpace(request.EndClientCode))
                {
                    var deepLink = await this.externalSystemService.GetExternalClientDeepLink(request.EndClientCode);
                    if (deepLink == null)
                    {
                        this.logger.LogError($"SSO LoginAsync; Can not find end client deepLink, request: {JsonConvert.SerializeObject(request)}");
                        return this.View("NotAuthenticated");
                    }

                    redirectUrl = deepLink.DeepLink;
                }

                var userId = $"{request.UserId}";

                if (!SecurityHelper.VerifyHash(userId, client.SecretKey, request.Hash))
                {
                    this.logger.LogError($"SSO LoginAsync; invalid hash, request: {JsonConvert.SerializeObject(request)}");
                    return this.View("NotAuthenticated");
                }

                var loginResult = await this.userService.AuthenticateSsoUserAsync(request.UserId, client.Id, client.Code);

                if (loginResult?.IsAuthenticated == true)
                {
                    var userHistory = this.GetUserHistoryViewModel(request.UserId, client.Name);

                    await Task.WhenAll(
                        this.eventService.RaiseAsync(new UserLoginSuccessEvent(loginResult.UserName, userId, loginResult.UserName)),
                        this.HttpContext.SignInAsync(new IdentityServerUser(userId) { DisplayName = loginResult.UserName }),
                        this.userService.StoreUserHistoryAsync(userHistory));

                    var param = new Dictionary<string, string>() { { "FromAuthService", "true" } };
                    var ssoRedirectUrl = QueryHelpers.AddQueryString(redirectUrl, param);

                    return this.Redirect(ssoRedirectUrl);
                }

                this.logger.LogError($"SSO LoginAsync; user not authenticated, request: {JsonConvert.SerializeObject(request)}");

                return this.View("NotAuthenticated", loginResult?.ErrorMessage ?? "We cannot find your details within our system, please contact your support for any further help.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SSO login error");
                return this.View("Error");
            }
        }

        /// <summary>
        /// Get create sso user view.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <param name="state">The state.</param>
        /// <param name="hash">Generated hash.</param>
        /// <returns>Create user view.</returns>
        [HttpGet("sso/create-user")]
        public async Task<IActionResult> CreateUserAsync(string clientCode, string state, string hash)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(clientCode)
                || string.IsNullOrWhiteSpace(state)
                || string.IsNullOrWhiteSpace(hash))
                {
                    this.logger.LogError($"SSO CreateUserAsync; invalid request, clientCode:{clientCode}, state:{state}, hash:{hash}");
                    return this.View("NotAuthenticated");
                }

                var client = await this.externalSystemService.GetExternalSystem(clientCode);

                if (client == null || !SecurityHelper.VerifyHash(state, client.SecretKey, hash))
                {
                    this.logger.LogError($"SSO CreateUserAsync; invalid hash, clientCode:{clientCode}, state:{state}, hash:{hash}");
                    return this.View("NotAuthenticated");
                }

                var vm = new CreateUserViewModel().SetClientInfo(client, state);
                await this.PopulateDropdowns(vm);

                return this.View("Create", vm);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SSO create user error");
                return this.View("Error");
            }
        }

        /// <summary>
        /// Process registering sso user.
        /// </summary>
        /// <param name="request">Sso regiter user view model.</param>
        /// <param name="clientCode">The client code.</param>
        /// <param name="state">The state.</param>
        /// <returns>Create user view.</returns>
        [HttpPost("sso/register/{clientCode}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUserAsync([Bind(Prefix = "SsoRegisterUserForm")] RegisterUserViewModel request, string clientCode, string state)
        {
            try
            {
                var client = await this.externalSystemService.GetExternalSystem(clientCode);
                if (client == null || client.DefaultUserGroupId.HasValue == false)
                {
                    return this.View("NotAuthenticated");
                }

                if (!this.ModelState.IsValid || request == null)
                {
                    return this.View("Create", await this.BuildCreateUserViewModel(request, client, state));
                }

                var result = await this.registrationService.RegisterUser(request, client.Id);
                if (!result.IsValid)
                {
                    var model = await this.BuildCreateUserViewModel(request, client, state);
                    model.Error = string.Join(", ", result.Details);
                    return this.View("Create", model);
                }

                return this.ClientCallback(result.CreatedId.Value, client, state);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SSO register user error");
                return this.View("Error");
            }
        }

        /// <summary>
        /// Process link existing user to sso.
        /// </summary>
        /// <param name="request">Sso regiter user view model.</param>
        /// <param name="clientCode">The client code.</param>
        /// <param name="state">The state.</param>
        /// <returns>Create user view.</returns>
        [HttpPost("sso/link-user/{clientCode}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkUserAsync([Bind(Prefix = "SsoLinkUserForm")] LinkUserViewModel request, string clientCode, string state)
        {
            try
            {
                var client = await this.externalSystemService.GetExternalSystem(clientCode);
                if (client == null)
                {
                    return this.View("NotAuthenticated");
                }

                if (!this.ModelState.IsValid || request == null)
                {
                    return this.View("Create", await this.BuildCreateUserViewModel(request, client, state));
                }

                var result = await this.registrationService.LinkUserToSso(request.Username, request.Password, client.Id, client.Code);
                if (!result.IsAuthenticated)
                {
                    var model = await this.BuildCreateUserViewModel(request, client, state);
                    model.Error = result.ErrorMessage;
                    return this.View("Create", model);
                }

                return this.ClientCallback(result.UserId, client, state);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "SSO link user error");
                return this.View("Error");
            }
        }

        private RedirectResult ClientCallback(int userId, ExternalSystem client, string state)
        {
            var hash = SecurityHelper.GenerateHash($"{userId}", client.SecretKey);

            return this.Redirect($"{client.CallbackUrl}?userid={userId}&hash={HttpUtility.UrlEncode(hash)}&state={state}");
        }

        private async Task<CreateUserViewModel> BuildCreateUserViewModel(RegisterUserViewModel request, ExternalSystem client, string state)
        {
            var vm = new CreateUserViewModel().SetClientInfo(client, state);
            request.SetRegistrationInfo(vm);
            await this.PopulateDropdowns(vm);
            return vm;
        }

        private async Task<CreateUserViewModel> BuildCreateUserViewModel(LinkUserViewModel request, ExternalSystem client, string state)
        {
            var vm = new CreateUserViewModel().SetClientInfo(client, state);
            request.SetLinkUserInfo(vm);
            await this.PopulateDropdowns(vm);
            return vm;
        }

        private async Task PopulateDropdowns(CreateUserViewModel vm)
        {
            var registerForm = vm.SsoRegisterUserForm;

            registerForm.Specialties.AddRange(await this.registrationService.GetSpecialtiesAsync());

            var staffGroupList = await this.registrationService.GetStaffGroupsAsync();
            registerForm.StaffGroups.AddRange(staffGroupList.OrderBy(x => x.DisplayOrder));

            if (registerForm.StaffGroupId.HasValue)
            {
                registerForm.JobRoles.AddRange(await this.registrationService.GetByStaffGroupIdAsync(registerForm.StaffGroupId.Value));
            }

            if (registerForm.JobRoleId.HasValue)
            {
                registerForm.Grades.AddRange(await this.registrationService.GetGradesForJobRoleAsync(registerForm.JobRoleId.Value));
            }
        }

        private UserHistoryViewModel GetUserHistoryViewModel(int userId, string clientName)
        {
            var clientInfo = Parser.GetDefault().Parse(this.Request.Headers["User-Agent"]);

            var userHistory = new UserHistoryViewModel
            {
                UserId = userId,
                UserHistoryTypeId = (int)UserHistoryType.Logon,
                Detail = $"User logged on. Source of auth: {clientName}",
                UserAgent = this.Request.Headers["User-Agent"],
                BrowserName = clientInfo.UA.Family,
                BrowserVersion = clientInfo.UA.Major + "." + clientInfo.UA.Minor + "." + clientInfo.UA.Patch,
                UrlReferer = this.Request.Headers["Referer"],
                LoginSuccessFul = true,
                LoginIP = this.Request.HttpContext.Connection.RemoteIpAddress.ToString(),
            };

            return userHistory;
        }
    }
}