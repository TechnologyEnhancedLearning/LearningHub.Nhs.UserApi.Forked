namespace LearningHub.Nhs.UserApi.Services
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Validation;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The terms and conditions service.
    /// </summary>
    public class TermsAndConditionsService : ITermsAndConditionsService
    {
        /// <summary>
        /// The terms and conditions repository.
        /// </summary>
        private readonly ITermsAndConditionsRepository termsAndConditionsRepository;

        /// <summary>
        /// The user terms and conditions repository.
        /// </summary>
        private readonly IUserTermsAndConditionsRepository userTermsAndConditionsRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<TermsAndConditionsService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsService"/> class.
        /// </summary>
        /// <param name="termsAndConditionsRepository">
        /// The terms and conditions repository.
        /// </param>
        /// <param name="userTermsAndConditionsRepository">
        /// The user terms and conditions repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TermsAndConditionsService(
            ITermsAndConditionsRepository termsAndConditionsRepository,
            IUserTermsAndConditionsRepository userTermsAndConditionsRepository,
            ILogger<TermsAndConditionsService> logger)
        {
            this.termsAndConditionsRepository = termsAndConditionsRepository;
            this.userTermsAndConditionsRepository = userTermsAndConditionsRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> AcceptByUser(UserTermsAndConditionsViewModel userTermsAndConditionsVM, int userId)
        {
            var retVal = await this.ValidateAsync(userTermsAndConditionsVM);

            if (retVal.IsValid)
            {
                UserTermsAndConditions userTermsAndConditions = new UserTermsAndConditions()
                {
                    TermsAndConditionsId = userTermsAndConditionsVM.TermsAndConditionsId,
                    UserId = userTermsAndConditionsVM.UserId,
                };

                retVal.CreatedId = await this.userTermsAndConditionsRepository.CreateAsync(userId, userTermsAndConditions);
            }

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<TermsAndConditions> GetLatestVersionAsync(int tenantId)
        {
            var country = await this.termsAndConditionsRepository.LatestVersionAsync(tenantId);

            return country;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(UserTermsAndConditionsViewModel userTermsAndConditionsVM)
        {
            var userTermsAndConditionsValidator = new UserTermsAndConditionsValidator();
            var clientValidationResult = await userTermsAndConditionsValidator.ValidateAsync(userTermsAndConditionsVM);

            var retVal = new LearningHubValidationResult(clientValidationResult);

            return retVal;
        }
    }
}