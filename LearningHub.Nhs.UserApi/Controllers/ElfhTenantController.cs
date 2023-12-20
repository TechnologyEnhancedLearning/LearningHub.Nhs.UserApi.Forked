// <copyright file="ElfhTenantController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The elfh tenant controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class ElfhTenantController : ControllerBase
    {
        /// <summary>
        /// The tenant service.
        /// </summary>
        private readonly ITenantService tenantService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ElfhTenantController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhTenantController"/> class.
        /// </summary>
        /// <param name="tenantService">The tenant service.</param>
        /// <param name="logger">The logger.</param>
        public ElfhTenantController(
            ITenantService tenantService,
            ILogger<ElfhTenantController> logger)
        {
            this.tenantService = tenantService;
            this.logger = logger;
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="includeTenantUrls">
        /// The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("getbyid/{id}/{includeTenantUrls}")]
        public async Task<IActionResult> GetByIdAsync(int id, bool includeTenantUrls)
        {
            if (id < 1)
            {
                return this.BadRequest("Tenant Id is malformed.");
            }

            return this.Ok(await this.tenantService.GetByIdAsync(id, includeTenantUrls));
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <param name="includeTenantUrls">
        /// The include tenant urls.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("getall/{includeTenantUrls}")]
        public async Task<IActionResult> GetAllAsync(bool includeTenantUrls)
        {
            return this.Ok(await this.tenantService.GetAllAsync(includeTenantUrls));
        }
    }
}
