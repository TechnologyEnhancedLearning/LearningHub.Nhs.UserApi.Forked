// <copyright file="SpecialtyController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The specialty controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        /// <summary>
        /// The specialty service.
        /// </summary>
        private ISpecialtyService specialtyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyController"/> class.
        /// </summary>
        /// <param name="specialtyService">
        /// The specialty service.
        /// </param>
        public SpecialtyController(
            ISpecialtyService specialtyService)
        {
            this.specialtyService = specialtyService;
        }

        // GET api/Specialty/GetById/id

        /// <summary>
        /// Get Specialty record by id.
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
            var specialty = await this.specialtyService.GetByIdAsync(id);

            return this.Ok(specialty);
        }

        // GET api/Specialty/GetAll

        /// <summary>
        /// Get a page of Specialty records.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var list = this.specialtyService.GetAll();
            return this.Ok(list);
        }

        /// <summary>
        /// Get paged Specialty records.
        /// </summary>
        /// <param name="filter">filter.</param>
        /// <param name="page">page.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAllPaged/{filter}/{page}/{pageSize}")]
        public Tuple<int, List<Specialty>> GetAllPaged(string filter, int page, int pageSize)
        {
            var list = this.specialtyService.GetAll().Where(x => x.Name.ToLower().Contains(filter.ToLower())).ToList();
            int total = list.Count;
            var pagedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, List<Specialty>>(total, pagedList);
        }
    }
}