namespace LearningHub.Nhs.Auth.Models.Grants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The grant view model.
    /// </summary>
    public class GrantViewModel
    {
        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client url.
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the client logo url.
        /// </summary>
        public string ClientLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the identity grant names.
        /// </summary>
        public IEnumerable<string> IdentityGrantNames { get; set; }

        /// <summary>
        /// Gets or sets the api grant names.
        /// </summary>
        public IEnumerable<string> ApiGrantNames { get; set; }
    }
}