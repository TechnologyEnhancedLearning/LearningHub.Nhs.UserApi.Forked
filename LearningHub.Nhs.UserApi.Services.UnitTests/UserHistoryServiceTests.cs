// <copyright file="UserHistoryServiceTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    /// <summary>
    /// The user history service tests.
    /// </summary>
    public class UserHistoryServiceTests
    {
        /// <summary>
        /// The get by id async_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByIdAsync_Valid()
        {
            int userHistoryId = 1;
            var userHistoryRepositoryMock = new Mock<IUserHistoryRepository>(MockBehavior.Strict);

            userHistoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            new UserHistory()
                                            {
                                                Id = 1,
                                                UserId = 100,
                                                UserHistoryTypeId = 1,
                                            }));

            var optionsMock = new Mock<IOptions<Settings>>();

            var userHistoryService = new UserHistoryService(userHistoryRepositoryMock.Object,  optionsMock.Object, null);

            var userHistory = await userHistoryService.GetByIdAsync(userHistoryId);

            Assert.IsType<UserHistory>(userHistory);
            Assert.Equal(1, userHistory.Id);
        }

        /// <summary>
        /// The get by id async_ not found.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByIdAsync_NotFound()
        {
            var userHistoryId = 999;
            var userHistoryRepositoryMock = new Mock<IUserHistoryRepository>(MockBehavior.Strict);

            userHistoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(
                                            Task.FromResult<UserHistory>(null));

            var optionsMock = new Mock<IOptions<Settings>>();

            var userHistoryService = new UserHistoryService(userHistoryRepositoryMock.Object, optionsMock.Object, null);

            var user = await userHistoryService.GetByIdAsync(userHistoryId);

            Assert.Null(user);
        }

        /// <summary>
        /// The create async_ success 1.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task CreateAsync_Success1()
        {
            var userHistoryRepositoryMock = new Mock<IUserHistoryRepository>(MockBehavior.Strict);

            userHistoryRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UserHistoryViewModel>()))
                                        .Returns(Task.CompletedTask);

            var userHistoryService = new UserHistoryService(userHistoryRepositoryMock.Object, this.GetSettings(), null);

            var learningHubValidationResult = await userHistoryService.CreateAsync(
                    new UserHistoryViewModel()
                    {
                        UserId = 100,
                        UserHistoryTypeId = 0,
                        Detail = "Test Detail",
                        UserAgent = "Test UserAgent",
                    });

            Assert.IsType<LearningHubValidationResult>(learningHubValidationResult);
            Assert.True(learningHubValidationResult.IsValid);

            userHistoryRepositoryMock.Verify(ur => ur.CreateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UserHistoryViewModel>()), Times.Once);
        }

        /// <summary>
        /// The create async_ success 2.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task CreateAsync_Success2()
        {
            var userHistoryRepositoryMock = new Mock<IUserHistoryRepository>(MockBehavior.Strict);

            userHistoryRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UserHistoryViewModel>()))
                                         .Returns(Task.CompletedTask);

            var userHistoryService = new UserHistoryService(userHistoryRepositoryMock.Object, this.GetSettings(), null);

            var learningHubValidationResult = await userHistoryService.CreateAsync(
                    new UserHistoryViewModel()
                    {
                        UserId = 100,
                        UserHistoryTypeId = 0,
                        Detail = "Test Detail",
                        UserAgent = "Test UserAgent",
                        BrowserName = "Test Browser Name",
                        BrowserVersion = "Test Browser Version",
                        UrlReferer = "Test UrlReferer",
                        LoginSuccessFul = true,
                        LoginIP = "Test IP",
                    });

            Assert.IsType<LearningHubValidationResult>(learningHubValidationResult);
            Assert.True(learningHubValidationResult.IsValid);

            userHistoryRepositoryMock.Verify(ur => ur.CreateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<UserHistoryViewModel>()), Times.Once);
        }

        /// <summary>
        /// The get settings.
        /// </summary>
        /// <returns>
        /// The <see cref="IOptions"/>.
        /// </returns>
        private IOptions<Settings> GetSettings()
        {
            return Options.Create(
                new Settings()
                {
                    LearningHubTenantId = 10,
                });
        }
    }
}
