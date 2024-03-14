namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserHistoryRepository interface.
    /// </summary>
    public interface IUserHistoryRepository
    {
        /// <summary>
        /// create user history.
        /// </summary>
        /// <param name="userId">user id.</param>
        /// <param name="tenantId">tenant id.</param>
        /// <param name="userHistoryVM">userhistory model.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateAsync(int userId, int tenantId, UserHistoryViewModel userHistoryVM);

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserHistory> GetByIdAsync(int id);

        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The userId.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<UserHistoryStoredProcResult>> GetByUserIdAsync(int userId);

        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The userId.
        /// </param>
        /// <param name="startPage">
        /// The startPage.
        /// </param>
        /// <param name="pageSize">
        /// The pageSize.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserHistoryStoredProcResults> GetPagedByUserIdAsync(int userId, int startPage, int pageSize);
    }
}