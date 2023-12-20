// <copyright file="LogoutViewModel.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Models.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The logout model.
    /// </summary>
    public class LogoutViewModel : LogoutInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the logout prompt is shown.
        /// </summary>
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
