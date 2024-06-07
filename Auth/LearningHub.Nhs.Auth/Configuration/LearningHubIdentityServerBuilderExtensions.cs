namespace LearningHub.Nhs.Auth.Configuration
{
    using LearningHub.Nhs.Auth.UserServices;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The learning hub identity server builder extensions.
    /// </summary>
    public static class LearningHubIdentityServerBuilderExtensions
    {
        /// <summary>
        /// The add learning hub user store.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        /// <returns>
        /// The <see cref="IIdentityServerBuilder"/>.
        /// </returns>
        public static IIdentityServerBuilder AddLearningHubUserStore(this IIdentityServerBuilder builder)
        {
            builder.AddProfileService<LearningHubProfileService>();
            builder.AddResourceOwnerValidator<LearningHubResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}
