// <copyright file="StaffGroupController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The staff group controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class StaffGroupController : ControllerBase
    {
        /// <summary>
        /// The staff group service.
        /// </summary>
        private IStaffGroupService staffGroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffGroupController"/> class.
        /// </summary>
        /// <param name="staffGroupService">
        /// The staff group service.
        /// </param>
        public StaffGroupController(
            IStaffGroupService staffGroupService)
        {
            this.staffGroupService = staffGroupService;
        }

        // GET api/StaffGroup/GetById/id

        /// <summary>
        /// The get by id.
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
            var staffGroup = await this.staffGroupService.GetByIdAsync(id);

            return this.Ok(staffGroup);
        }

        // GET api/StaffGroup/GetAll

        /// <summary>
        /// Get a page of StaffGroup records.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var list = this.staffGroupService.GetAll();
            return this.Ok(list);
        }
    }
}