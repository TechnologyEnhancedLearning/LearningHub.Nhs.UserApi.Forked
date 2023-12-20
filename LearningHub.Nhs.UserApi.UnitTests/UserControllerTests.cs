// <copyright file="UserControllerTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.UnitTests
{
    using System.Security.Claims;
    using LearningHub.Nhs.UserApi.Controllers;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    /// <summary>
    /// The user controller tests.
    /// </summary>
    public class UserControllerTests
    {
        /// <summary>
        /// The get user id by username_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetUserIdByUsername_Success()
        {
            var elfhUserServiceMock = new Mock<IElfhUserService>();

            elfhUserServiceMock.Setup(us => us.GetUserIdByUsernameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(1));

            var controller = new ElfhUserController(
               elfhUserServiceMock.Object,
               null,
               null,
               null,
               null,
               null);

            var response = await controller.GetUserIdByUsername("user.name");

            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            var userResult = okResult.Value;

            Assert.Equal(1, userResult);
        }

        /// <summary>
        /// The get by username_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByUsername_Success()
        {
            var elfhUserServiceMock = new Mock<IElfhUserService>();

            elfhUserServiceMock.Setup(us => us.GetByUsernameAsync(It.IsAny<string>(), null))
                .Returns(Task.FromResult(
                    new elfhHub.Nhs.Models.Entities.User() { Id = 1, UserName = "user.name", FirstName = "User" }));

            var controller = new ElfhUserController(
                elfhUserServiceMock.Object,
                null,
                null,
                null,
                null,
                null);

            var response = await controller.GetByUsername("user.name");

            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            var userResult = okResult.Value as elfhHub.Nhs.Models.Entities.User;

            Assert.Equal("User", userResult.FirstName);
        }

        /// <summary>
        /// The get basic by user id_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetBasicByUserId_Success()
        {
            var elfhUserServiceMock = new Mock<IElfhUserService>(MockBehavior.Strict);

            elfhUserServiceMock.Setup(us => us.GetBasicProfileByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(
                    new elfhHub.Nhs.Models.Common.UserBasic() { Id = 1, UserName = "user.name", FirstName = "User" }));

            var controller = new ElfhUserController(
                elfhUserServiceMock.Object,
                null,
                null,
                null,
                null,
                null);

            var response = await controller.GetBasicByUserId(1);

            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            var userResult = okResult.Value as elfhHub.Nhs.Models.Common.UserBasic;

            Assert.Equal("User", userResult.FirstName);
        }

        /// <summary>
        /// The get by user id_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByUserId_Success()
        {
            var elfhUserServiceMock = new Mock<IElfhUserService>(MockBehavior.Strict);

            elfhUserServiceMock.Setup(us => us.GetByIdAsync(It.IsAny<int>(), null))
                .Returns(Task.FromResult(
                    new elfhHub.Nhs.Models.Entities.User() { Id = 1, UserName = "user.name", FirstName = "User" }));

            var controller = new ElfhUserController(
                elfhUserServiceMock.Object,
                null,
                null,
                null,
                null,
                null);

            var response = await controller.GetByUserId(1);

            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            var userResult = okResult.Value as elfhHub.Nhs.Models.Entities.User;

            Assert.Equal("User", userResult.FirstName);
        }

        /// <summary>
        /// The get current user_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetCurrentUser_Success()
        {
            var elfhUserServiceMock = new Mock<IElfhUserService>(MockBehavior.Strict);

            elfhUserServiceMock.Setup(us => us.GetByIdAsync(It.IsAny<int>(), null))
                .Returns(Task.FromResult(
                    new elfhHub.Nhs.Models.Entities.User() { Id = 1, UserName = "user.name", FirstName = "User" }));

            var controller = new ElfhUserController(elfhUserServiceMock.Object, null, null, null, null, null);
            controller.ControllerContext = this.SetContollerContext();

            var controllerContextMock = new Mock<ControllerContext>();

            var response = await controller.GetCurrentUser();

            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            var userResult = okResult.Value as elfhHub.Nhs.Models.Entities.User;

            Assert.Equal("User", userResult.FirstName);
        }

        /// <summary>
        /// The set contoller context.
        /// </summary>
        /// <returns>
        /// The <see cref="ControllerContext"/>.
        /// </returns>
        private ControllerContext SetContollerContext()
        {
            IList<Claim> claimCollection = new List<Claim>
                    {
                        new Claim("given_name", "TestUser"),
                        new Claim("sub", "1"),
                    };

            var context = new ControllerContext();
            context.HttpContext = new DefaultHttpContext();
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claimCollection));

            return context;
        }
    }
}