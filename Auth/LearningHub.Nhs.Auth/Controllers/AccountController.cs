// <copyright file="AccountController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using IdentityModel;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Extensions;
    using LearningHub.Nhs.Auth.Filters;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.Models.Account;
    using LearningHub.Nhs.Caching;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Entities.Reporting;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using NHSUKViewComponents.Web.ViewModels;

    /// <summary>
    /// Account Controller operations.
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : IdentityServerController
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly LearningHubAuthConfig authConfig;
        private readonly WebSettings webSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="interaction">interaction parameter.</param>
        /// <param name="clientStore">clientStore parameter.</param>
        /// <param name="schemeProvider">schemeProvider parameter.</param>
        /// <param name="events">events parameter.</param>
        /// <param name="userService">userService parameter.</param>
        /// <param name="webSettings">webSettings parameter.</param>
        /// <param name="authConfig">Auth service config.</param>
        /// <param name="cacheService">Cacje service config.</param>
        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IUserService userService,
            WebSettings webSettings,
            IOptions<LearningHubAuthConfig> authConfig,
            ICacheService cacheService)
            : base(userService, events, clientStore, webSettings, cacheService)
        {
            this.interaction = interaction;
            this.schemeProvider = schemeProvider;
            this.authConfig = authConfig?.Value;
            this.webSettings = webSettings;
    }

        /// <summary>
        /// Shows the Login page.
        /// </summary>
        /// <param name="returnUrl">returnUrl parameter.</param>
        /// <returns>Login ViewModel.</returns>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // Use internal login page
            // build a model so we know what to show on the login page
            var vm = await this.BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return this.RedirectToAction("Challenge", "External", new { provider = vm.ExternalLoginScheme, returnUrl });
            }

            if ((vm.ClientId == "learninghubwebclient") || (vm.ClientId == "learninghubadmin"))
            {
                this.ViewData["Layout"] = vm.LoginClientTemplate.LayoutPath;
                this.ViewBag.SupportFormUrl = this.webSettings.SupportForm;
                return this.View("LHLogin", vm);
            }
            else
            {
                return this.View(vm);
            }
        }

        /// <summary>
        /// Handles the post back from the Login page.
        /// </summary>
        /// <param name="model">Login parameters.</param>
        /// <param name="button">Button name.</param>
        /// <returns>Login ViewModel.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            var decodeReturnUrl = WebUtility.UrlDecode(model.ReturnUrl);
            var context = await this.interaction.GetAuthorizationContextAsync(decodeReturnUrl);

            // the user clicked the "cancel" button
            if (button == "cancel")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await this.interaction.GrantConsentAsync(
                        context,
                        new ConsentResponse { Error = AuthorizationError.AccessDenied });

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await this.ClientStore.IsPkceClientAsync(context.Client.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                    }

                    return this.Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return this.Redirect("~/");
                }
            }

            if (this.ModelState.IsValid)
            {
                // validate username/password
                var loginResult = await this.UserService.AuthenticateUserAsync(model.Username.Trim(), model.Password.Trim());
                int userId;
                try
                {
                    userId = await this.UserService.GetUserIdByUserNameAsync(model.Username.Trim());
                }
                catch (Exception)
                {
                    userId = 0;
                }

                if (loginResult.IsAuthenticated)
                {
                    await this.SignInUser(userId, model.Username.Trim(), model.RememberLogin, context.Parameters["ext_referer"]);

                    if (context != null)
                    {
                        if (await this.ClientStore.IsPkceClientAsync(context.Client.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return this.Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (this.Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return this.Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }
                else if (userId > 0)
                {
                    await this.UserService.AddLogonToUserHistory(
                        $"Login failed for user {model.Username.Trim()}",
                        userId,
                        UserHistoryType.Logon,
                        false,
                        this.Request,
                        context.Parameters["ext_referer"]);
                }

                await this.Events.RaiseAsync(new UserLoginFailureEvent(model.Username.Trim(), loginResult.ErrorMessage));
                this.ModelState.AddModelError(nameof(model.Username), "Enter your username again");
                this.ModelState.AddModelError(nameof(model.Password), "Enter your password again");
                this.ModelState.AddModelError(string.Empty, loginResult.ErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await this.BuildLoginViewModelAsync(model);
            if ((vm.ClientId == "learninghubwebclient") || (vm.ClientId == "learninghubadmin"))
            {
                this.ViewData["Layout"] = vm.LoginClientTemplate.LayoutPath;
                this.ViewBag.SupportFormUrl = this.webSettings.SupportForm;
                return this.View("LHLogin", vm);
            }
            else
            {
                return this.View(vm);
            }
        }

        /// <summary>
        /// Show logout page.
        /// </summary>
        /// <param name="logoutId">logoutId parameter.</param>
        /// <returns>Logout ViewModel.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await this.BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await this.Logout(vm);
            }

            return this.View(vm);
        }

        /// <summary>
        /// Handle logout page post back.
        /// </summary>
        /// <param name="model">logout parameters.</param>
        /// <returns>Logout ViewModel.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await this.BuildLoggedOutViewModelAsync(model.LogoutId);

            if (this.User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await this.HttpContext.SignOutAsync();

                // raise the logout event
                await this.Events.RaiseAsync(new UserLogoutSuccessEvent(this.User.GetSubjectId(), this.User.GetDisplayName()));

                int.TryParse(this.User.GetSubjectId(), out var userId);

                // Add successful logout to the UserHistory
                UserHistoryViewModel userHistory = new UserHistoryViewModel()
                {
                    UserId = userId,
                    UserHistoryTypeId = (int)UserHistoryType.Logout,
                    Detail = @"User logged out",
                };

                await this.UserService.StoreUserHistoryAsync(userHistory);
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = this.Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return this.SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return this.View("LoggedOut", vm);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="clientUrl">The client url.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="invalidScope">The invalid scope.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public IActionResult OpenAthens(string clientUrl, string returnUrl, bool invalidScope = false)
        {
            var url = clientUrl + @"/openathens/login";
            var uri = new Uri(url);

            var queryParams = new List<string>();
            queryParams.Add($"returnUrl={Uri.EscapeDataString(returnUrl)}");
            queryParams.Add($"invalidScope={invalidScope}");
            var queryString = string.Join("&", queryParams);

            return this.Redirect($"{url}?{queryString}");
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/

        /// <summary>
        /// The build login view model async.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(returnUrl);
            LoginClientTemplate loginClientTemplate = null;
            if (context?.Client.ClientId != null && this.authConfig.IdsClients.ContainsKey(context.Client.ClientId))
            {
                loginClientTemplate = this.authConfig.IdsClients[context.Client.ClientId];

                // Base _layout.cshtml template config
                this.ViewData["AuthMainTitle"] = loginClientTemplate.AuthMainTitle;
                this.ViewData["ClientUrl"] = loginClientTemplate.ClientUrl;
                this.ViewData["ClientLogoUrl"] = loginClientTemplate.ClientLogoUrl;
                this.ViewData["ClientLogoSrc"] = loginClientTemplate.ClientLogoSrc;
                this.ViewData["ClientLogoAltText"] = loginClientTemplate.ClientLogoAltText;
                this.ViewData["HeaderCssClass"] = loginClientTemplate.ClientCssClass;

                if (context?.IdP != null)
                {
                    var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                    // this is meant to short circuit the UI and only trigger the one external IdP
                    var vm = new LoginViewModel
                    {
                        EnableLocalLogin = local,
                        ReturnUrl = returnUrl,
                        Username = context.LoginHint,
                        LoginClientTemplate = loginClientTemplate,
                    };

                    if (!local)
                    {
                        vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                    }

                    return vm;
                }
            }

            var schemes = await this.schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name,
                }).ToList();

            var allowLocal = true;

            if (context?.Client.ClientId != null)
            {
                var client = await this.ClientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = loginClientTemplate?.AllowRememberLogin ?? AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray(),
                LoginClientTemplate = loginClientTemplate ?? new LoginClientTemplate(),
                ClientId = context.Client.ClientId,
            };
        }

        /// <summary>
        /// The build login view model async.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await this.BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        /// <summary>
        /// The build logout view model async.
        /// </summary>
        /// <param name="logoutId">
        /// The logout id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (this.User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await this.interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        /// <summary>
        /// The build logged out view model async.
        /// </summary>
        /// <param name="logoutId">
        /// The logout id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await this.interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId,
            };

            if (this.User?.Identity.IsAuthenticated == true)
            {
                var idp = this.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await this.HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await this.interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}