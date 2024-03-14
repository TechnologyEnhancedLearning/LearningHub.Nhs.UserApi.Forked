namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Automapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    /// <summary>
    /// The user service tests.
    /// </summary>
    public class UserServiceTests
    {
        /// <summary>
        /// The password mgr svc mock.
        /// </summary>
        private readonly Mock<IPasswordManagerService> passwordMgrSvcMock = new Mock<IPasswordManagerService>();

        /// <summary>
        /// The user attr repo mock.
        /// </summary>
        private readonly Mock<IUserAttributeRepository> userAttrRepoMock = new Mock<IUserAttributeRepository>();

        private readonly IOptions<Settings> elfhCacheSettingOptions = Options.Create(new Settings
        {
            ElfhCacheSettings = new ElfhCacheSettings
            {
                ElfhRedisKeyPrefix = "dev:",
                ElfhUserLoadByUserIdKey = "UserLoadByUserId_",
                ElfhUserLoadByUserNameKey = "UserLoadByUserName_",
            },
        });

        /////// <summary>
        /////// The get by id async_ valid.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetByIdAsync_Valid()
        ////{
        ////    int userId = 1;
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetByIdAsync(userId);

        ////    Assert.IsType<User>(user);
        ////    Assert.Equal(1, user.Id);
        ////    Assert.Equal("user.name1", user.UserName);
        ////}

        /////// <summary>
        /////// The get by id async_ not found.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetByIdAsync_NotFound()
        ////{
        ////    var userId = 999;
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(
        ////                                    Task.FromResult<User>(null));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetByIdAsync(userId);

        ////    Assert.Null(user);
        ////}

        /////// <summary>
        /////// The get basic profile by id async_ valid.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetBasicProfileByIdAsync_Valid()
        ////{
        ////    int userId = 1;
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetBasicProfileByIdAsync(userId);

        ////    Assert.IsType<UserBasic>(user);
        ////    Assert.Equal(1, user.Id);
        ////    Assert.Equal("user.name1", user.UserName);
        ////}

        /////// <summary>
        /////// The get basic profile by id async_ not found.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetBasicProfileByIdAsync_NotFound()
        ////{
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(
        ////                                    Task.FromResult<User>(null));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetBasicProfileByIdAsync(999);

        ////    Assert.Null(user);
        ////}

        /////// <summary>
        /////// The get by username async_ valid.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetByUsernameAsync_Valid()
        ////{
        ////    int userId = 2;
        ////    var username = "user.name2";
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetByUsernameAsync(username);

        ////    Assert.IsType<User>(user);
        ////    Assert.Equal(2, user.Id);
        ////    Assert.Equal("user.name2", user.UserName);
        ////}

        /////// <summary>
        /////// The get by username async_ not found.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task GetByUsernameAsync_NotFound()
        ////{
        ////    var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), null))
        ////                                .Returns(
        ////                                    Task.FromResult<User>(null));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var user = await userService.GetByUsernameAsync("user.name999");

        ////    Assert.Null(user);
        ////}

        /////// <summary>
        /////// The record successful signin async.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task RecordSuccessfulSigninAsync()
        ////{
        ////    int userId = 3;
        ////    var userRepositoryMock = new Mock<IUserRepository>();
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));
        ////    var elfhCacheMock = new Mock<IElfhRedisCache>();
        ////    elfhCacheMock.Setup(s => s.RemoveAsync(It.IsAny<string>(), CancellationToken.None));
        ////    var userHistoryMock = new Mock<IUserHistoryService>();
        ////    userHistoryMock.Setup(s => s.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        userHistoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.elfhCacheSettingOptions,
        ////        null,
        ////        elfhCacheMock.Object);
        ////    await userService.RecordSuccessfulSigninAsync(userId);

        ////    userRepositoryMock.Verify(ur => ur.UpdateAsync(3, It.IsAny<User>()), Times.Once);
        ////}

        /////// <summary>
        /////// The record un successful signin async.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task RecordUnSuccessfulSigninAsync()
        ////{
        ////    int userId = 3;
        ////    var userRepositoryMock = new Mock<IUserRepository>();
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>(MockBehavior.Strict);

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));
        ////    var elfhCacheMock = new Mock<IElfhRedisCache>();
        ////    elfhCacheMock.Setup(s => s.RemoveAsync(It.IsAny<string>(), CancellationToken.None));
        ////    var userHistoryMock = new Mock<IUserHistoryService>();
        ////    userHistoryMock.Setup(s => s.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        userHistoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.elfhCacheSettingOptions,
        ////        null,
        ////        elfhCacheMock.Object);

        ////    await userService.RecordUnsuccessfulSigninAsync(userId);

        ////    userRepositoryMock.Verify(ur => ur.UpdateAsync(3, It.IsAny<User>()), Times.Once);
        ////}

        /////// <summary>
        /////// The sync lh user async_ new user.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task SyncLHUserAsync_NewUser()
        ////{
        ////    var userId = 1;
        ////    var userRepositoryMock = new Mock<IUserRepository>();
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    var lhUserProfileRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserProfileRepository>();

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));
        ////    lhUserRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
        ////                                .Returns(Task.FromResult<LearningHub.Nhs.Models.Entities.User>(null));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        lhUserProfileRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    await userService.SyncLHUserAsync(userId);

        ////    lhUserRepositoryMock.Verify(ur => ur.CreateAsync(It.IsAny<int>(), It.IsAny<LearningHub.Nhs.Models.Entities.User>()), Times.Once);
        ////}

        /////// <summary>
        /////// The sync lh user async_ existing user.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task SyncLHUserAsync_ExistingUser()
        ////{
        ////    var userId = 1;
        ////    var userRepositoryMock = new Mock<IUserRepository>();
        ////    var lhUserRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    var lhUserProfileRepositoryMock = new Mock<LearningHub.Nhs.Repository.Interface.IUserProfileRepository>();

        ////    userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), null))
        ////                                .Returns(Task.FromResult(this.GetUser(userId)));
        ////    lhUserRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
        ////                                .Returns(Task.FromResult(this.GetLhUser(userId)));

        ////    var userService = new UserService(
        ////        userRepositoryMock.Object,
        ////        null,
        ////        lhUserRepositoryMock.Object,
        ////        lhUserProfileRepositoryMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        this.userAttrRepoMock.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    await userService.SyncLHUserAsync(userId);

        ////    lhUserRepositoryMock.Verify(ur => ur.UpdateAsync(It.IsAny<int>(), It.IsAny<LearningHub.Nhs.Models.Entities.User>()), Times.Once);
        ////}

        /// <summary>
        /// The get user role_ administrator.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetUserRole_Administrator()
        {
            var userGroupRepositoryMock = new Mock<IUserGroupRepository>();
            userGroupRepositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            new List<UserGroup>()
                                            {
                                                new UserGroup() { Id = 2, Name = "System Administrators" },
                                                new UserGroup() { Id = 1070, Name = "General Public" },
                                                new UserGroup() { Id = 81, Name = "Anaesthesia (ANA)" },
                                            }));

            var userService = new ElfhUserService(
                null,
                userGroupRepositoryMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var role = await userService.GetUserRoleAsync(1);

            Assert.Equal("Administrator", role);
        }

        /// <summary>
        /// The get user role_ blue user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetUserRole_BlueUser()
        {
            var userGroupRepositoryMock = new Mock<IUserGroupRepository>();
            userGroupRepositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            new List<UserGroup>()
                                            {
                                                new UserGroup() { Id = 1070, Name = "General Public" },
                                                new UserGroup() { Id = 81, Name = "Anaesthesia (ANA)" },
                                            }));

            var userService = new ElfhUserService(
                null,
                userGroupRepositoryMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var role = await userService.GetUserRoleAsync(1);

            Assert.Equal("BlueUser", role);
        }

        /////// <summary>
        /////// The create open athens user with basic info_ params blank.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task CreateOpenAthensUserWithBasicInfo_ParamsBlank()
        ////{
        ////    var usrRepo = new Mock<IUserRepository>();
        ////    var usrAttrRepo = new Mock<IUserAttributeRepository>();
        ////    var lhUsrRepo = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    usrRepo.Setup(s => s.CreateAsync(0, new User()))
        ////        .ReturnsAsync(411);
        ////    usrAttrRepo.Setup(s => s.LinkOpenAthensAccountToElfhUser(new OpenAthensToElfhUserLinkDetails()))
        ////        .ReturnsAsync(true);

        ////    var userSvc = new UserService(
        ////        usrRepo.Object,
        ////        null,
        ////        lhUsrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        this.passwordMgrSvcMock.Object,
        ////        usrAttrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var retVal = await userSvc.CreateOpenAthensUserWithBasicInfoAsync(new CreateOpenAthensLinkToLhUser() { LastName = string.Empty, EmailAddress = string.Empty, OaUserId = string.Empty, OaOrganisationId = string.Empty });

        ////    Assert.False(retVal.IsValid);
        ////    Assert.Contains("Email address is mandatory.", retVal.Details);
        ////    Assert.Contains("VerifyEmail is mandatory.", retVal.Details);
        ////    Assert.Contains("Last name is mandatory.", retVal.Details);
        ////}

        /////// <summary>
        /////// The create open athens user with basic info_ cannot create account.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task CreateOpenAthensUserWithBasicInfo_CannotCreateAccount()
        ////{
        ////    var usrRepo = new Mock<IUserRepository>();
        ////    var usrAttrRepo = new Mock<IUserAttributeRepository>();
        ////    var lhUsrRepo = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    var logger = new Mock<ILogger<UserService>>();
        ////    usrRepo.Setup(s => s.CreateAsync(0, new User()))
        ////        .ReturnsAsync(0);
        ////    usrAttrRepo.Setup(s => s.LinkOpenAthensAccountToElfhUser(new OpenAthensToElfhUserLinkDetails()))
        ////        .ReturnsAsync(true);

        ////    var userSvc = new UserService(
        ////        usrRepo.Object,
        ////        null,
        ////        lhUsrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        logger.Object,
        ////        this.passwordMgrSvcMock.Object,
        ////        usrAttrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var retVal = await userSvc.CreateOpenAthensUserWithBasicInfoAsync(new CreateOpenAthensLinkToLhUser() { LastName = "First", EmailAddress = "a876@nhs.net", VerifyEmail = "a876@nhs.net", OaUserId = "a78d98sd09", OaOrganisationId = "member@eng.nhs.net" });

        ////    Assert.False(retVal.IsValid);
        ////    Assert.Contains("OpenAthens user could not create an ELFH account.", retVal.Details);
        ////}

        /////// <summary>
        /////// The create open athens user with basic info_ cannot link account to oa attributes.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task CreateOpenAthensUserWithBasicInfo_CannotLinkAccountToOAAttributes()
        ////{
        ////    var usrRepo = new Mock<IUserRepository>();
        ////    var usrAttrRepo = new Mock<IUserAttributeRepository>();
        ////    var lhUsrRepo = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    var logger = new Mock<ILogger<UserService>>();
        ////    usrRepo.Setup(s => s.CreateAsync(0, It.IsAny<User>()))
        ////        .ReturnsAsync(5);
        ////    usrAttrRepo.Setup(s => s.LinkOpenAthensAccountToElfhUser(It.IsAny<OpenAthensToElfhUserLinkDetails>()))
        ////        .ReturnsAsync(false);

        ////    var userSvc = new UserService(
        ////        usrRepo.Object,
        ////        null,
        ////        lhUsrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        logger.Object,
        ////        this.passwordMgrSvcMock.Object,
        ////        usrAttrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null);

        ////    var retVal = await userSvc.CreateOpenAthensUserWithBasicInfoAsync(new CreateOpenAthensLinkToLhUser() { LastName = "First", EmailAddress = "a876@nhs.net", VerifyEmail = "a876@nhs.net", OaUserId = "a78d98sd09", OaOrganisationId = "member@eng.nhs.net" });
        ////    Assert.False(retVal.IsValid);
        ////    Assert.Contains("OpenAthens ELFH user linking to OpenAthens user attributes failure.", retVal.Details);
        ////}

        /////// <summary>
        /////// The create open athens user with basic info_ successfully return user id.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="Task"/>.
        /////// </returns>
        ////[Fact]
        ////public async Task CreateOpenAthensUserWithBasicInfo_SuccessfullyReturnUserId()
        ////{
        ////    var usrRepo = new Mock<IUserRepository>();
        ////    var usrAttrRepo = new Mock<IUserAttributeRepository>();
        ////    var usrHistoryService = new Mock<IUserHistoryService>();
        ////    var usrUserGroupRepository = new Mock<IUserUserGroupRepository>();
        ////    var usrEmploymentRepository = new Mock<IUserEmploymentRepository>();
        ////    var lhUsrRepo = new Mock<LearningHub.Nhs.Repository.Interface.IUserRepository>();
        ////    var logger = new Mock<ILogger<UserService>>();
        ////    var userId = 5;
        ////    usrRepo.Setup(s => s.CreateAsync(0, It.IsAny<User>()))
        ////        .ReturnsAsync(userId);
        ////    usrAttrRepo.Setup(s => s.LinkOpenAthensAccountToElfhUser(It.IsAny<OpenAthensToElfhUserLinkDetails>()))
        ////        .ReturnsAsync(true);
        ////    usrHistoryService.Setup(uhs => uhs.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()))
        ////        .ReturnsAsync(new LearningHubValidationResult() { IsValid = true });
        ////    usrUserGroupRepository.Setup(uhs => uhs.CreateAsync(It.IsAny<int>(), It.IsAny<UserUserGroup>()));

        ////    usrEmploymentRepository.Setup(uer => uer.GetPrimaryForUser(It.IsAny<int>())).Returns(Task.FromResult(new UserEmployment()));

        ////    var userSvc = new UserService(
        ////        usrRepo.Object,
        ////        null,
        ////        lhUsrRepo.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        logger.Object,
        ////        this.passwordMgrSvcMock.Object,
        ////        usrAttrRepo.Object,
        ////        usrHistoryService.Object,
        ////        usrUserGroupRepository.Object,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        null,
        ////        usrEmploymentRepository.Object,
        ////        null);

        ////    var retVal = await userSvc.CreateOpenAthensUserWithBasicInfoAsync(new CreateOpenAthensLinkToLhUser() { LastName = "First", EmailAddress = "a876@nhs.net", VerifyEmail = "a876@nhs.net", OaUserId = "a78d98sd09", OaOrganisationId = "member@eng.nhs.net" });

        ////    Assert.True(retVal.IsValid);
        ////    Assert.Equal(userId, retVal.CreatedId);
        ////}

        /// <summary>
        /// The get user role_none.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetUserRole_none()
        {
            var userGroupRepositoryMock = new Mock<IUserGroupRepository>();
            userGroupRepositoryMock.Setup(r => r.GetByUserAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            new List<UserGroup>()
                                            {
                                                new UserGroup() { Id = 1071, Name = "Guest User" },
                                                new UserGroup() { Id = 81, Name = "Anaesthesia (ANA)" },
                                            }));

            var userService = new ElfhUserService(
                null,
                userGroupRepositoryMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var role = await userService.GetUserRoleAsync(1);

            Assert.Equal("none", role);
        }

        /// <summary>
        /// The update user security questions_ create.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task UpdateUserSecurityQuestions_Create()
        {
            var userId = 1;
            List<UserSecurityQuestionViewModel> userSecurityQuestions = new List<UserSecurityQuestionViewModel>()
            {
                new UserSecurityQuestionViewModel() { Id = 1, UserId = userId, SecurityQuestionId = 1, SecurityQuestionAnswerHash = "********" },
                new UserSecurityQuestionViewModel(),
            };

            var userSecurityQuestionRepositoryyMock = new Mock<IUserSecurityQuestionRepository>();
            userSecurityQuestionRepositoryyMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()));

            var userService = new ElfhUserService(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                userSecurityQuestionRepositoryyMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                this.NewMapper(),
                null);

            await userService.UpdateUserSecurityQuestions(userSecurityQuestions, userId);

            userSecurityQuestionRepositoryyMock.Verify(ur => ur.CreateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()), Times.Once);
            userSecurityQuestionRepositoryyMock.Verify(ur => ur.UpdateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()), Times.Never);
        }

        /// <summary>
        /// The update user security questions_ create and update.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task UpdateUserSecurityQuestions_CreateAndUpdate()
        {
            var userId = 1;
            List<UserSecurityQuestionViewModel> userSecurityQuestions = new List<UserSecurityQuestionViewModel>()
            {
                new UserSecurityQuestionViewModel() { Id = 1, UserId = userId, SecurityQuestionId = 1, SecurityQuestionAnswerHash = "Test Answer" },
                new UserSecurityQuestionViewModel(),
            };

            var userSecurityQuestionRepositoryyMock = new Mock<IUserSecurityQuestionRepository>();
            userSecurityQuestionRepositoryyMock.Setup(r =>
                r.CreateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()));

            var userService = new ElfhUserService(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                userSecurityQuestionRepositoryyMock.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                this.NewMapper(),
                null);

            await userService.UpdateUserSecurityQuestions(userSecurityQuestions, userId);

            userSecurityQuestionRepositoryyMock.Verify(ur => ur.CreateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()), Times.Once);
            userSecurityQuestionRepositoryyMock.Verify(ur => ur.UpdateAsync(It.IsAny<int>(), It.IsAny<UserSecurityQuestion>()), Times.Once);
        }

        /// <summary>
        /// Test cache is invoked to invalidate ELFH user.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        [Fact]
        public async Task InvalidateElfhUserCache_InvalidateSuccess()
        {
            var elfhCacheMock = new Mock<IElfhRedisCache>();
            elfhCacheMock.Setup(s => s.RemoveAsync(It.IsAny<string>(), CancellationToken.None));

            var userService = new ElfhUserService(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                this.elfhCacheSettingOptions,
                elfhCacheMock.Object,
                this.NewMapper(),
                null);

            await userService.InvalidateElfhUserCacheAsync(123456, "test.user", CancellationToken.None);

            elfhCacheMock.Verify(v => v.RemoveAsync(It.IsAny<string>(), CancellationToken.None), Times.Exactly(2));
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        private User GetUser(int userId)
        {
            return this.TestUsers().FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// The get lh user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        private LearningHub.Nhs.Models.Entities.User GetLhUser(int userId)
        {
            return this.TestLHUsers().FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// The test users.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<User> TestUsers()
        {
            return new List<User>()
            {
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
                },
                new User()
                {
                    Id = 2,
                    UserName = "user.name2",
                    FirstName = "Firstname2",
                    LastName = "Lastname2",
                    EmailAddress = "user.name2@test.com",
                    Active = true,
                    LoginTimes = 0,
                    PasswordLifeCounter = 0,
                    SecurityLifeCounter = 0,
                },
                new User()
                {
                    Id = 3,
                    UserName = "user.name3",
                    FirstName = "Firstname3",
                    LastName = "Lastname3",
                    EmailAddress = "user.name3@test.com",
                    Active = true,
                    LoginTimes = 1,
                    PasswordLifeCounter = 2,
                    SecurityLifeCounter = 2,
                },
            };
        }

        /// <summary>
        /// The test lh users.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<LearningHub.Nhs.Models.Entities.User> TestLHUsers()
        {
            return new List<LearningHub.Nhs.Models.Entities.User>()
            {
                new LearningHub.Nhs.Models.Entities.User()
                {
                    Id = 1,
                    UserName = "user.name1",
                },
                new LearningHub.Nhs.Models.Entities.User()
                {
                    Id = 2,
                    UserName = "user.name2",
                },
                new LearningHub.Nhs.Models.Entities.User()
                {
                    Id = 3,
                    UserName = "user.name3",
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
    }
}
