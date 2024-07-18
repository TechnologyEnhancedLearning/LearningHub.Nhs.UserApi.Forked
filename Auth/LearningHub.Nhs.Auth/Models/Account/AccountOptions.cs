namespace LearningHub.Nhs.Auth.Models.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The account options.
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// The windows authentication scheme name.
        /// </summary>
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        /// <summary>
        /// Gets a value indicating whether the allow local login.
        /// </summary>
        public static bool AllowLocalLogin { get; } = true;

        /// <summary>
        /// Gets a value indicating whether the allow remember login.
        /// </summary>
        public static bool AllowRememberLogin { get; } = true;

        /// <summary>
        /// Gets the remember me login duration.
        /// </summary>
        public static TimeSpan RememberMeLoginDuration { get; } = TimeSpan.FromDays(30);

        /// <summary>
        /// Gets a value indicating whether the show logout prompt.
        /// </summary>
        public static bool ShowLogoutPrompt { get; } = true;

        /// <summary>
        /// Gets a value indicating whether the automatic redirect after sign out.
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut { get; } = true;

        /// <summary>
        /// Gets a value indicating whether the include windows groups. if user uses windows auth, should we load the groups from windows.
        /// </summary>
        public static bool IncludeWindowsGroups { get; } = false;
    }
}
