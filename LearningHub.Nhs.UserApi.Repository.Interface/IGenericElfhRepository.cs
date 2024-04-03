namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The Elfh Generic Repository interface.
    /// </summary>
    /// <typeparam name="TEntity">Input type.</typeparam>
    public interface IGenericElfhRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<int> CreateAsync(int userId, TEntity entity);

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateAsync(int userId, TEntity entity);
    }
}