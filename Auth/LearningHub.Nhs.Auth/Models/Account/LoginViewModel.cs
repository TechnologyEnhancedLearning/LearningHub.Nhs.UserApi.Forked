// <copyright file="LoginViewModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Models.Account
{
    using System.Collections.Generic;
    using System.Linq;
    using LearningHub.Nhs.Models.Common;

    /// <summary>
    /// The login view model.
    /// </summary>
    public class LoginViewModel : LoginInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether allow remember login.
        /// </summary>
        public bool AllowRememberLogin { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether enable local login.
        /// </summary>
        public bool EnableLocalLogin { get; set; } = true;

        /// <summary>
        /// Gets or sets the external providers.
        /// </summary>
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

        /// <summary>
        /// Gets the visible external providers.
        /// </summary>
        public IEnumerable<ExternalProvider> VisibleExternalProviders => this.ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        /// <summary>
        /// Gets a value indicating whether the is external login only.
        /// </summary>
        public bool IsExternalLoginOnly => this.EnableLocalLogin == false && this.ExternalProviders?.Count() == 1;

        /// <summary>
        /// Gets the external login scheme.
        /// </summary>
        public string ExternalLoginScheme => this.IsExternalLoginOnly ? this.ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

        /// <summary>
        /// Gets or sets the login client template.
        /// </summary>
        public LoginClientTemplate LoginClientTemplate { get; set; }

        /// <summary>
        /// Gets or sets the ClientId.
        /// </summary>
        public string ClientId { get; set; }
    }
}
