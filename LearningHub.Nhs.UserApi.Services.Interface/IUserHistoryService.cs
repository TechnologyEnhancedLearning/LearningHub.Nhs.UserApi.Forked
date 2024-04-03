namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The UserHistoryService interface.
    /// </summary>
    public interface IUserHistoryService
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserHistory> GetByIdAsync(int id);

        /// <summary>
        /// The get by user id async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<UserHistoryViewModel>> GetByUserIdAsync(int userId);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userHistory">The user history.</param>
        /// <param name="currentUserId">The current user id.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> CreateAsync(UserHistoryViewModel userHistory, int currentUserId = 0);

        /// <summary>
        /// Returns a list of users - filtered, sorted and paged as required.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="presetFilter">The preset filter.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<PagedResultSet<UserHistoryViewModel>> GetUserHistoryPageAsync(int page, int pageSize, string sortColumn = "", string sortDirection = "", string presetFilter = "", string filter = "");
    }
}
