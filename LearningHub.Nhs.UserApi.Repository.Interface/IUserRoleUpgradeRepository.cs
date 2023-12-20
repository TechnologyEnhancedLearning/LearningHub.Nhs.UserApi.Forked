// <copyright file="IUserRoleUpgradeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The EmailTemplateRepository interface.
    /// </summary>
    public interface IUserRoleUpgradeRepository : IGenericElfhRepository<UserRoleUpgrade>
    {
        /// <summary>
        /// The get by user id async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<UserRoleUpgrade> GetByUserIdAsync(int userId);

        /// <summary>
        /// The get by User Id and email Address async.
        /// </summary>
        /// <param name="emailAddress">
        /// The email Address.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<UserRoleUpgrade> GetByEmailAddressAsync(string emailAddress, int userId);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userRoleUpgrade">
        /// The email change validation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<int> CreateAsync(int userId, UserRoleUpgrade userRoleUpgrade);

        /// <summary>
        /// The create async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userRoleUpgrade">
        /// The email change validation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task UpdateAsync(int userId, UserRoleUpgrade userRoleUpgrade);
    }
}
