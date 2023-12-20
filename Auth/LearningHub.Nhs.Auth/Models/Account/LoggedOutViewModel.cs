// <copyright file="LoggedOutViewModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Models.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The logged out view model.
    /// </summary>
    public class LoggedOutViewModel
    {
        /// <summary>
        /// Gets or sets the post logout redirect uri.
        /// </summary>
        public string PostLogoutRedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the sign out iframe url.
        /// </summary>
        public string SignOutIframeUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether automatic redirect after sign out.
        /// </summary>
        public bool AutomaticRedirectAfterSignOut { get; set; } = false;

        /// <summary>
        /// Gets or sets the logout id.
        /// </summary>
        public string LogoutId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the trigger external signout.
        /// </summary>
        public bool TriggerExternalSignout => this.ExternalAuthenticationScheme != null;

        /// <summary>
        /// Gets or sets the external authentication scheme.
        /// </summary>
        public string ExternalAuthenticationScheme { get; set; }
    }
}
