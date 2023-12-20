// <copyright file="RegionController.cs" company="HEE.nhs.uk">
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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The region controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        /// <summary>
        /// The region service.
        /// </summary>
        private IRegionService regionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionController"/> class.
        /// </summary>
        /// <param name="regionService">The region service.</param>
        public RegionController(
            IRegionService regionService)
        {
            this.regionService = regionService;
        }

        // GET api/Region/GetById/id

        /// <summary>
        /// Get Region record by id.
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
            var region = await this.regionService.GetByIdAsync(id);

            return this.Ok(region);
        }

        // GET api/Region/GetAll

        /// <summary>
        /// Get a page of Region records.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await this.regionService.GetAllAsync();
            return this.Ok(list);
        }

        /// <summary>
        /// Get paged Region records.
        /// </summary>
        /// <param name="page">page.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAllPaged/{page}/{pageSize}")]
        public async Task<Tuple<int, List<GenericListViewModel>>> GetAllPaged(int page, int pageSize)
        {
            var list = await this.regionService.GetAllAsync();
            int total = list.Count;
            var pagedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, List<GenericListViewModel>>(total, pagedList);
        }
    }
}