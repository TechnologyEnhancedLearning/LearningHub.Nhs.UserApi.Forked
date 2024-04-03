namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The UserGroupRepository interface.
    /// </summary>
    public interface IUserGroupRepository : IGenericElfhRepository<UserGroup>
    {
        /// <summary>
        /// The get by user async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<UserGroup>> GetByUserAsync(int userId);
    }
}
