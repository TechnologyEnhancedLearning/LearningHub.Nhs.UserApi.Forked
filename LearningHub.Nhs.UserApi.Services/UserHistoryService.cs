namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Validation;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using UAParser;

    /// <summary>
    /// The user history service.
    /// </summary>
    public class UserHistoryService : IUserHistoryService
    {
        private readonly Settings settings;
        private readonly IUserHistoryRepository userHistoryRepository;
        private readonly IMapper mapper;
        private readonly Parser userAgentParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHistoryService"/> class.
        /// </summary>
        /// <param name="userHistoryRepo">The user history repository.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="mapper">The mapper.</param>
        public UserHistoryService(
            IUserHistoryRepository userHistoryRepo,
            IOptions<Settings> settings,
            IMapper mapper)
        {
            this.userHistoryRepository = userHistoryRepo;
            this.settings = settings.Value;
            this.mapper = mapper;

            this.userAgentParser = Parser.GetDefault();
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> CreateAsync(UserHistoryViewModel userHistoryVM, int currentUserId = 0)
        {
            if (userHistoryVM.UserId == 0 && currentUserId != 0)
            {
                userHistoryVM.UserId = currentUserId;
            }

            var retVal = await this.ValidateAsync(userHistoryVM);

            if (retVal.IsValid)
            {
                userHistoryVM.SessionId = Guid.NewGuid().ToString();
                userHistoryVM.IsActive = true;
                await this.userHistoryRepository.CreateAsync(userHistoryVM.UserId, this.settings.LearningHubTenantId, userHistoryVM);
            }

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<UserHistory> GetByIdAsync(int id)
        {
            var userHistory = await this.userHistoryRepository.GetByIdAsync(id);

            return userHistory;
        }

        /// <inheritdoc/>
        public async Task<List<UserHistoryViewModel>> GetByUserIdAsync(int userId)
        {
            var userHistory = await this.userHistoryRepository.GetByUserIdAsync(userId);
            userHistory.ForEach(x => x.UserAgent = this.ParseUserAgentString(x.UserAgent));

            var retVal = this.mapper.Map<List<UserHistoryViewModel>>(userHistory);

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<PagedResultSet<UserHistoryViewModel>> GetUserHistoryPageAsync(int page, int pageSize, string sortColumn = "", string sortDirection = "", string presetFilter = "", string filter = "")
        {
            PagedResultSet<UserHistoryViewModel> result = new PagedResultSet<UserHistoryViewModel>();
            var presetFilterCriteria = JsonConvert.DeserializeObject<List<PagingColumnFilter>>(presetFilter);
            var userIdFilter = presetFilterCriteria.Where(f => f.Column == "userid").FirstOrDefault();
            if (userIdFilter != null)
            {
                var userHistory = await this.userHistoryRepository.GetPagedByUserIdAsync(int.Parse(userIdFilter.Value), page, pageSize);
                userHistory.Results.ForEach(x => x.UserAgent = this.ParseUserAgentString(x.UserAgent));

                result.Items = this.mapper.Map<List<UserHistoryViewModel>>(userHistory.Results);
                result.TotalItemCount = userHistory.TotalResults;
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<PagedResultSet<UserHistoryViewModel>> CheckUserHasActiveSessionAsync(int userId)
        {
            PagedResultSet<UserHistoryViewModel> result = new PagedResultSet<UserHistoryViewModel>();
            var userHistory = await this.userHistoryRepository.CheckUserHasActiveSessionAsync(userId);
            userHistory.Results.ForEach(x => x.UserAgent = this.ParseUserAgentString(x.UserAgent));
            result.Items = this.mapper.Map<List<UserHistoryViewModel>>(userHistory.Results);
            return result;
        }

        private string ParseUserAgentString(string userAgent)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(userAgent))
            {
                var clientInfo = this.userAgentParser.Parse(userAgent);

                if (clientInfo.Device != null && !string.IsNullOrEmpty(clientInfo.Device.Model))
                {
                    retVal = $"{clientInfo.Device.Model} ";
                }

                if (clientInfo.OS != null)
                {
                    retVal += clientInfo.OS;
                }
            }

            return retVal;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(UserHistoryViewModel userHistory)
        {
            var userHistoryValidator = new UserHistoryValidator();
            var clientValidationResult = await userHistoryValidator.ValidateAsync(userHistory).ConfigureAwait(false);

            var retVal = new LearningHubValidationResult(clientValidationResult);

            return retVal;
        }
    }
}