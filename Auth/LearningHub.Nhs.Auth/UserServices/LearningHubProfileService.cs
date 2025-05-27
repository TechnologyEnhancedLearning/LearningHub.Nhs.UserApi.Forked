namespace LearningHub.Nhs.Auth.UserServices
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The learning hub profile service.
    /// </summary>
    public class LearningHubProfileService : IProfileService
    {
        private readonly WebSettings webSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubProfileService"/> class.
        /// </summary>
        /// <param name="userService">
        /// The user service.
        /// </param>
        /// <param name="moodleApiService">
        /// The moodle api service.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="webSettings">
        /// The webSettings.
        /// </param>
        public LearningHubProfileService(IUserService userService, IMoodleApiService moodleApiService, ILogger<LearningHubProfileService> logger, WebSettings webSettings)
        {
            this.UserService = userService;
            this.MoodleApiService = moodleApiService;
            this.Logger = logger;
            this.webSettings = webSettings;
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
        /// Gets the moodle api service.
        /// </summary>
        protected IMoodleApiService MoodleApiService { get; }

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

                if (this.webSettings.EnableMoodle)
                {
                    var moodleUser = await this.MoodleApiService.GetMoodleUserIdByUsernameAsync(user.Id);
                    claims.Add(new Claim("preferred_username", moodleUser.ToString()));
                }

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
