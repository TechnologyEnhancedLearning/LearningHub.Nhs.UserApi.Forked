namespace LearningHub.Nhs.Auth.ViewModels
{
    using System.Collections.Generic;
    using System.Security.Claims;

    /// <summary>
    /// The oa callback view model.
    /// </summary>
    public class OaCallBackViewModel
    {
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the provider user id.
        /// </summary>
        public string ProviderUserId { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        public IEnumerable<Claim> Claims { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }
    }
}
