namespace LearningHub.Nhs.Auth.Configuration
{
    /// <summary>
    /// The open athens config.
    /// </summary>
    public class OpenAthensConfig
    {
        /// <summary>
        /// Gets or sets the authority.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the redirect uri.
        /// </summary>
        public string RedirectUri { get; set; }
    }
}
