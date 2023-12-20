// <copyright file="LearningHubProfileService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.UserServices
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using LearningHub.Nhs.Auth.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The learning hub profile service.
    /// </summary>
    public class LearningHubProfileService : IProfileService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubProfileService"/> class.
        /// </summary>
        /// <param name="userService">
        /// The user service.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public LearningHubProfileService(IUserService userService, ILogger<LearningHubProfileService> logger)
        {
            this.UserService = userService;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the user service.
        /// </summary>
        protected IUserService UserService { get; }

        /// <summary>
        /// The get profile data async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context != null)
            {
                var user = await this.UserService.GetBasicUserByUserIdAsync(context.Subject.GetSubjectId());
                var roleName = await this.UserService.GetUserRoleAsync(user.Id);

                var claims = new List<Claim>
                                 {
                                     // new Claim("sub", sub),
                                     new Claim("email", user.EmailAddress),
                                     new Claim("given_name", user.FirstName),
                                     new Claim("family_name", user.LastName),
                                     new Claim("role", roleName),
                                     new Claim("elfh_userName", user.UserName),
                                 };

                if (context.Subject.HasClaim("openAthensUser", "true"))
                {
                    claims.Add(new Claim("openAthensUser", "true"));
                }

                context.IssuedClaims = claims;
            }
        }

        /// <summary>
        /// The is active async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            if (context != null)
            {
                var user = await this.UserService.GetBasicUserByUserIdAsync(context.Subject.GetSubjectId());
                context.IsActive = user != null;
            }
        }
    }
}
