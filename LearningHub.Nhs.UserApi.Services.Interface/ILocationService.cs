// <copyright file="ILocationService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;

    /// <summary>
    /// The LocationService interface.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// The get by search criteria async.
        /// </summary>
        /// <param name="searchTerm">
        /// The search term.
        /// </param>
        /// <param name="showArchived">
        /// The show archived.
        /// </param>
        /// <param name="maxRecords">
        /// The max records.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<LocationBasicViewModel>> GetBySearchCriteriaAsync(string searchTerm, bool showArchived, int maxRecords);

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LocationBasicViewModel> GetByIdAsync(int id);
    }
}
