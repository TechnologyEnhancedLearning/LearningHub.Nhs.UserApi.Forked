// <copyright file="PasswordManagerServiceTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;

    /// <summary>
    /// The password manager service tests.
    /// </summary>
    public class PasswordManagerServiceTests
    {
        /// <summary>
        /// The password generate random password default strategy.
        /// </summary>
        [Fact]
        public void PasswordGenerateRandomPasswordDefaultStrategy()
        {
            var pwSvc = new PasswordManagerService();

            var pwd = pwSvc.Generate();

            Assert.NotNull(pwd);
            Assert.NotEmpty(pwd);
            Assert.InRange(pwd.Length, 8, 256);
        }

        /// <summary>
        /// The password generate random password and passes check.
        /// </summary>
        [Fact]
        public void PasswordGenerateRandomPasswordAndPassesCheck()
        {
            var pwSvc = new PasswordManagerService();

            var pwd = pwSvc.Generate();

            Assert.True(pwSvc.Check(pwd));
        }

        /// <summary>
        /// The password generate random password and fails check.
        /// </summary>
        [Fact]
        public void PasswordGenerateRandomPasswordAndFailsCheck()
        {
            var pwSvc = new PasswordManagerService();

            var pwd = "pppp"; // Fails default strategy

            Assert.False(pwSvc.Check(pwd));
        }
    }
}
