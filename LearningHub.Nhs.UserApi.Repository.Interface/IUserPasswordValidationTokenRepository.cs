namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserPasswordValidationTokenRepository interface.
    /// </summary>
    public interface IUserPasswordValidationTokenRepository
    {
        /// <summary>
        /// The get by token.
        /// </summary>
        /// <param name="lookup">
        /// The lookup.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<UserPasswordValidationToken> GetByToken(string lookup);

        /// <summary>
        /// The expire user password validation token.
        /// </summary>
        /// <param name="lookup">
        /// The lookup.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task ExpireUserPasswordValidationToken(string lookup);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userPasswordValidationToken">
        /// The user password validation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> CreateAsync(int userId, UserPasswordValidationToken userPasswordValidationToken);
    }
}
