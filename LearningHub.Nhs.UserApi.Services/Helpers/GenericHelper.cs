namespace LearningHub.Nhs.UserApi.Services.Helpers
{
    using System;

    /// <summary>
    /// The generic helper.
    /// </summary>
    public static class GenericHelper
    {
        /// <summary>
        /// The get password timeout string.
        /// </summary>
        /// <param name="minutes">
        /// The number of minutes.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetPasswordTimeoutString(int minutes)
        {
            var time = new TimeSpan(0, minutes, 0);
            if (minutes < 1)
            {
                return "Expired";
            }

            if (time.Days == 0 && time.Hours == 0 && time.Minutes > 0)
            {
                return minutes + " minutes";
            }

            if (time.Days == 0 && time.Hours > 0 && time.Minutes > 0)
            {
                return string.Format("{0} hours and {1} minutes", time.Hours, time.Minutes);
            }

            if (time.Days == 0 && time.Hours > 0 && time.Minutes == 0)
            {
                return string.Format("{0} hours", time.Hours);
            }

            if (time.Days == 1)
            {
                return string.Format("{0} day", time.Days);
            }

            if (time.Days > 1 && time.Days < 7)
            {
                return string.Format("{0:F1} days", time.TotalDays);
            }

            // if (time.Days > 6)
            return string.Format("{0:F1} week(s)", time.Days / 7);
        }
    }
}
