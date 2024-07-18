namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The CountryService interface.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Country> GetByIdAsync(int id);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<Country> GetAll();

        /// <summary>
        /// The get filtered async.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<GenericListViewModel>> GetFilteredAsync(string filter);
    }
}