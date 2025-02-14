namespace LearningHub.Nhs.Auth.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// IMoodleApiService.
    /// </summary>
    public interface IMoodleApiService
    {
        /// <summary>
        /// GetResourcesAsync.
        /// </summary>
        /// <param name="currentUserId">The current User Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<int> GetMoodleUserIdByUsernameAsync(int currentUserId);
    }
}
