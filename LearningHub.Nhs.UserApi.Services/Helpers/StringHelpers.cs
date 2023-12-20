// <copyright file="StringHelpers.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Helpers
{
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// The string helpers.
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// The Strip unicode characters from string method.
        /// </summary>
        /// <param name="inputValue">
        /// The input string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string StripUnicodeCharactersFromString(this string inputValue)
        {
            return new string(inputValue.Where(c => c <= sbyte.MaxValue).ToArray());
        }

        /// <summary>
        /// Converts a string to Proper Case by first toLower() then ToTileCase(), and handles troublesome/empty strings more gracefully
        /// - doesn't handle properly: abbreviations, van der ..., McDonald, O'Brien, bracketed or hyphened words.  But what does?.
        /// </summary>
        /// <param name="aString">string to be converted.</param>
        /// <returns>string in Proper Case.</returns>
        public static string ToTitleCase(this string aString)
        {
            try
            {
                return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(aString.ToLower());
            }
            catch
            {
                return aString;
            }
        }
    }
}