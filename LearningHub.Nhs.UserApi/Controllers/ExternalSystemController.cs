// <copyright file="ExternalSystemController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// External System Properties.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalSystemController : ApiControllerBase
    {
        private readonly IExternalSystemService lhExtSysSvc;
        private readonly IExternalSystemDeepLinkService lhExtSysDeepLinkSvc;
        private readonly ILogger<ExternalSystemController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemController"/> class.
        /// External System Controller.
        /// </summary>
        /// <param name="elfhUserService">The elfh user service.</param>
        /// <param name="lhExtSysSvc">The learning hub ext Sys Svc.</param>
        /// <param name="lhExtSysDeepLinkSvc">The learning hub ext deep link Svc.</param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public ExternalSystemController(
            IElfhUserService elfhUserService,
            IExternalSystemService lhExtSysSvc,
            IExternalSystemDeepLinkService lhExtSysDeepLinkSvc,
            ILogger<ExternalSystemController> logger)
            : base(elfhUserService)
        {
            this.lhExtSysSvc = lhExtSysSvc;
            this.lhExtSysDeepLinkSvc = lhExtSysDeepLinkSvc;
            this.logger = logger;
        }

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        [Route("GetExternalSystem/{clientCode}")]
        public async Task<IActionResult> GetExternalClient(string clientCode)
        {
            return this.Ok(await this.lhExtSysSvc.GetByCodeAsync(clientCode));
        }

        /// <summary>
        /// Get external system deep link entity by code.
        /// </summary>
        /// <param name="code">The end client code.</param>
        /// <returns>The external system deep link.</returns>
        [HttpGet]
        [Route("GetExternalSystemDeepLink/{code}")]
        public async Task<IActionResult> GetExternalClientDeepLink(string code)
        {
            return this.Ok(await this.lhExtSysDeepLinkSvc.GetByCodeAsync(code));
        }

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>The <see cref="IActionResult"/>.</returns>
        [HttpGet]
        [Route("GetExternalSystemById/{id}")]
        public async Task<IActionResult> GetExternalSystemById(int id)
        {
            return this.Ok(await this.lhExtSysSvc.GetExtSystemById(id));
        }

        /// <summary>
        /// The get filtered page.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="sortColumn">
        /// The sort column.
        /// </param>
        /// <param name="sortDirection">
        /// The sort direction.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetFilteredPage/{page}/{pageSize}/{sortColumn}/{sortDirection}/{filter}")]
        public async Task<IActionResult> GetFilteredPage(int page, int pageSize, string sortColumn, string sortDirection, string filter)
        {
            PagedResultSet<ExternalSystem> pagedResultSet = await this.lhExtSysSvc.GetExternalSystems(page, pageSize, sortColumn, sortDirection, filter);
            return this.Ok(pagedResultSet);
        }

        /// <summary>
        /// Create a new ExternalSystem.
        /// </summary>
        /// <param name="externalsystem">
        /// The lhExtSysSvc.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("{externalsystem}")]
        public async Task<IActionResult> PostAsync([FromBody] ExternalSystem externalsystem)
        {
            try
            {
                var vr = await this.lhExtSysSvc.CreateAsync(this.CurrentUserId, externalsystem);
                if (vr.IsValid)
                {
                    return this.Ok(new ApiResponse(true, vr));
                }
                else
                {
                    return this.BadRequest(new ApiResponse(false, vr));
                }
            }
            catch (System.Exception)
            {
                return this.UniqueCodeValidationError();
            }
        }

        /// <summary>
        /// Update an existing ExternalSystem.
        /// </summary>
        /// <param name="externalsystem">
        /// The lhExtSysSvc.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut("{externalsystem}")]
        public async Task<IActionResult> PutAsync([FromBody] ExternalSystem externalsystem)
        {
            try
            {
                var vr = await this.lhExtSysSvc.UpdateAsync(this.CurrentUserId, externalsystem);
                if (vr.IsValid)
                {
                    return this.Ok(new ApiResponse(true, vr));
                }
                else
                {
                    return this.BadRequest(new ApiResponse(false, vr));
                }
            }
            catch (System.Exception ex)
            {
                return this.UniqueCodeValidationError();
            }
        }

        /// <summary>
        /// Deletes a lhExtSysSvc.
        /// </summary>
        /// <param name="id">The lhExtSysSvc id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var externalSystem = await this.lhExtSysSvc.GetExtSystemById(id);
            externalSystem.CreateUser = null;
            externalSystem.AmendUser = null;
            externalSystem.Deleted = true;

            await this.lhExtSysSvc.UpdateAsync(this.CurrentUserId, externalSystem);

            return this.Ok(new ApiResponse(true));
        }

        private IActionResult UniqueCodeValidationError()
        {
            LearningHubValidationResult validationResult = new LearningHubValidationResult();
            validationResult.CreatedId = -1;
            validationResult.IsValid = false;
            validationResult.Details = new List<string>();
            validationResult.Details.Add("Entered Code value already exists. Please re-enter a unique Code.");
            return this.Conflict(new ApiResponse(true, validationResult));
        }
    }
}