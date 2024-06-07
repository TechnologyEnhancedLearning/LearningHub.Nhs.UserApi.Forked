namespace LearningHub.Nhs.Auth.UserServices
{
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4.Validation;
    using LearningHub.Nhs.Auth.Interfaces;

    /// <summary>
    /// The learning hub resource owner password validator.
    /// </summary>
    public class LearningHubResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// Gets the user service.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="userService">
        /// The user service.
        /// </param>
        public LearningHubResourceOwnerPasswordValidator(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// The validate async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if ((await this.userService.AuthenticateUserAsync(context.UserName, context.Password)).IsAuthenticated)
            {
                var user = await this.userService.GetUserByUserNameAsync(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}
