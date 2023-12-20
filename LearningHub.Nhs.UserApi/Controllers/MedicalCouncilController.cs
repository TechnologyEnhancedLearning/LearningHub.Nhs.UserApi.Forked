// <copyright file="MedicalCouncilController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The medical council controller.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalCouncilController : ControllerBase
    {
        /// <summary>
        /// The medical council service.
        /// </summary>
        private IMedicalCouncilService medicalCouncilService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalCouncilController"/> class.
        /// </summary>
        /// <param name="medicalCouncilService">
        /// The medical council service.
        /// </param>
        public MedicalCouncilController(
            IMedicalCouncilService medicalCouncilService)
        {
            this.medicalCouncilService = medicalCouncilService;
        }

        // GET api/MedicalCouncil/ValidateGMCNumber

        /// <summary>
        /// The validate gmc number.
        /// </summary>
        /// <param name="lastname">
        /// The lastname.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <param name="yearOfQualification">
        /// The year of qualification.
        /// </param>
        /// <param name="firstname">
        /// The firstname.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("ValidateGMCNumber/{lastname}/{medicalCouncilNumber}/{yearOfQualification}/{firstname}")]
        public async Task<IActionResult> ValidateGmcNumber(string lastname, string medicalCouncilNumber, int? yearOfQualification, string firstname)
        {
            var errorMessage = await this.medicalCouncilService.ValidateGMCNumber(lastname, medicalCouncilNumber, yearOfQualification, firstname);
            return this.Ok(errorMessage);
        }

        // GET api/MedicalCouncil/ValidateGMCNumber

        /// <summary>
        /// The validate gmc number.
        /// </summary>
        /// <param name="lastname">
        /// The lastname.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("ValidateGMCNumber/{lastname}/{medicalCouncilNumber}")]
        public async Task<IActionResult> ValidateGMCNumber(string lastname, string medicalCouncilNumber)
        {
            var errorMessage = await this.medicalCouncilService.ValidateGMCNumber(lastname, medicalCouncilNumber, null, string.Empty);
            return this.Ok(errorMessage);
        }

        // GET api/MedicalCouncil/ValidateGDCNumber

        /// <summary>
        /// The validate gdc number.
        /// </summary>
        /// <param name="lastname">
        /// The lastname.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("ValidateGDCNumber/{lastname}/{medicalCouncilNumber}")]
        public async Task<IActionResult> ValidateGDCNumber(string lastname, string medicalCouncilNumber)
        {
            var errorMessage = await this.medicalCouncilService.ValidateGDCNumber(lastname, medicalCouncilNumber);
            return this.Ok(errorMessage);
        }

        // GET api/MedicalCouncil/ValidateNMCNumber

        /// <summary>
        /// The validate nmc number.
        /// </summary>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("ValidateNMCNumber/{medicalCouncilNumber}")]
        public IActionResult ValidateNMCNumber(string medicalCouncilNumber)
        {
            var errorMessage = this.medicalCouncilService.ValidateNMCNumber(medicalCouncilNumber);
            return this.Ok(errorMessage);
        }

        /// <summary>
        /// Get medical council by jobRoleId.
        /// </summary>
        /// <param name="jobRoleId">The jobRoleId.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        [HttpGet]
        [Route("GetByJobRoleId/{jobRoleId}")]
        public async Task<IActionResult> GetByStaffGroupId(int jobRoleId)
        {
            var medicalCouncil = await this.medicalCouncilService.GetByJobRoleIdAsync(jobRoleId);
            return this.Ok(medicalCouncil);
        }
    }
}