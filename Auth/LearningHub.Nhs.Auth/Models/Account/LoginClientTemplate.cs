// <copyright file="LoginClientTemplate.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Models.Account
{
    using System.Collections.Generic;

    /// <summary>
    /// The login Client template.
    /// </summary>
    public class LoginClientTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginClientTemplate"/> class.
        /// </summary>
        public LoginClientTemplate()
        {
            this.UseForgottenPassword = true;
            this.UseRegister = true;
            this.UseSupport = true;
            this.RedirectUris = new HashSet<string>();
            this.PostLogoutUris = new HashSet<string>();
            this.Scopes = new HashSet<string>();
            this.AllowRememberLogin = false;
        }

        /// <summary>
        /// Gets or sets the Client description.
        /// </summary>
        public string ClientDescription { get; set; }

        /// <summary>
        /// Gets or sets the auth secret.
        /// </summary>
        public string AuthSecret { get; set; }

        /// <summary>
        /// Gets or sets the Client url.
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the redirect uris.
        /// </summary>
        public ICollection<string> RedirectUris { get; set; }

        /// <summary>
        /// Gets or sets the post logout uris.
        /// </summary>
        public ICollection<string> PostLogoutUris { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        public ICollection<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the auth main title.
        /// </summary>
        public string AuthMainTitle { get; set; }

        /// <summary>
        /// Gets or sets the Client logo src.
        /// </summary>
        public string ClientLogoSrc { get; set; }

        /// <summary>
        /// Gets or sets the Client logo alt text.
        /// </summary>
        public string ClientLogoAltText { get; set; }

        /// <summary>
        /// Gets or sets the Client logo url.
        /// </summary>
        public string ClientLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the Client panel css class.
        /// </summary>
        public string ClientCssClass { get; set; }

        /// <summary>
        /// Gets or sets the forgotten password relative url.
        /// </summary>
        public string ForgottenPasswordRelativeUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use forgotten password.
        /// </summary>
        public bool UseForgottenPassword { get; set; }

        /// <summary>
        /// Gets or sets the register account relative url.
        /// </summary>
        public string RegisterAccountRelativeUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use register.
        /// </summary>
        public bool UseRegister { get; set; }

        /// <summary>
        /// Gets or sets the support url.
        /// </summary>
        public string SupportUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use support.
        /// </summary>
        public bool UseSupport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to user Remember Me.
        /// </summary>
        public bool AllowRememberLogin { get; set; }

        /// <summary>
        /// Gets or sets the value of LayoutPath.
        /// </summary>
        public string LayoutPath { get; set; }
    }
}
