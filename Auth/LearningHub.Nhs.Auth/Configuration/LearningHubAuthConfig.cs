namespace LearningHub.Nhs.Auth.Configuration
{
    using System;
    using System.Collections.Generic;

    using LearningHub.Nhs.Auth.Models.Account;

    /// <summary>
    /// The learning hub auth config.
    /// </summary>
    public class LearningHubAuthConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubAuthConfig"/> class.
        /// </summary>
        public LearningHubAuthConfig()
        {
            this.LearningHubClientSecrets = new Dictionary<string, string>();
            this.IdsClients = new Dictionary<string, LoginClientTemplate>();
            this.AuthClients = new Dictionary<string, ClientAppConfig>();
        }

        /// <summary>
        /// Gets the learning hub client secrets.
        /// </summary>
        public Dictionary<string, string> LearningHubClientSecrets { get; }

        /// <summary>
        /// Gets or sets the auth client identity key.
        /// </summary>
        public Guid AuthClientIdentityKey { get; set; }

        /// <summary>
        /// Gets or sets the auth timeout.
        /// </summary>
        public int AuthTimeout { get; set; }

        /// <summary>
        /// Gets the elfh tenants.
        /// </summary>
        public Dictionary<string, LoginClientTemplate> IdsClients { get; }

        /// <summary>
        /// Gets the auth clients.
        /// </summary>
        public Dictionary<string, ClientAppConfig> AuthClients { get; }
    }
}
