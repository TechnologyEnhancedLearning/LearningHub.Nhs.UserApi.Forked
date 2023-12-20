// <copyright file="IPasswordManagerService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;

    /// <summary>
    /// The PasswordManagerService interface.
    /// </summary>
    public interface IPasswordManagerService : IComparer<string>
    {
        /// <summary>
        /// The generate.
        /// </summary>
        /// <param name="seed">
        /// The seed.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Generate(int seed = 0);

        /// <summary>
        /// The check.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Check(string password);

        /// <summary>
        /// The base 64 m d 5 hash digest.
        /// </summary>
        /// <param name="szString">
        /// The sz string.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Base64MD5HashDigest(string szString);
    }
}
