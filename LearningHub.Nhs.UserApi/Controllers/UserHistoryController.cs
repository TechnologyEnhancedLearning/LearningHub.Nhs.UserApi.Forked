namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user history controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService userHistoryService;
        private readonly ILogger<UserHistoryController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHistoryController"/> class.
        /// </summary>
        /// <param name="userHistoryService">The user history service.</param>
        /// <param name="logger">The logger.</param>
        public UserHistoryController(IUserHistoryService userHistoryService, ILogger<UserHistoryController> logger)
        {
            this.userHistoryService = userHistoryService;
            this.logger = logger;
        }

        // GET api/UserHistory/GetById/id

        /// <summary>
        /// Get UserHistory record by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userHistory = await this.userHistoryService.GetByIdAsync(id);

            return this.Ok(userHistory);
        }

        /// <summary>
        /// Get UserHistory records by user id.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetByUserId/{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var userHistory = await this.userHistoryService.GetByUserIdAsync(id);

            return this.Ok(userHistory);
        }

        /// <summary>
        /// Get UserHistory record by id.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <param name="presetFilter">
        /// The preset filter.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetUserHistoryPage/{page}/{pageSize}/{sortColumn}/{sortDirection}/{presetFilter}/{filter}")]
        public async Task<IActionResult> GetUserHistoryPageAsync(int page, int pageSize, string sortColumn, string sortDirection, string presetFilter, string filter)
        {
            PagedResultSet<UserHistoryViewModel> pagedResultSet = await this.userHistoryService.GetUserHistoryPageAsync(page, pageSize, sortColumn, sortDirection, presetFilter, filter);
            return this.Ok(pagedResultSet);
        }

        /// <summary>
        /// Check the user has an active login session.
        /// </summary>
        /// <param name="userId">The UserId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("CheckUserHasActiveSession/{userId}")]
        public async Task<IActionResult> CheckUserHasActiveSessionAsync(int userId)
        {
            PagedResultSet<UserHistoryViewModel> pagedResultSet = await this.userHistoryService.CheckUserHasActiveSessionAsync(userId);
            return this.Ok(pagedResultSet);
        }

        /// <summary>
        /// Create a UserHistory.
        /// </summary>
        /// <param name="userHistory">The user history.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserHistoryViewModel userHistory)
        {
            var vr = await this.userHistoryService.CreateAsync(userHistory);
            if (vr.IsValid)
            {
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                this.logger.LogError("UserHistory - PostAsync: " + string.Join(",", vr.Details.ToArray()));
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }
    }
}