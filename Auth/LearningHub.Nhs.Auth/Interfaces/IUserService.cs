namespace LearningHub.Nhs.Auth.Interfaces
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Auth.Models;
    using LearningHub.Nhs.Models.Common;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The UserService interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// The authenticate user async.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LoginResult> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// The authenticate user sso async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="externalSystemId">The client id.</param>
        /// <param name="clientCode">The client Code.</param>
        /// <returns>The <see cref="LoginResultInternal"/>.</returns>
        Task<LoginResultInternal> AuthenticateSsoUserAsync(int userId, int externalSystemId, string clientCode);

        /// <summary>
        /// The get user by user name async.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserBasicViewModel> GetUserByUserNameAsync(string username);

        /// <summary>
        /// The get basic user by user id async.
        /// </summary>
        /// <param name="subjectId">
        /// The subject id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserBasicViewModel> GetBasicUserByUserIdAsync(string subjectId);

        /// <summary>
        /// The get user by user id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserViewModel> GetUserByUserIdAsync(string id);

        /// <summary>
        /// The get user id by user name async.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> GetUserIdByUserNameAsync(string username);

        /// <summary>
        /// The get user by oa user id async.
        /// </summary>
        /// <param name="oaUserId">
        /// The oa user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        Task<UserBasic> GetUserByOAUserIdAsync(string oaUserId);

        /// <summary>
        /// The get user role async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<string> GetUserRoleAsync(int id);

        /// <summary>
        /// The store user history async.
        /// </summary>
        /// <param name="userHistory">
        /// The user history.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task StoreUserHistoryAsync(UserHistoryViewModel userHistory);

        /// <summary>
        /// check user has an laredy active session.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<PagedResultSet<UserHistoryViewModel>> CheckUserHasAnActiveSessionAsync(int userId);

        /// <summary>
        /// The store user history async.
        /// </summary>
        /// <param name="detail">Text description of the user history entry.</param>
        /// <param name="userId">User id.</param>
        /// <param name="userHistoryType">User history type.</param>
        /// <param name="loginSuccessFull">Login successful.</param>
        /// <param name="request">Http request.</param>
        /// <param name="externalReferer">External referer.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task AddLogonToUserHistory(string detail, int userId, UserHistoryType userHistoryType, bool loginSuccessFull, HttpRequest request, string externalReferer);
    }
}
