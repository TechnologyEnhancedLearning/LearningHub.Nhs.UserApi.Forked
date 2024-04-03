namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Automapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using EntityFrameworkCore.Testing.Common;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;
    using MockQueryable.Moq;
    using Moq;
    using Xunit;

    /// <summary>
    /// The login wizard service tests.
    /// </summary>
    public class LoginWizardServiceTests
    {
        /// <summary>
        /// The start wizard for user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task StartWizardForUser()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();

            userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
                                        .Returns(Task.FromResult(
                                            new User()
                                            {
                                                Id = 1,
                                                UserName = "user.name1",
                                                FirstName = "Firstname1",
                                                LastName = "Lastname1",
                                                EmailAddress = "user.name1@test.com",
                                                Active = true,
                                                LoginTimes = 0,
                                                PasswordLifeCounter = 0,
                                                SecurityLifeCounter = 0,
                                            }));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, this.NewMapper());
            await loginWizardService.StartWizardForUser(userId);

            userRepositoryMock.Verify(ur => ur.UpdateAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Once);
        }

        /// <summary>
        /// The complete wizard for user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task CompleteWizardForUser()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var userHistoryServiceMock = new Mock<IUserHistoryService>();

            userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
                                        .Returns(Task.FromResult(
                                            new User()
                                            {
                                                Id = 1,
                                                UserName = "user.name1",
                                                FirstName = "Firstname1",
                                                LastName = "Lastname1",
                                                EmailAddress = "user.name1@test.com",
                                                Active = true,
                                                LoginTimes = 0,
                                                PasswordLifeCounter = 0,
                                                SecurityLifeCounter = 0,
                                            }));
            userHistoryServiceMock.Setup(r => r.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            new LearningHubValidationResult()
                                            {
                                                IsValid = true,
                                            }));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, null, null, null, null, null, null, null, userHistoryServiceMock.Object, null, null, this.NewMapper());
            await loginWizardService.CompleteWizardForUser(userId);

            userRepositoryMock.Verify(ur => ur.UpdateAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Once);
            userHistoryServiceMock.Verify(ur => ur.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// The create stage activity.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task CreateStageActivity()
        {
            int userId = 1;
            int stageId = 2;
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<LoginWizardStageActivity>()))
                                        .Returns(Task.FromResult(7));

            var loginWizardService = new LoginWizardService(null, loginWizardStageActivityRepositoryMock.Object, null, null, null, null, null, null, null, null, null, this.NewMapper());
            var returnVal = await loginWizardService.CreateStageActivity(userId, stageId);

            Assert.IsType<LearningHubValidationResult>(returnVal);
            Assert.Equal(7, returnVal.CreatedId);
        }

        /// <summary>
        /// The get login wizard by user id async_ no stages.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NoStages()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());

            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Empty(returnVal.LoginWizardStages);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ first logon terms and conditions.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_FirstLogonTermsAndConditions()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 2 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(1, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Terms and Conditions - first login", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon terms and conditions.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonTermsAndConditions()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 2 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(1, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Terms and Conditions Accepted", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon password reset.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonPasswordReset()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon(true)));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(2, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Password Reset Required", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ first logon security questions.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_FirstLogonSecurityQuestions()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject(1));
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(3, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Security Questions Completed - first login", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon security questions.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonSecurityQuestions()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject(1));
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(3, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Security Questions Completed", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ first logon job role.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_FirstLogonJobRole()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, null, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon(null)));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Job Role Specified - first login", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ student job role.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_StudentJobRole()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-92), DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon(false, 1001)));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Job Role Specified - Trainee / Students", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ first logon place of work.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_FirstLogonPlaceOfWork()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, null, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Place of Work Specified - first login", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon no place of work.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonNoPlaceOfWork()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, null, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Place of Work Specified - eLfH User", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ user upgrade to full user no place of work.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_UserUpgradeToFullUserPlaceOfWorkRequiredAfterLogon()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddHours(-1), DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());

            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Place of Work Specified - eLfH User", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ first logon personal details.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_FirstLogonPersonalDetails()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, null).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(6, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Personal Details Correct - first login", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon personal details consultant.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonPersonalDetailsConsultant()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-367)).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon(false, 1, 1, 175)));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(6, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Personal Details Correct - Consultants", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon personal details non consultant.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonPersonalDetailsNonConsultant()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now.AddDays(-92)).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(6, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Personal Details Correct - Non-Consultants", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon place of work inactive.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonPlaceOfWorkInactive()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon(false, 1, 1, 1, false)));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(5, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Place of work is inactive", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ not first logon job role bulk upload.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NotFirstLogonJobRoleBulkUpload()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, null, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Single(returnVal.LoginWizardStages);
            Assert.Single(returnVal.LoginWizardStages[0].LoginWizardRules);
            Assert.Equal(4, returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].LoginWizardStageId);
            Assert.Equal("Job role check after bulk upload location override", returnVal.LoginWizardStages[0].LoginWizardRules.ToList()[0].Description);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get login wizard by user id async_ no failures.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetLoginWizardByUserIdAsync_NoFailures()
        {
            int userId = 1;
            var userRepositoryMock = new Mock<IElfhUserRepository>();
            var loginWizardStageActivityRepositoryMock = new Mock<ILoginWizardStageActivityRepository>();
            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>();
            var termsAndConditionsRepositoryMock = new Mock<ITermsAndConditionsRepository>();
            var userSecurityQuestionRepositoryMock = new Mock<IUserSecurityQuestionRepository>();
            var loginWizardRuleRepositoryMock = new Mock<ILoginWizardRuleRepository>();
            var loginWizardStageRepositoryMock = new Mock<ILoginWizardStageRepository>();
            var userRoleUpgradeRepositoryMock = new Mock<IUserRoleUpgradeRepository>();

            loginWizardStageActivityRepositoryMock.Setup(r => r.GetByUser(It.IsAny<int>()))
                                        .Returns(this.GetLoginWizardStageActivity(DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now, DateTimeOffset.Now).AsQueryable());
            userEmploymentRepositoryMock.Setup(r => r.GetPrimaryForUser(It.IsAny<int>()))
                                        .Returns(Task.FromResult(GetUserEmploymentNotFirstLogon()));
            userRepositoryMock.Setup(r => r.IsEIntegrityUser(It.IsAny<int>())).Returns(false);
            userRepositoryMock.Setup(r => r.IsBasicUser(It.IsAny<int>())).Returns(false);
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAsync(It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            termsAndConditionsRepositoryMock.Setup(r => r.LatestVersionAcceptedAsync(It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns(Task.FromResult(new TermsAndConditions() { Id = 1 }));
            userSecurityQuestionRepositoryMock.Setup(r => r.GetByUserId(It.IsAny<int>()))
                                       .Returns(this.GetQueryableSecurityQuestionsMockObject());
            loginWizardRuleRepositoryMock.Setup(r => r.GetActive()).Returns(this.GetActiveRules());
            loginWizardStageRepositoryMock.Setup(r => r.GetAll()).Returns(this.GetLoginWizardStages().AsQueryable());
            userRoleUpgradeRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>())).Returns(new AsyncEnumerable<UserRoleUpgrade>(this.GetBlankUserRoleUpgradeMockData()));

            var loginWizardService = new LoginWizardService(userRepositoryMock.Object, loginWizardStageActivityRepositoryMock.Object, userEmploymentRepositoryMock.Object, termsAndConditionsRepositoryMock.Object, null, userSecurityQuestionRepositoryMock.Object, loginWizardRuleRepositoryMock.Object, loginWizardStageRepositoryMock.Object, null, userRoleUpgradeRepositoryMock.Object, this.GetSettings(), this.NewMapper());
            var returnVal = await loginWizardService.GetLoginWizardByUserIdAsync(userId);

            Assert.IsType<LoginWizardStagesViewModel>(returnVal);
            Assert.Empty(returnVal.LoginWizardStages);
            Assert.Empty(returnVal.LoginWizardStagesCompleted);
        }

        /// <summary>
        /// The get user employment first logon.
        /// </summary>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <returns>
        /// The <see cref="UserEmployment"/>.
        /// </returns>
        private static UserEmployment GetUserEmploymentFirstLogon(int? jobRoleId = 1)
        {
            return new UserEmployment()
            {
                Id = 1,
                UserId = 1,
                User = new User()
                {
                    Id = 1,
                    LastLoginWizardCompleted = null,
                    RestrictToSso = false,
                    MustChangeNextLogin = false,
                },
                JobRoleId = jobRoleId, // Student = 1001,1003,1327,1328
                LocationId = 1,
                GradeId = 1, // Consultant = 175
                Location = new Location()
                {
                    Id = 1,
                    Active = true,
                },
            };
        }

        /// <summary>
        /// The get user employment not first logon.
        /// </summary>
        /// <param name="mustChangeNextLogin">
        /// The must change next login.
        /// </param>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <param name="locationId">
        /// The location id.
        /// </param>
        /// <param name="gradeId">
        /// The grade id.
        /// </param>
        /// <param name="locationActive">
        /// The location active.
        /// </param>
        /// <returns>
        /// The <see cref="UserEmployment"/>.
        /// </returns>
        private static UserEmployment GetUserEmploymentNotFirstLogon(bool mustChangeNextLogin = false, int? jobRoleId = 1, int locationId = 1, int gradeId = 1, bool locationActive = true)
        {
            return new UserEmployment()
            {
                Id = 1,
                UserId = 1,
                User = new User()
                {
                    Id = 1,
                    LastLoginWizardCompleted = DateTimeOffset.Now,
                    RestrictToSso = false,
                    MustChangeNextLogin = mustChangeNextLogin,
                },
                JobRoleId = jobRoleId, // Student = 1001,1003,1327,1328
                LocationId = locationId,
                GradeId = gradeId, // Consultant = 175
                Location = new Location()
                {
                    Id = 1,
                    Active = locationActive,
                },
            };
        }

        /// <summary>
        /// The new mapper.
        /// </summary>
        /// <returns>
        /// The <see cref="IMapper"/>.
        /// </returns>
        private IMapper NewMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ElfhMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
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
                    SecurityQuestionsRequired = 2,
                });
        }

        /// <summary>
        /// The get login wizard stage activity.
        /// </summary>
        /// <param name="passwordReset">
        /// The password reset.
        /// </param>
        /// <param name="jobRole">
        /// The job role.
        /// </param>
        /// <param name="placeOfWork">
        /// The place of work.
        /// </param>
        /// <param name="personalDetails">
        /// The personal details.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<LoginWizardStageActivity> GetLoginWizardStageActivity(DateTimeOffset? passwordReset, DateTimeOffset? jobRole, DateTimeOffset? placeOfWork, DateTimeOffset? personalDetails)
        {
            var activity = new List<LoginWizardStageActivity>();
            if (passwordReset != null)
            {
                activity.Add(new LoginWizardStageActivity()
                {
                    Id = 3000001,
                    LoginWizardStageId = 2, // Password Reset
                    UserId = 1,
                    ActivityDatetime = (DateTimeOffset)passwordReset, // DateTimeOffset.Parse("2019-11-28T11:03:38.7130000Z")
                });
            }

            if (jobRole != null)
            {
                activity.Add(new LoginWizardStageActivity()
                {
                    Id = 3000002,
                    LoginWizardStageId = 4, // Job Details
                    UserId = 1,
                    ActivityDatetime = (DateTimeOffset)jobRole, // DateTimeOffset.Parse("2019-11-28T11:03:47.7130000Z")
                });
            }

            if (placeOfWork != null)
            {
                activity.Add(new LoginWizardStageActivity()
                {
                    Id = 3000003,
                    LoginWizardStageId = 5, // Place of work
                    UserId = 1,
                    ActivityDatetime = (DateTimeOffset)placeOfWork, // DateTimeOffset.Parse("2019-11-28T11:03:56.7130000Z")
                });
            }

            if (personalDetails != null)
            {
                activity.Add(new LoginWizardStageActivity()
                {
                    Id = 3000004,
                    LoginWizardStageId = 6, // Personal Details
                    UserId = 1,
                    ActivityDatetime = (DateTimeOffset)personalDetails, // DateTimeOffset.Parse("2019-11-28T11:05:12.7130000Z")
                });
            }

            return activity;
        }

        private IQueryable<UserSecurityQuestion> GetQueryableSecurityQuestionsMockObject(int numberOfQuestions = 2)
        {
            var securityQustionsQueryableMock = this.GetSecurityQuestions(numberOfQuestions).AsQueryable().BuildMock();

            return securityQustionsQueryableMock.Object;
        }

        /// <summary>
        /// The get security questions.
        /// </summary>
        /// <param name="numberOfQuestions">
        /// The number of questions.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<UserSecurityQuestion> GetSecurityQuestions(int numberOfQuestions = 2)
        {
            var questions = new List<UserSecurityQuestion>();

            for (int i = 0; i < numberOfQuestions; i++)
            {
                questions.Add(
                    new UserSecurityQuestion()
                    {
                        Id = i + 1,
                    });
            }

            return questions;
        }

        /// <summary>
        /// The get active rules.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<LoginWizardRule> GetActiveRules()
        {
            return new List<LoginWizardRule>()
            {
                new LoginWizardRule()
                {
                    Id = 1,
                    LoginWizardStageId = 1,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Terms and Conditions - first login",
                    ReasonDisplayText = "Please complete the following steps to ensure the initial set up of your account is fully complete.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 2,
                    LoginWizardStageId = 1,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Terms and Conditions Accepted",
                    ReasonDisplayText = "The e-LfH Hub terms and conditions have been updated. <strong>You are required to accept these new Terms and Conditions before you are able to continue to use the Hub.</strong>",
                    ActivationPeriod = 0,
                },
                new LoginWizardRule()
                {
                    Id = 4,
                    LoginWizardStageId = 2,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Password Reset Required",
                    ReasonDisplayText = string.Empty,
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 5,
                    LoginWizardStageId = 3,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Security Questions Completed - first login",
                    ReasonDisplayText = "Please complete the following steps to ensure the initial set up of your account is fully complete.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 6,
                    LoginWizardStageId = 3,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Security Questions Completed",
                    ReasonDisplayText = "Please configure your Security Questions. <br />Please check the details below and use the 'Save changes' button to record any changes you make.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 7,
                    LoginWizardStageId = 4,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Job Role Specified - first login",
                    ReasonDisplayText = "Please complete the following steps to ensure the initial set up of your account is fully complete.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 8,
                    LoginWizardStageId = 4,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Job Role Specified - Trainee / Students",
                    ReasonDisplayText = "You have a Trainee or Student related Job Role. <br />Please check the details below and use the 'Save changes' button to record any changes you make.",
                    ActivationPeriod = 90,
                },
                new LoginWizardRule()
                {
                    Id = 9,
                    LoginWizardStageId = 4,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Place of Work Specified - first login",
                    ReasonDisplayText = "Please complete the following steps to ensure the initial set up of your account is fully complete.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 10,
                    LoginWizardStageId = 4,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Place of Work Specified - eLfH User",
                    ReasonDisplayText = "You do not have a specific Place of Work stored against your profile. <br />Please check the details below and use the 'Save changes' button to record any changes you make.",
                    ActivationPeriod = 90,
                },
                new LoginWizardRule()
                {
                    Id = 11,
                    LoginWizardStageId = 6,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Personal Details Correct - first login",
                    ReasonDisplayText = "Please complete the following steps to ensure the initial set up of your account is fully complete.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 12,
                    LoginWizardStageId = 6,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Personal Details Correct - Consultants",
                    ReasonDisplayText = "You have not updated your Personal Details information for some time. <br />Please check the details below and use the 'Save changes' button to record any changes you make.",
                    ActivationPeriod = 365,
                },
                new LoginWizardRule()
                {
                    Id = 13,
                    LoginWizardStageId = 6,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Personal Details Correct - Non-Consultants",
                    ReasonDisplayText = "You have not updated your Personal Details information for some time. <br />Please check the details below and use the 'Save changes' button to record any changes you make.",
                    ActivationPeriod = 90,
                },
                new LoginWizardRule()
                {
                    Id = 14,
                    LoginWizardStageId = 7,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Technical Check - first login",
                    ReasonDisplayText = string.Empty,
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 15,
                    LoginWizardStageId = 5,
                    LoginWizardRuleCategoryId = 2,
                    Description = "Place of work is inactive",
                    ReasonDisplayText = "Please update your Place of Work as this location has been removed.<br />Update your Place of Work and then save your changes using the 'Save Changes' button.",
                    ActivationPeriod = null,
                },
                new LoginWizardRule()
                {
                    Id = 16,
                    LoginWizardStageId = 4,
                    LoginWizardRuleCategoryId = 3,
                    Description = "Job role check after bulk upload location override",
                    ReasonDisplayText = "Please complete the following steps to ensure your account is accurate.",
                    ActivationPeriod = 0,
                },
            };
        }

        /// <summary>
        /// The get login wizard stages.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<LoginWizardStage> GetLoginWizardStages()
        {
            return new List<LoginWizardStage>()
            {
                new LoginWizardStage()
                {
                    Id = 1,
                    Description = "Terms & Conditions and Privacy Notice for e-lfh.org.uk",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 2,
                    Description = "Password Reset",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 3,
                    Description = "Security Questions",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 4,
                    Description = "Job Role",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 5,
                    Description = "Place of Work",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 6,
                    Description = "Personal Details",
                    ReasonDisplayText = string.Empty,
                },
                new LoginWizardStage()
                {
                    Id = 7,
                    Description = "Technical Check",
                    ReasonDisplayText = string.Empty,
                },
            };
        }

        /// <summary>
        ///  get user role upgrade mock data.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        private IQueryable<UserRoleUpgrade> GetUserRoleUpgradeMockData()
        {
            return new List<UserRoleUpgrade>()
            {
                new UserRoleUpgrade()
                {
                    Id = 1,
                    UserId = 1,
                    EmailAddress = "test1@hee.nhs.uk",
                    UpgradeDate = DateTimeOffset.Now,
                    CreateDate = DateTimeOffset.Now,
                    AmendDate = DateTimeOffset.Now,
                },
            }.AsQueryable();
         }

        /// <summary>
        ///  get blank user role upgrade mock data.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        private IQueryable<UserRoleUpgrade> GetBlankUserRoleUpgradeMockData()
        {
            return new List<UserRoleUpgrade>().AsQueryable();
        }
    }
}
