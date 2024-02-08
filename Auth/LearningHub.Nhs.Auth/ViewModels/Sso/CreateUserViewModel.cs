namespace LearningHub.Nhs.Auth.ViewModels.Sso
{
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// The single-sign-on user view model.
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Gets or sets client code.
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets client state.
        /// </summary>
        public string ClientState { get; set; }

        /// <summary>
        /// Gets or sets client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether registration is allowed.
        /// </summary>
        public bool AllowRegistration { get; set; }

        /// <summary>
        /// Gets or sets terms and conditions.
        /// </summary>
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// Gets or sets the sso link user form.
        /// </summary>
        public LinkUserViewModel SsoLinkUserForm { get; set; }

        /// <summary>
        /// Gets or sets the sso regiter user form.
        /// </summary>
        public RegisterUserViewModel SsoRegisterUserForm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether registration is allowed.
        /// </summary>
        public bool ShowLinkUserForm { get; set; } = true;

        /// <summary>
        /// Gets or sets error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Set Sso Create User View Model.
        /// </summary>
        /// <param name="client">The client info.</param>
        /// <param name="state">The state.</param>
        /// <returns>The <see cref="CreateUserViewModel"/>.</returns>
        public CreateUserViewModel SetClientInfo(ExternalSystem client, string state)
        {
            this.ClientCode = client.Code;
            this.ClientName = client.Name;
            this.TermsAndConditions = client.TermsAndConditions;
            this.ClientState = state;
            this.AllowRegistration = client.DefaultUserGroupId.HasValue;
            this.SsoRegisterUserForm = new RegisterUserViewModel
            {
                StaffGroupId = client.DefaultStaffGroupId,
                JobRoleId = client.DefaultJobRoleId,
                GradeId = client.DefaultGradingId,
                SpecialtyId = client.DefaultSpecialityId,
                LocationId = client.DefaultLocationId,
                ShowStaffGroupOption = !client.DefaultStaffGroupId.HasValue,
                ShowJobRoleOption = !client.DefaultJobRoleId.HasValue,
                ShowGradingOption = !client.DefaultGradingId.HasValue,
                ShowSpecialityOption = !client.DefaultSpecialityId.HasValue,
                ShowLocation = !client.DefaultLocationId.HasValue,
            };

            return this;
        }
    }
}