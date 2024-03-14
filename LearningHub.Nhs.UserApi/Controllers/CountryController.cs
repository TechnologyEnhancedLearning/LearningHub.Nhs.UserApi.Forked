namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The country controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        /// <summary>
        /// The country service.
        /// </summary>
        private ICountryService countryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryController"/> class.
        /// </summary>
        /// <param name="countryService">
        /// The country service.
        /// </param>
        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        // GET api/Country/GetById/id

        /// <summary>
        /// Get country by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await this.countryService.GetByIdAsync(id);

            return this.Ok(log);
        }

        // GET api/Country/GetAll

        /// <summary>
        /// Get a page of Country records.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var list = this.countryService.GetAll();
            return this.Ok(list);
        }

        // GET api/Country/GetFiltered/filter

        /// <summary>
        /// Get a subset of Country records.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetFiltered/{filter}")]
        public async Task<IActionResult> GetFiltered(string filter)
        {
            var list = await this.countryService.GetFilteredAsync(filter);
            return this.Ok(list);
        }
    }
}