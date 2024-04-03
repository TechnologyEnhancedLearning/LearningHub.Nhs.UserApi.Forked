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
    /// The grade controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        /// <summary>
        /// The grade service.
        /// </summary>
        private IGradeService gradeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeController"/> class.
        /// </summary>
        /// <param name="gradeService">
        /// The grade service.
        /// </param>
        public GradeController(
            IGradeService gradeService)
        {
            this.gradeService = gradeService;
        }

        // GET api/Grade/GetById/id

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
            var grade = await this.gradeService.GetByIdAsync(id);

            return this.Ok(grade);
        }

        // GET api/Grade/GetAll

        /// <summary>
        /// Get a all Grade records.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var list = this.gradeService.GetAll();
            return this.Ok(list);
        }

        // GET api/Grade/GetByJobRole

        /// <summary>
        /// Get a Grade records by JobRole.
        /// </summary>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetByJobRole/{jobRoleId}")]
        public async Task<IActionResult> GetByJobRole(int jobRoleId)
        {
            var list = await this.gradeService.GetByJobRole(jobRoleId);
            return this.Ok(list);
        }

        /// <summary>
        /// Get paged records by JobRole.
        /// </summary>
        /// <param name="jobRoleId">
        /// The job role id.
        /// </param>
        /// <param name="page">page.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetPagedByJobRole/{jobRoleId}/{page}/{pageSize}")]
        public async Task<Tuple<int, List<Grade>>> GetPagedByJobRole(int jobRoleId, int page, int pageSize)
        {
            var list = await this.gradeService.GetByJobRole(jobRoleId);
            int total = list.Count;
            var pagedList = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<int, List<Grade>>(total, pagedList);
        }
    }
}