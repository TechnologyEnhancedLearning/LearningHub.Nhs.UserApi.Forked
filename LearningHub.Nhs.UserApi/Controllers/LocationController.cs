// <copyright file="LocationController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    ////[Authorize(Policy = "AuthorizeOrCallFromLH")]

    /// <summary>
    /// The location controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        /// <summary>
        /// The location service.
        /// </summary>
        private ILocationService locationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationController"/> class.
        /// </summary>
        /// <param name="locationService">
        /// The location service.
        /// </param>
        public LocationController(
            ILocationService locationService)
        {
            this.locationService = locationService;
        }

        // GET api/Location/GetById/id

        /// <summary>
        /// Get Location record by id.
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
            var location = await this.locationService.GetByIdAsync(id);

            return this.Ok(location);
        }

        /// <summary>
        /// Get Locations by search criteria.
        /// </summary>
        /// <param name="criteria">
        /// The criteria.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetBySearchCriteria/{criteria}")]
        public async Task<IActionResult> GetBySearchCriteria(string criteria)
        {
            var locations = await this.locationService.GetBySearchCriteriaAsync(criteria, false, 150);

            return this.Ok(locations);
        }

        /// <summary>
        /// Get Locations by search criteria.
        /// </summary>
        /// <param name="criteria">
        /// The criteria.
        /// </param>
        /// <param name="page">page.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetPagedBySearchCriteria/{criteria}/{page}/{pageSize}")]
        public async Task<Tuple<int, List<LocationBasicViewModel>>> GetPagedBySearchCriteria(string criteria, int page, int pageSize)
        {
            var locations = await this.locationService.GetBySearchCriteriaAsync(criteria, false, 150);
            int total = locations.Count;
            var pagedList = locations.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, List<LocationBasicViewModel>>(total, pagedList);
        }
    }
}