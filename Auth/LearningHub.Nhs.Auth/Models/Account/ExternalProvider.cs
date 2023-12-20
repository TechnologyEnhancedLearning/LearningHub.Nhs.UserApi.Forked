// <copyright file="ExternalProvider.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Models.Account
{
    /// <summary>
    /// The external provider out view model.
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the authentication scheme.
        /// </summary>
        public string AuthenticationScheme { get; set; }
    }
}
