// <copyright file="LoginRequest.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.ViewModels.Sso
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The single-sign-on login request.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets calling system client code.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets generated hash.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets end client code.
        /// </summary>
        [MinLength(2)]
        public string EndClientCode { get; set; }

        /// <summary>
        /// Gets or sets end client code.
        /// </summary>
        [MinLength(10)]
        public string EndClientUrl { get; set; }

        /// <summary>
        /// Gets a value indicating whether redirect values is present.
        /// </summary>
        [RegularExpression(@"True", ErrorMessage = "Either EndClientCode or EndClientUrl must have a value")]
        public string ValidRedirectValue => (!string.IsNullOrWhiteSpace(this.EndClientCode) || !string.IsNullOrWhiteSpace(this.EndClientUrl)).ToString();
    }
}