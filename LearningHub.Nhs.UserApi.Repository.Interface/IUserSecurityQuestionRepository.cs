namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserSecurityQuestionRepository interface.
    /// </summary>
    public interface IUserSecurityQuestionRepository : IGenericElfhRepository<UserSecurityQuestion>
    {
        /// <summary>
        /// The get by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<UserSecurityQuestion> GetByUserId(int userId);
    }
}
