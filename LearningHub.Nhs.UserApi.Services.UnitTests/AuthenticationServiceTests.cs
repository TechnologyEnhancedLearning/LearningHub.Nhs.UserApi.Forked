// <copyright file="AuthenticationServiceTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;
    using Login = elfhHub.Nhs.Models.Common.Login;

    /// <summary>
    /// The authentication service tests.
    /// </summary>
    public class AuthenticationServiceTests
    {
        /// <summary>
        /// The password svc mock.
        /// </summary>
        private readonly Mock<IPasswordManagerService> passwordSvcMock = new Mock<IPasswordManagerService>();

        /// <summary>
        /// The authenticate async_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_Valid()
        {
            Login login = new Login()
            {
                Username = "test.user",
                Password = "testpassword#",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();
            var loggerServiceMock = new Mock<ILogger<AuthenticationService>>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<UserIncludeCollectionsEnum[]>()))
                            .Returns(Task.FromResult(
                                this.TestUser()));
            this.passwordSvcMock.Setup(pms => pms.Base64MD5HashDigest(It.IsAny<string>())).Returns("3deXavNvO8D5AeKLTOBAWA==");

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), loggerServiceMock.Object, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            userServiceMock.Verify(us => us.RecordSuccessfulSigninAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.True(loginResult.IsAuthenticated);
            Assert.Equal(string.Empty, loginResult.ErrorMessage);
        }

        /// <summary>
        /// The authenticate async_ invalid username.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_InvalidUsername()
        {
            Login login = new Login()
            {
                Username = "test.user.doesnotexist",
                Password = "testpassword#",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), null))
                                        .Returns(
                                            Task.FromResult<User>(null));

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), null, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.False(loginResult.IsAuthenticated);
            Assert.Equal("The username or password is incorrect", loginResult.ErrorMessage);
        }

        /// <summary>
        /// The authenticate async_ invalid password.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_InvalidPassword()
        {
            Login login = new Login()
            {
                Username = "test.user",
                Password = "invalidpassword",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();
            var loggerServiceMock = new Mock<ILogger<AuthenticationService>>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<UserIncludeCollectionsEnum[]>()))
                                        .Returns(Task.FromResult(
                                            this.TestUser()));

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), loggerServiceMock.Object, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            userServiceMock.Verify(us => us.RecordUnsuccessfulSigninAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.False(loginResult.IsAuthenticated);
            Assert.Equal("The username or password is incorrect", loginResult.ErrorMessage);
        }

        /// <summary>
        /// The authenticate async_ in active user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_InActiveUser()
        {
            Login login = new Login()
            {
                Username = "test.user",
                Password = "testpassword#",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();
            var loggerServiceMock = new Mock<ILogger<AuthenticationService>>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<UserIncludeCollectionsEnum[]>()))
                                        .Returns(Task.FromResult(
                                            this.TestUser(false)));

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), loggerServiceMock.Object, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            userServiceMock.Verify(us => us.RecordUnsuccessfulSigninAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.False(loginResult.IsAuthenticated);
            Assert.Equal("This account is locked. Please contact the support team if you need help.", loginResult.ErrorMessage);
        }

        /// <summary>
        /// The authenticate async_ password attempts exceeded.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_PasswordAttemptsExceeded()
        {
            Login login = new Login()
            {
                Username = "test.user",
                Password = "testpassword#",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();
            var loggerServiceMock = new Mock<ILogger<AuthenticationService>>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<UserIncludeCollectionsEnum[]>()))
                                        .Returns(Task.FromResult(
                                            this.TestUser(true, 3)));

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), loggerServiceMock.Object, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            userServiceMock.Verify(us => us.RecordUnsuccessfulSigninAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.False(loginResult.IsAuthenticated);
            Assert.Equal("This account is locked. Please contact the support team if you need help.", loginResult.ErrorMessage);
        }

        /// <summary>
        /// The authenticate async_ active date range invalid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task AuthenticateAsync_ActiveDateRangeInvalid()
        {
            Login login = new Login()
            {
                Username = "test.user",
                Password = "testpassword#",
                IsAuthenticated = false,
            };

            var userServiceMock = new Mock<IElfhUserService>();
            var loggerServiceMock = new Mock<ILogger<AuthenticationService>>();

            userServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<UserIncludeCollectionsEnum[]>()))
                                        .Returns(Task.FromResult(
                                            this.TestUser(true, 0, DateTimeOffset.Now.AddDays(-730), DateTimeOffset.Now.AddDays(-360))));

            var authenticationService = new AuthenticationService(userServiceMock.Object, this.GetSettings(), loggerServiceMock.Object, this.passwordSvcMock.Object);

            var loginResult = await authenticationService.AuthenticateAsync(login);

            userServiceMock.Verify(us => us.RecordUnsuccessfulSigninAsync(It.IsAny<int>(), CancellationToken.None), Times.Once);

            Assert.IsType<LoginResultInternal>(loginResult);
            Assert.False(loginResult.IsAuthenticated);
            Assert.Equal("This account is not active. Please contact the support team if you need help.", loginResult.ErrorMessage);
        }

        /// <summary>
        /// The test user.
        /// </summary>
        /// <param name="active">
        /// The active.
        /// </param>
        /// <param name="passwordLifeCounter">
        /// The password life counter.
        /// </param>
        /// <param name="activeFromDate">
        /// The active from date.
        /// </param>
        /// <param name="activeToDate">
        /// The active to date.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        private User TestUser(bool active = true, int passwordLifeCounter = 0, DateTimeOffset? activeFromDate = null, DateTimeOffset? activeToDate = null)
        {
            return new User()
            {
                Id = 1,
                UserName = "test.user",
                PasswordHash = "3deXavNvO8D5AeKLTOBAWA==", // Hash of "testpassword#"
                Active = active,
                PasswordLifeCounter = passwordLifeCounter,
                ActiveFromDate = activeFromDate,
                ActiveToDate = activeToDate,
            };
        }

        private IOptions<Settings> GetSettings()
        {
            return Options.Create(
                new Settings()
                {
                    MaxLogonAttempts = 3,
                });
        }
    }
}