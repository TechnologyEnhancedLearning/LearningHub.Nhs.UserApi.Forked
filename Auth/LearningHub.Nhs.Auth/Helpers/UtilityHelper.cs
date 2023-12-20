// <copyright file="UtilityHelper.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Helpers
{
    using System;
    using System.Text;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The utility helper.
    /// </summary>
    public static class UtilityHelper
    {
        /// <summary>
        /// The is active.
        /// </summary>
        /// <param name="htmlhelper">
        /// The htmlhelper.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string IsActive(this IHtmlHelper htmlhelper, string control, string action)
        {
            var routeData = htmlhelper?.ViewContext.RouteData;

            var routeAction = (string)routeData?.Values["action"];
            var routeControl = (string)routeData?.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : string.Empty;
        }

        /// <summary>
        /// The decode from base 64 to string.
        /// </summary>
        /// <param name="encodedString">
        /// The encoded string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string DecodeFromBase64ToString(this string encodedString)
        {
            var data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// The get string before or empty.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="stopAt">
        /// The stop at.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetStringBeforeOrEmpty(this string text, string stopAt = "/")
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var charLoc = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLoc > 0)
                {
                    return text.Substring(0, charLoc);
                }
            }

            return string.Empty;
        }
    }
}
