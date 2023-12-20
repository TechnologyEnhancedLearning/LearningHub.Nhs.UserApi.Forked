// <copyright file="IdentityServerController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Controllers
{
    using System;
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using IdentityServer4;
    using IdentityServer4.Events;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.Models.Account;
    using LearningHub.Nhs.Caching;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using UAParser;

    /// <summary>
    /// Identity Server Operations.
    /// </summary>
    public abstract class IdentityServerController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerController"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        /// <param name="events">Events service.</param>
        /// <param name="clientStore">Client Store.</param>
        /// <param name="webSettings">Web Settings object.</param>
        /// <param name="cacheService">Cache service.</param>
        protected IdentityServerController(
            IUserService userService,
            IEventService events,
            IClientStore clientStore,
            WebSettings webSettings,
            ICacheService cacheService)
        {
            this.UserService = userService;
            this.Events = events;
            this.ClientStore = clientStore;
            this.WebSettings = webSettings;
            this.CacheService = cacheService;
        }

        /// <summary>
        /// Gets userService property.
        /// </summary>
        protected IUserService UserService { get; }

        /// <summary>
        /// Gets events property.
        /// </summary>
        protected IEventService Events { get; }

        /// <summary>
        /// Gets clientStore property.
        /// </summary>
        protected IClientStore ClientStore { get; }

        /// <summary>
        /// Gets webSettings property.
        /// </summary>
        protected WebSettings WebSettings { get; }

        /// <summary>
        /// Gets cacheService property.
        /// </summary>
        protected ICacheService CacheService { get; }

        /// <summary>
        /// Sign in User method.
        /// </summary>
        /// <param name="userId">
        /// userId parameter.
        /// </param>
        /// <param name="username">
        /// username parameter.
        /// </param>
        /// <param name="rememberLogin">
        /// rememberLogin parameter.
        /// </param>
        /// <param name="externalReferer">
        /// The external referer url.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// Void task returned.
        /// </returns>
        protected async Task SignInUser(int userId, string username, bool rememberLogin, string externalReferer, Claim[] claims = null)
        {
            await this.Events.RaiseAsync(new UserLoginSuccessEvent(username, userId.ToString(CultureInfo.InvariantCulture), username));

            // only set explicit expiration here if user chooses "remember me".
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (AccountOptions.AllowRememberLogin && rememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration),
                };
            }

            var identityServerUser = new IdentityServerUser(userId.ToString(CultureInfo.InvariantCulture))
            {
                DisplayName = username,
            };

            if (claims != null && claims.Length > 0)
            {
                foreach (var claim in claims)
                {
                    identityServerUser.AdditionalClaims.Add(claim);
                }
            }

            await this.HttpContext.SignInAsync(identityServerUser, props);
            await this.CacheService.RemoveAsync($"{userId}:AllRolesWithPermissions");

            // Add successful sign-in to the UserHistory
            await this.UserService.AddLogonToUserHistory(
                @"User logged on. Source of auth: LearningHub.Nhs.Auth Account\Login",
                userId,
                UserHistoryType.Logon,
                true,
                this.Request,
                externalReferer);
        }

        /// <summary>
        /// The sign in user for OpenAthens.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="rememberLogin">
        /// The remember login.
        /// </param>
        /// <param name="externalReferer">
        /// The external referer url.
        /// </param>
        /// <param name="oaUserId">
        /// The oa user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected async Task SignInUser(int userId, string username, bool rememberLogin, string externalReferer, string oaUserId)
        {
            await this.UserService.AddLogonToUserHistory(
                $"User has authenticated with OA ({oaUserId}). ELFH/LH ({username})",
                userId,
                UserHistoryType.OpenAthens,
                true,
                this.Request,
                externalReferer);

            await this.SignInUser(userId, username, rememberLogin, externalReferer, new[] { new Claim("openAthensUser", "true"), });
        }
    }
}