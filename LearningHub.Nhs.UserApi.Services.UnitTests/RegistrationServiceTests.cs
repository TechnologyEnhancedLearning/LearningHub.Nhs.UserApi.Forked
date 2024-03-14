namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Services.Models;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    /// <summary>
    /// The registration service tests.
    /// </summary>
    public class RegistrationServiceTests
    {
        private Mock<IIpCountryLookupRepository> countryLookupRepoMock;
        private string ipAddress = "127.0.0.1";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationServiceTests"/> class.
        /// </summary>
        public RegistrationServiceTests()
        {
            this.countryLookupRepoMock = new Mock<IIpCountryLookupRepository>();
            this.countryLookupRepoMock.Setup(t => t.IsUKIpAddress(It.IsAny<long>())).ReturnsAsync(true);
        }

        /// <summary>
        /// The get registration status_ existing email_ blue user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetRegistrationStatus_ExistingEmail_BlueUser()
        {
            var userRepositoryMock = new Mock<IElfhUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(r => r.DoesEmailExistAsync(It.IsAny<string>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUsersForEmail(It.IsAny<string>())).Returns(new User[0].AsQueryable());

            var userGroupTypeInputValidationRepositoryMock = new Mock<IUserGroupTypeInputValidationRepository>(MockBehavior.Strict);
            userGroupTypeInputValidationRepositoryMock.Setup(r => r.IsEmailValidForUserGroup(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(userRepositoryMock.Object, userGroupTypeInputValidationRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var status = await registrationService.GetRegistrationStatus("test@here.com", this.ipAddress);

            Assert.IsType<EmailRegistrationStatus>(status);
            Assert.Equal(EmailRegistrationStatus.ExistingUserIsEligible, status);
        }

        /// <summary>
        /// The get registration status_ existing email_ non blue user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetRegistrationStatus_ExistingEmail_NonBlueUser()
        {
            var userRepositoryMock = new Mock<IElfhUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(r => r.DoesEmailExistAsync(It.IsAny<string>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUsersForEmail(It.IsAny<string>())).Returns(new User[] { new User() { Id = 1 } }.AsQueryable());

            var userGroupRepositoryMock = new Mock<IUserGroupRepository>(MockBehavior.Strict);
            userGroupRepositoryMock.Setup(uug => uug.GetByUserAsync(1)).Returns(Task.FromResult(new List<UserGroup>() { new UserGroup() { Id = 1071 } }));

            var userGroupTypeInputValidationRepositoryMock = new Mock<IUserGroupTypeInputValidationRepository>(MockBehavior.Strict);
            userGroupTypeInputValidationRepositoryMock.Setup(r => r.IsEmailValidForUserGroup(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(userRepositoryMock.Object, userGroupTypeInputValidationRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, userGroupRepositoryMock.Object, null, null, null, this.countryLookupRepoMock.Object);
            var status = await registrationService.GetRegistrationStatus("test@here.com", this.ipAddress);

            Assert.IsType<EmailRegistrationStatus>(status);
            Assert.Equal(EmailRegistrationStatus.ExistingUserNotEligible, status);
        }

        /// <summary>
        /// The get registration status_ non existing email_ blue user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetRegistrationStatus_NonExistingEmail_BlueUser()
        {
            var userRepositoryMock = new Mock<IElfhUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(r => r.DoesEmailExistAsync(It.IsAny<string>())).ReturnsAsync(false);

            var userGroupTypeInputValidationRepositoryMock = new Mock<IUserGroupTypeInputValidationRepository>(MockBehavior.Strict);
            userGroupTypeInputValidationRepositoryMock.Setup(r => r.IsEmailValidForUserGroup(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(userRepositoryMock.Object, userGroupTypeInputValidationRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var status = await registrationService.GetRegistrationStatus("test@here.com", this.ipAddress);

            Assert.IsType<EmailRegistrationStatus>(status);
            Assert.Equal(EmailRegistrationStatus.NewUserIsEligible, status);
        }

        /// <summary>
        /// The get registration status_ non existing email_ non blue user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetRegistrationStatus_NonExistingEmail_NonBlueUser()
        {
            var userRepositoryMock = new Mock<IElfhUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(r => r.DoesEmailExistAsync(It.IsAny<string>())).ReturnsAsync(false);

            var userGroupTypeInputValidationRepositoryMock = new Mock<IUserGroupTypeInputValidationRepository>(MockBehavior.Strict);
            userGroupTypeInputValidationRepositoryMock.Setup(r => r.IsEmailValidForUserGroup(It.IsAny<string>(), It.IsAny<int>())).Returns(false);

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(userRepositoryMock.Object, userGroupTypeInputValidationRepositoryMock.Object, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var status = await registrationService.GetRegistrationStatus("test@here.com", this.ipAddress);

            Assert.IsType<EmailRegistrationStatus>(status);
            Assert.Equal(EmailRegistrationStatus.NewGeneralUserIsEligible, status);
        }

        /// <summary>
        /// The register user_ invalid_ no email or names.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_Invalid_NoEmailOrNames()
        {
            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    EmailAddress = string.Empty,
                    CountryId = 1,
                    RegionId = 3,
                    JobRoleId = 1,
                    SpecialtyId = 1,
                    LocationId = 1001,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Email address is mandatory.", validationResult.Details);
            Assert.Contains("First name is mandatory.", validationResult.Details);
            Assert.Contains("Last name is mandatory.", validationResult.Details);
        }

        /// <summary>
        /// The register user_ missing region for england.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_MissingRegionForEngland()
        {
            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 0,
                    JobRoleId = 1,
                    SpecialtyId = 1,
                    LocationId = 1001,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Region is a required field for England", validationResult.Details);
        }

        /// <summary>
        /// The register user_ missing country job role specialty and location.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_MissingCountryJobRoleSpecialtyAndLocation()
        {
            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, null, null, null, null, null, null, null, null, null, null, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 0,
                    RegionId = 0,
                    JobRoleId = 0,
                    SpecialtyId = 0,
                    LocationId = 0,
                    GradeId = 0,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Job role is a required field", validationResult.Details);
            Assert.Contains("Specialty is a required field", validationResult.Details);
            Assert.Contains("Place of work is a required field", validationResult.Details);
            Assert.Contains("Grade is a required field", validationResult.Details);
        }

        /// <summary>
        /// The register user_ gmc failure.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_GMCFailure()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);
            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(
                                                new JobRole()
                                                {
                                                    Id = 1,
                                                    MedicalCouncilId = 1, // GMC
                                                }));
            var medicalcouncilServiceMock = new Mock<IMedicalCouncilService>(MockBehavior.Strict);
            medicalcouncilServiceMock.Setup(r => r.ValidateGMCNumber(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>())).Returns(Task.FromResult(
                                                "Test GMC error"));

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, jobRoleRepositoryMock.Object, null, null, null, null, null, null, null, null, medicalcouncilServiceMock.Object, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 2,
                    JobRoleId = 3,
                    SpecialtyId = 4,
                    LocationId = 5,
                    GradeId = 6,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Test GMC error", validationResult.Details);
        }

        /// <summary>
        /// The register user_ gdc failure.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_GDCFailure()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);
            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(
                                                new JobRole()
                                                {
                                                    Id = 1,
                                                    MedicalCouncilId = 3, // GDC
                                                }));
            var medicalcouncilServiceMock = new Mock<IMedicalCouncilService>(MockBehavior.Strict);
            medicalcouncilServiceMock.Setup(r => r.ValidateGDCNumber(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(
                                                "Test GDC error"));

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, jobRoleRepositoryMock.Object, null, null, null, null, null, null, null, null, medicalcouncilServiceMock.Object, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 2,
                    JobRoleId = 3,
                    SpecialtyId = 4,
                    LocationId = 5,
                    GradeId = 6,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Test GDC error", validationResult.Details);
        }

        /// <summary>
        /// The register user_ nmc failure.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_NMCFailure()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);
            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(
                                                new JobRole()
                                                {
                                                    Id = 1,
                                                    MedicalCouncilId = 2, // NMC
                                                }));
            var medicalcouncilServiceMock = new Mock<IMedicalCouncilService>(MockBehavior.Strict);
            medicalcouncilServiceMock.Setup(r => r.ValidateNMCNumber(It.IsAny<string>())).Returns(
                                                "Test NMC error");

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, jobRoleRepositoryMock.Object, null, null, null, null, null, null, null, null, medicalcouncilServiceMock.Object, null, null, optionsMock.Object, null, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 2,
                    JobRoleId = 3,
                    SpecialtyId = 4,
                    LocationId = 5,
                    GradeId = 6,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("Test NMC error", validationResult.Details);
        }

        /// <summary>
        /// The register user_ user already exists.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_UserAlreadyExists()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);
            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(
                                                new JobRole()
                                                {
                                                    Id = 1,
                                                    MedicalCouncilId = 0,
                                                }));
            var userServiceMock = new Mock<IElfhUserService>(MockBehavior.Strict);
            userServiceMock.Setup(r => r.UserAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(
                                                true));

            var medicalCouncilRepositoryMock = new Mock<IMedicalCouncilRepository>(MockBehavior.Strict);
            medicalCouncilRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new MedicalCouncil() { Code = "GMC" });

            var optionsMock = new Mock<IOptions<Settings>>();

            var registrationService = new RegistrationService(null, null, jobRoleRepositoryMock.Object, null, null, null, null, null, null, null, null, null, userServiceMock.Object, null, optionsMock.Object, null, medicalCouncilRepositoryMock.Object, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 2,
                    JobRoleId = 3,
                    SpecialtyId = 4,
                    LocationId = 5,
                    GradeId = 6,
                });

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Contains("This GMC number you entered is already registered to an existing account.", validationResult.Details);
        }

        /// <summary>
        /// The register user_ success.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task RegisterUser_Success()
        {
            var optionsMock = new Mock<IOptions<Settings>>();

            var userGroupTypeInputValidationRepositoryMock = new Mock<IUserGroupTypeInputValidationRepository>(MockBehavior.Strict);
            userGroupTypeInputValidationRepositoryMock.Setup(r => r.IsEmailValidForUserGroup(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);
            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new JobRole() { Id = 1, MedicalCouncilId = 0 }));

            var userServiceMock = new Mock<IElfhUserService>(MockBehavior.Strict);
            userServiceMock.Setup(r => r.UserAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            userServiceMock.Setup(r => r.CreateUserNameAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult("UserName1"));

            var userRepositoryMock = new Mock<IElfhUserRepository>();
            userRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<User>())).Returns(Task.FromResult(1));
            userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<User>()));

            var systemSettingsRepositoryMock = new Mock<ISystemSettingRepository>(MockBehavior.Strict);
            systemSettingsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new SystemSetting() { IntValue = 120 }));

            var userPasswordValidationTokenRepositoryMock = new Mock<IUserPasswordValidationTokenRepository>();
            userPasswordValidationTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<UserPasswordValidationTokenExtended>()));

            var emailTemplateRepositoryMock = new Mock<IEmailTemplateRepository>(MockBehavior.Strict);
            emailTemplateRepositoryMock.Setup(r => r.GetByTypeAndTenantAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new EmailTemplate() { Id = 1, Body = string.Empty }));

            var userEmploymentRepositoryMock = new Mock<IUserEmploymentRepository>(MockBehavior.Strict);
            userEmploymentRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<UserEmployment>())).Returns(Task.FromResult(1));

            var userHistoryServiceMock = new Mock<IUserHistoryService>(MockBehavior.Strict);
            userHistoryServiceMock.Setup(r => r.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>())).Returns(Task.FromResult(new LearningHubValidationResult() { IsValid = true }));

            var loginWizardServiceMock = new Mock<ILoginWizardService>(MockBehavior.Strict);
            loginWizardServiceMock.Setup(r => r.CreateStageActivity(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new LearningHubValidationResult() { IsValid = true }));

            var userUserGroupRepositoryMock = new Mock<IUserUserGroupRepository>();
            userUserGroupRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<UserUserGroup>()));

            var tenantSmtpRepositoryMock = new Mock<ITenantSmtpRepository>(MockBehavior.Strict);
            tenantSmtpRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new TenantSmtp() { From = "from@there.com" }));

            var tenantRepositoryMock = new Mock<ITenantRepository>(MockBehavior.Strict);
            tenantRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.FromResult(new Tenant() { QuickStartGuideUrl = "https://www.QuickStartGuideUrl.com", SupportFormUrl = "https://www.SupportFormUrl.com" }));

            var emailLogRepositoryMock = new Mock<IEmailLogRepository>();
            emailLogRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<int>(), It.IsAny<EmailLog>()));

            var registrationService = new RegistrationService(userRepositoryMock.Object, userGroupTypeInputValidationRepositoryMock.Object, jobRoleRepositoryMock.Object, userUserGroupRepositoryMock.Object, userEmploymentRepositoryMock.Object, emailTemplateRepositoryMock.Object, emailLogRepositoryMock.Object, systemSettingsRepositoryMock.Object, userPasswordValidationTokenRepositoryMock.Object, tenantRepositoryMock.Object, tenantSmtpRepositoryMock.Object, null, userServiceMock.Object, userHistoryServiceMock.Object, this.GetSettings(), loginWizardServiceMock.Object, null, null, null, null, null, this.countryLookupRepoMock.Object);
            var validationResult = await registrationService.RegisterUser(
                new RegistrationRequestViewModel()
                {
                    FirstName = "Name1",
                    LastName = "Name2",
                    EmailAddress = "test@here.com",
                    CountryId = 1,
                    RegionId = 2,
                    JobRoleId = 3,
                    SpecialtyId = 4,
                    LocationId = 5,
                    SelfRegistration = true,
                    GradeId = 6,
                });

            userRepositoryMock.Verify(us => us.CreateAsync(It.IsAny<int>(), It.IsAny<User>()), Times.Once);
            userEmploymentRepositoryMock.Verify(us => us.CreateAsync(It.IsAny<int>(), It.IsAny<UserEmployment>()), Times.Once);
            userHistoryServiceMock.Verify(us => us.CreateAsync(It.IsAny<UserHistoryViewModel>(), It.IsAny<int>()), Times.Once);
            userUserGroupRepositoryMock.Verify(us => us.CreateAsync(It.IsAny<int>(), It.IsAny<UserUserGroup>()), Times.Once);
            userPasswordValidationTokenRepositoryMock.Verify(us => us.CreateAsync(It.IsAny<int>(), It.IsAny<UserPasswordValidationTokenExtended>()), Times.Once);
            emailLogRepositoryMock.Verify(us => us.CreateAsync(It.IsAny<int>(), It.IsAny<EmailLog>()), Times.Once);
            loginWizardServiceMock.Verify(us => us.CreateStageActivity(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));

            Assert.IsType<LearningHubValidationResult>(validationResult);
            Assert.True(validationResult.IsValid);
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
                    LearningHubUrl = "https://testurl.com",
                });
        }
    }
}
