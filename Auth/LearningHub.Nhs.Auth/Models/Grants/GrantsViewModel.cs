namespace LearningHub.Nhs.Auth.Models.Grants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The grants view model.
    /// </summary>
    public class GrantsViewModel
    {
        /// <summary>
        /// Gets or sets the grants.
        /// </summary>
        public IEnumerable<GrantViewModel> Grants { get; set; }
    }
}