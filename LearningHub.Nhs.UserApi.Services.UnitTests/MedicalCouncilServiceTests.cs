namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Moq;
    using Xunit;

    /// <summary>
    /// The medical council service tests.
    /// </summary>
    public class MedicalCouncilServiceTests
    {
        /// <summary>
        /// The validate gdc number_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGDCNumber_Valid()
        {
            var gdcRegisterRepositoryMock = new Mock<IGdcRegisterRepository>(MockBehavior.Strict);

            gdcRegisterRepositoryMock.Setup(r => r.GetByLastNameAndGDCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GdcRegister()));

            var medicalCouncilService = new MedicalCouncilService(null, gdcRegisterRepositoryMock.Object, null, null);

            var errorMessage = await medicalCouncilService.ValidateGDCNumber("TestLastName", "123456");

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate gdc number_ in valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGDCNumber_InValid()
        {
            var gdcRegisterRepositoryMock = new Mock<IGdcRegisterRepository>(MockBehavior.Strict);

            gdcRegisterRepositoryMock.Setup(r => r.GetByLastNameAndGDCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult<GdcRegister>(null));

            var medicalCouncilService = new MedicalCouncilService(null, gdcRegisterRepositoryMock.Object, null, null);

            var errorMessage = await medicalCouncilService.ValidateGDCNumber("TestLastName", "123456");

            Assert.IsType<string>(errorMessage);
            Assert.Equal("These details do not match the records held by the GDC. Please ensure that the surname and GMC number entered are the same as the details held on the GDC register.", errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ registere d_ wit h_ licenc e_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_REGISTERED_WITH_LICENCE_Valid()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GmcLrmp()
                                            {
                                                GmcRefNo = "123456",
                                                RegistrationStatus = "REGISTERED WITH LICENCE",
                                            }));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ registere d_ withou t_ a_ licenc e_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_REGISTERED_WITHOUT_A_LICENCE_Valid()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GmcLrmp()
                                            {
                                                GmcRefNo = "123456",
                                                RegistrationStatus = "REGISTERED WITHOUT A LICENCE",
                                            }));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ provisionally_registered_with_ licence_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_Provisionally_registered_with_Licence_Valid()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GmcLrmp()
                                            {
                                                GmcRefNo = "123456",
                                                RegistrationStatus = "Provisionally registered with Licence",
                                            }));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ provisionally_registered_without_a_ licence_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_Provisionally_registered_without_a_Licence_Valid()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GmcLrmp()
                                            {
                                                GmcRefNo = "123456",
                                                RegistrationStatus = "Provisionally registered without a Licence",
                                            }));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ in valid_ status.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_InValid_Status()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult(
                                            new GmcLrmp()
                                            {
                                                GmcRefNo = "123456",
                                                RegistrationStatus = "Not Registered - Administrative Reason",
                                            }));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal("You do not appear to have a valid GMC registration status Not Registered - Administrative Reason", errorMessage);
        }

        /// <summary>
        /// The validate gmc number_ in valid_ gmc number.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task ValidateGMCNumber_InValid_GMCNumber()
        {
            var gmcLrmpRepositoryMock = new Mock<IGmcLrmpRepository>(MockBehavior.Strict);

            gmcLrmpRepositoryMock.Setup(r => r.GetByLastNameAndGMCNumber(It.IsAny<string>(), It.IsAny<string>()))
                                        .Returns(Task.FromResult<GmcLrmp>(null));

            var medicalCouncilService = new MedicalCouncilService(gmcLrmpRepositoryMock.Object, null, null, null);

            var errorMessage = await medicalCouncilService.ValidateGMCNumber("TestLastName", "123456", null, string.Empty);

            Assert.IsType<string>(errorMessage);
            Assert.Equal("These details do not match the records held by the GMC. Please ensure that the surname and GMC number entered are the same as the details held on the GMC register.", errorMessage);
        }

        /// <summary>
        /// The validate nmc number_ valid.
        /// </summary>
        [Fact]
        public void ValidateNMCNumber_Valid()
        {
            var medicalCouncilService = new MedicalCouncilService(null, null, null, null);

            var errorMessage = medicalCouncilService.ValidateNMCNumber("12A4567B");

            Assert.IsType<string>(errorMessage);
            Assert.Equal(string.Empty, errorMessage);
        }

        /// <summary>
        /// The validate nmc number_ in valid.
        /// </summary>
        [Fact]
        public void ValidateNMCNumber_InValid()
        {
            var medicalCouncilService = new MedicalCouncilService(null, null, null, null);

            var errorMessage = medicalCouncilService.ValidateNMCNumber("A4567B12");

            Assert.IsType<string>(errorMessage);
            Assert.Equal("Invalid Nursing and Midwifery Council Number!", errorMessage);
        }
    }
}
