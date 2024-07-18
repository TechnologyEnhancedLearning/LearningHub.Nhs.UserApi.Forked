namespace LearningHub.Nhs.Auth.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The user basic view model.
    /// </summary>
    public class UserBasicViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
