// <copyright file="OpenAthensControllerTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Tests
{
    using System;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Controllers;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    /// <summary>
    /// The open athens controller tests.
    /// </summary>
    public class OpenAthensControllerTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private OpenAthensController controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAthensControllerTests"/> class.
        /// </summary>
        public OpenAthensControllerTests()
        {
            this.controller = new OpenAthensController(
                Options.Create(new OpenAthensLearningHubClientDictionary()), null, null, null, null, null, null, null);
        }

        /// <summary>
        /// The login returns exception if client id is empty.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task LoginReturnsExceptionIfClientIdIsEmpty()
        {
            var ex = await Assert.ThrowsAsync<Exception>(() => this.controller.Login(string.Empty, "sdfg", string.Empty));

            Assert.Equal("ClientId or origin are empty.", ex.Message);
        }

        /// <summary>
        /// The login returns exception if origin empty.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task LoginReturnsExceptionIfOriginEmpty()
        {
            var ex = await Assert.ThrowsAsync<Exception>(() => this.controller.Login("lkjh", string.Empty, string.Empty));

            Assert.Equal("ClientId or origin are empty.", ex.Message);
        }

        /// <summary>
        /// The login returns exception if no oa client is found.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task LoginReturnsExceptionIfNoOaClientIsFound()
        {
            var ex = await Assert.ThrowsAsync<Exception>(() => this.controller.Login("kljhgkj", "lhuklj", string.Empty));

            Assert.Equal("No OA LH client found.", ex.Message);
        }

        /// <summary>
        /// The login returns exception if origin does not match config.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task LoginReturnsExceptionIfOriginDoesNotMatchConfig()
        {
            this.controller = new OpenAthensController(
                Options.Create(new OpenAthensLearningHubClientDictionary { { "client1", "value1" } }), null, null, null, null, null, null, null);

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                this.controller.Login("client1", "value2", string.Empty));

            Assert.Equal("Invalid origin", ex.Message);
        }

        /// <summary>
        /// The login returns task when challenging request.
        /// </summary>
        [Fact]
        public void LoginReturnsTaskWhenChallengingRequest()
        {
            var asMock = new Mock<IAuthenticationService>();
            asMock.Setup(
                s => s.ChallengeAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<AuthenticationProperties>())).Returns(Task.FromResult((object)null));

            var spMock = new Mock<IServiceProvider>();
            spMock.Setup(s => s.GetService(typeof(IAuthenticationService))).Returns(asMock.Object);

            this.controller =
                new OpenAthensController(
                    Options.Create(new OpenAthensLearningHubClientDictionary
                                       {
                                           { "clientId", "value1" },
                                       }),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            RequestServices = spMock.Object,
                        },
                    },
                };

            var theTask = this.controller.Login("clientId", "https://value1", string.Empty);
            theTask.Wait();

            Assert.True(theTask.IsCompletedSuccessfully);
        }
    }
}
