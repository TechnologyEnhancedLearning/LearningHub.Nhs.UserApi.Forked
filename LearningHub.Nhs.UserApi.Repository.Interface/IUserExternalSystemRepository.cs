namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// IUserExternalSystemRepository.
    /// </summary>
    public interface IUserExternalSystemRepository : IGenericElfhRepository<UserExternalSystem>
    {
        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="externalSystemId">externalSystemId.</param>
        /// <returns>
        /// The true if exists.
        /// </returns>
        UserExternalSystem GetUserExternalSystem(int userId, int externalSystemId);

        /// <summary>
        /// The get user external system details by userId and code.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="code">externalSystemId.</param>
        /// <returns>
        /// The true if exists.
        /// </returns>
        Task<UserExternalSystem> GetUserExternalSystemByUserIdandCodeAsync(int userId, string code);
    }
}
