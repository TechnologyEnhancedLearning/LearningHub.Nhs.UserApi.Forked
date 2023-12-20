// <copyright file="UserEmploymentController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// UserEmployment operations.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserEmploymentController : ApiControllerBase
    {
        /// <summary>
        /// The user employment service.
        /// </summary>
        private readonly IUserEmploymentService userEmploymentService;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<UserEmploymentController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEmploymentController"/> class.
        /// </summary>
        /// <param name="elfhUserService">
        /// The elfh user service.
        /// </param>
        /// <param name="userEmploymentService">
        /// The user employment service.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserEmploymentController(
                                        IElfhUserService elfhUserService,
                                        IUserEmploymentService userEmploymentService,
                                        ILogger<UserEmploymentController> logger)
            : base(elfhUserService)
        {
            this.userEmploymentService = userEmploymentService;
            this.logger = logger;
        }

        // GET api/UserEmployment/GetById/id

        /// <summary>
        /// Get UserEmployment record by id.
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
            var userEmployment = await this.userEmploymentService.GetByIdAsync(id);

            return this.Ok(userEmployment);
        }

        /// <summary>
        /// Get Prinary UserEmployment record by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetPrimaryForUser/{id}")]
        public async Task<IActionResult> GetPrimaryForUser(int id)
        {
            var userEmployment = await this.userEmploymentService.GetPrimaryForUser(id);

            return this.Ok(userEmployment);
        }

        /// <summary>
        /// Update an existing UserEmployment record.
        /// </summary>
        /// <param name="userEmployment">
        /// The user employment.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut("{userEmployment}")]
        public async Task<IActionResult> PutAsync([FromBody] UserEmploymentViewModel userEmployment)
        {
            this.logger.LogDebug($"UserEmployment={JsonConvert.SerializeObject(userEmployment)}");

            var vr = await this.userEmploymentService.UpdateAsync(this.CurrentUserId, userEmployment);
            if (vr.IsValid)
            {
                this.logger.LogDebug($"IsValid=true");
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                this.logger.LogDebug($"IsValid=false, validation details={string.Join(",", vr.Details.ToArray())}");
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }
    }
}