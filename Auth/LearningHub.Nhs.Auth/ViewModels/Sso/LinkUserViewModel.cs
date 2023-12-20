// <copyright file="LinkUserViewModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.ViewModels.Sso
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The single-sign-on link user request.
    /// </summary>
    public class LinkUserViewModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required(ErrorMessage = "Enter your username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Enter your password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consented.
        /// </summary>
        [MustBeTrue(ErrorMessage = "Accept the terms and conditions")]
        public bool Consented { get; set; }

        /// <summary>
        /// Set Sso Create User View Model.
        /// </summary>
        /// <param name="vm">Sso Create User View Model.</param>
        public void SetLinkUserInfo(CreateUserViewModel vm)
        {
            vm.ShowLinkUserForm = true;
            vm.SsoLinkUserForm = new LinkUserViewModel
            {
                Username = this.Username,
                Password = this.Password,
            };
        }
    }
}