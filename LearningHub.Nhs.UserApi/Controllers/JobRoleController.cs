// <copyright file="JobRoleController.cs" company="HEE.nhs.uk">
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
    /// The job role controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController : ControllerBase
    {
        /// <summary>
        /// The job role service.
        /// </summary>
        private IJobRoleService jobRoleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRoleController"/> class.
        /// </summary>
        /// <param name="jobRoleService">The job role service.</param>
        public JobRoleController(
            IJobRoleService jobRoleService)
        {
            this.jobRoleService = jobRoleService;
        }

        // GET api/JobRole/GetById/id

        /// <summary>
        /// Get JobRole record by id.
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
            var jobRole = await this.jobRoleService.GetByIdAsync(id);

            return this.Ok(jobRole);
        }

        // GET api/JobRole/GetAll

        /// <summary>
        /// Get all StaffGroup records.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var list = this.jobRoleService.GetAll();
            return this.Ok(list);
        }

        // GET api/JobRole/GetAll

        /// <summary>
        /// Get a page of StaffGroup records.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetFilteredWithStaffGroup/{filter}")]
        public async Task<IActionResult> GetFilteredWithStaffGroup(string filter)
        {
            var list = await this.jobRoleService.GetFilteredWithStaffGroupAsync(filter);
            return this.Ok(list);
        }

        /// <summary>
        /// Get paged StaffGroup records.
        /// </summary>
        /// <param name="filter">filter.</param>
        /// <param name="page">page.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetPagedFilteredWithStaffGroup/{filter}/{page}/{pageSize}")]
        public async Task<Tuple<int, List<JobRoleBasicViewModel>>> GetPagedFilteredWithStaffGroup(string filter, int page, int pageSize)
        {
            var list = await this.jobRoleService.GetFilteredWithStaffGroupAsync(filter);
            int total = list.Count;
            var pagedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, List<JobRoleBasicViewModel>>(total, pagedList);
        }

        /// <summary>
        /// Get job roles by staff group id.
        /// </summary>
        /// <param name="staffGroupId">The staffGroupId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetByStaffGroupId/{staffGroupId}")]
        public async Task<IActionResult> GetByStaffGroupId(int staffGroupId)
        {
            var list = await this.jobRoleService.GetByStaffGroupIdAsync(staffGroupId);
            return this.Ok(list);
        }
    }
}