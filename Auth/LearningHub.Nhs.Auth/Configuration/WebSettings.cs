namespace LearningHub.Nhs.Auth.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Web Settings Class.
    /// </summary>
    public class WebSettings
    {
        /// <summary>
        /// Gets or sets the BuildNumber.
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Gets or sets the user api url.
        /// </summary>
        public string UserApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the learning hub admin url.
        /// </summary>
        public string LearningHubAdminUrl { get; set; }

        /// <summary>
        /// Gets or sets the x 509 certificate 2 thumbprint.
        /// </summary>
        public string X509Certificate2Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets the elfh client mvc url.
        /// </summary>
        public string ElfhClientMvcUrl { get; set; }

        /// <summary>
        /// Gets or sets the learning hub web client.
        /// </summary>
        public string LearningHubWebClient { get; set; }

        /// <summary>
        /// Gets or sets the elfh hub.
        /// </summary>
        public string ElfhHub { get; set; }

        /// <summary>
        /// Gets or sets the rcr.
        /// </summary>
        public string Rcr { get; set; }

        /// <summary>
        /// Gets or sets the SupportForm.
        /// </summary>
        public string SupportForm { get; set; }

        /// <summary>
        /// Gets or sets the SupportFeedbackForm.
        /// </summary>
        public string SupportFeedbackForm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsPasswordUpdate.
        /// </summary>
        public bool IsPasswordUpdate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a value to Enable Moodle.
        /// </summary>
        public bool EnableMoodle { get; set; }
    }
}
