namespace LearningHub.Nhs.UserApi.Shared.Configuration
{
    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets the authentication service url.
        /// </summary>
        public string AuthenticationServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the auth client identity key.
        /// </summary>
        public string AuthClientIdentityKey { get; set; }

        /// <summary>
        /// Gets or sets the lh client identity key.
        /// </summary>
        public string LHClientIdentityKey { get; set; }

        /// <summary>
        /// Gets or sets the max logon attempts.
        /// </summary>
        public int MaxLogonAttempts { get; set; }

        /// <summary>
        /// Gets or sets the learning hub tenant id.
        /// </summary>
        public int LearningHubTenantId { get; set; }

        /// <summary>
        /// Gets or sets the ELFH cache settings.
        /// </summary>
        public ElfhCacheSettings ElfhCacheSettings { get; set; }

        /// <summary>
        /// Gets or sets the learning hub url.
        /// </summary>
        public string LearningHubUrl { get; set; }

        /// <summary>
        /// Gets or sets the website emails to.
        /// </summary>
        public string WebsiteEmailsTo { get; set; }

        /// <summary>
        /// Gets or sets the security questions required.
        /// </summary>
        public int SecurityQuestionsRequired { get; set; }
    }
}
