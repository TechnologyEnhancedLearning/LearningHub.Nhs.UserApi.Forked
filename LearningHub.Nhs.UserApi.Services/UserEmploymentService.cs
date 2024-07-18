namespace LearningHub.Nhs.UserApi.Services
{
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Validation;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The user employment service.
    /// </summary>
    public class UserEmploymentService : IUserEmploymentService
    {
        /// <summary>
        /// The user employment repository.
        /// </summary>
        private readonly IUserEmploymentRepository userEmploymentRepository;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly IOptions<Settings> settings;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEmploymentService"/> class.
        /// </summary>
        /// <param name="userEmploymentRepository">
        /// The user employment repository.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public UserEmploymentService(
            IUserEmploymentRepository userEmploymentRepository,
            IOptions<Settings> settings,
            IMapper mapper)
        {
            this.userEmploymentRepository = userEmploymentRepository;
            this.settings = settings;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<UserEmployment> GetByIdAsync(int id)
        {
            var userEmployment = await this.userEmploymentRepository.GetByIdAsync(id);

            return userEmployment;
        }

        /// <inheritdoc/>
        public async Task<UserEmploymentViewModel> GetPrimaryForUser(int id)
        {
            var userEmployment = await this.userEmploymentRepository.GetPrimaryForUser(id);

            var userEmploymentVM = this.mapper.Map<UserEmploymentViewModel>(userEmployment);

            return userEmploymentVM;
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> UpdateAsync(int userId, UserEmploymentViewModel userEmploymentVM)
        {
            var userEmployment = this.mapper.Map<UserEmployment>(userEmploymentVM);

            var retVal = await this.ValidateAsync(userEmployment);

            if (retVal.IsValid)
            {
                if (userEmployment.MedicalCouncilId == 0)
                {
                    userEmployment.MedicalCouncilId = null;
                }

                await this.userEmploymentRepository.UpdateAsync(userId, userEmployment);
            }

            return retVal;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(UserEmployment userEmployment)
        {
            var userEmploymentValidator = new UserEmploymentValidator();
            var clientValidationResult = await userEmploymentValidator.ValidateAsync(userEmployment);

            var retVal = new LearningHubValidationResult(clientValidationResult);

            return retVal;
        }
    }
}
