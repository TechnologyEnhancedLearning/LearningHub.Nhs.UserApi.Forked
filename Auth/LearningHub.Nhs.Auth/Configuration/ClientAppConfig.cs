// <copyright file="ClientAppConfig.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The learning hub client app auth config.
    /// </summary>
    public class ClientAppConfig
    {
        /// <summary>
        /// Gets or sets the base url.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the Client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client auth secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the allowed grant types.
        /// </summary>
        public ICollection<string> AllowedGrantTypes { get; set; }

        /// <summary>
        /// Gets or sets the redirect uris.
        /// </summary>
        public ICollection<string> RedirectUris { get; set; }

        /// <summary>
        /// Gets or sets the post logout uris.
        /// </summary>
        public ICollection<string> PostLogoutUris { get; set; }

        /// <summary>
        /// Gets or sets the allowed scopes.
        /// </summary>
        public ICollection<string> AllowedScopes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the back channel uri is required.
        /// </summary>
        public bool BackChannelLogoutSessionRequired { get; set; }

        /// <summary>
        /// Gets or sets the back channel uri.
        /// </summary>
        public string BackChannelLogoutUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether access tokens are updated on refresh.
        /// </summary>
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consent required.
        /// </summary>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pkce required.
        /// </summary>
        public bool RequirePkce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offline access required.
        /// </summary>
        public bool AllowOfflineAccess { get; set; }
    }
}
