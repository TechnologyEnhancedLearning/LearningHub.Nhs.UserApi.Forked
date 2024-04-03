namespace LearningHub.Nhs.Auth.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The user view model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="countryId">
        /// The country id.
        /// </param>
        /// <param name="loginTimes">
        /// The login times.
        /// </param>
        /// <param name="primaryUserEmploymentId">
        /// The primary user employment id.
        /// </param>
        /// <param name="regionId">
        /// The region id.
        /// </param>
        public UserViewModel(int? countryId, int loginTimes, int? primaryUserEmploymentId, int? regionId)
        {
            this.CountryId = countryId;
            this.LoginTimes = loginTimes;
            this.PrimaryUserEmploymentId = primaryUserEmploymentId;
            this.RegionId = regionId;
        }

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

        /// <summary>
        /// Gets or sets the alt email address.
        /// </summary>
        public string AltEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the registration code.
        /// </summary>
        public string RegistrationCode { get; set; }

        /// <summary>
        /// Gets or sets the active from date.
        /// </summary>
        public DateTimeOffset? ActiveFromDate { get; set; }

        /// <summary>
        /// Gets or sets the active to date.
        /// </summary>
        public DateTimeOffset? ActiveToDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the login times.
        /// </summary>
        public int LoginTimes { get; set; }

        /// <summary>
        /// Gets or sets the primary user employment id.
        /// </summary>
        public int? PrimaryUserEmploymentId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the preferred tenant id.
        /// </summary>
        public int PreferredTenantId { get; set; }
    }
}
