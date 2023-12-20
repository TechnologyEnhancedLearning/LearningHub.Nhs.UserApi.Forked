// <copyright file="AuthorizeOrCallFromLHHandler.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Authentication
{
    using System.Linq;
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Provide Authentication policy for Auth Service.
    /// </summary>
    public class AuthorizeOrCallFromLHHandler : AuthorizationHandler<AuthorizeOrCallFromLHRequirement>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IOptions<Settings> settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeOrCallFromLHHandler"/> class.
        /// Provide Authentication policy for Auth Service.
        /// </summary>
        /// <param name="contextAccessor">The context Accessor.</param>
        /// <param name="settings">The settings.</param>
        public AuthorizeOrCallFromLHHandler(IHttpContextAccessor contextAccessor, IOptions<Settings> settings)
        {
            this.contextAccessor = contextAccessor;
            this.settings = settings;
        }

        /// <summary>
        /// Handle Authentication policy Requirement.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="requirement">The requirement.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeOrCallFromLHRequirement requirement)
        {
            bool callFromAuthService = false;
            bool callFromLearningHubClient = false;

            if (!context.User.Identity.IsAuthenticated)
            {
                var headers = this.contextAccessor.HttpContext.Request.Headers;

                // Note: headers.ContainsKey and headers.TryGetValue are case-insensitive.
                if (headers.ContainsKey("Client-Identity-Key"))
                {
                    Microsoft.Extensions.Primitives.StringValues clientKeyValues;
                    if (headers.TryGetValue("Client-Identity-Key", out clientKeyValues))
                    {
                        string clientKey = clientKeyValues.First().ToUpperInvariant();

                        callFromAuthService = clientKey
                                          == this.settings.Value.AuthClientIdentityKey.ToUpperInvariant();

                        callFromLearningHubClient = clientKey
                                                == this.settings.Value.LHClientIdentityKey.ToUpperInvariant();
                    }
                }
            }

            if (!callFromAuthService && !callFromLearningHubClient && !context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
