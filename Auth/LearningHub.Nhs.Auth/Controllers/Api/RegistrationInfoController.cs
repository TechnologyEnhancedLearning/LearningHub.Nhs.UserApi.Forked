namespace LearningHub.Nhs.Auth.Controllers.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Auth.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Grades API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationInfoController : Controller
    {
        private readonly IRegistrationService registrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationInfoController"/> class.
        /// </summary>
        /// <param name="registrationService">The registration info service.</param>
        public RegistrationInfoController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        /// <summary>
        /// Gets the grades for a particular job role.
        /// </summary>
        /// <param name="jobRoleId">The job role id.</param>
        /// <returns>List of grades.</returns>
        [HttpGet]
        [Route("GetGrades")]
        public async Task<JsonResult> GetByJobRole(int jobRoleId)
        {
            List<GenericListViewModel> gradeList = await this.registrationService.GetGradesForJobRoleAsync(jobRoleId);

            if (jobRoleId > 0 && gradeList.Count > 0)
            {
                return this.Json(gradeList);
            }

            return this.Json("No grades found!");
        }

        /// <summary>
        /// Gets the job roles for a particular staff group.
        /// </summary>
        /// <param name="staffGroupId">The staff group id.</param>
        /// <returns>List of job roles.</returns>
        [HttpGet]
        [Route("GetJobRoles")]
        public async Task<JsonResult> GetByStaffGroup(int staffGroupId)
        {
            List<JobRoleBasicViewModel> roleList = await this.registrationService.GetByStaffGroupIdAsync(staffGroupId);

            if (staffGroupId > 0 && roleList.Count > 0)
            {
                return this.Json(roleList);
            }

            return this.Json("No job roles found!");
        }

        /// <summary>
        /// Gets the medical council for a particular job role.
        /// </summary>
        /// <param name="jobRoleId">The jobRoleId.</param>
        /// <returns>A medical council.</returns>
        [HttpGet]
        [Route("GetMedicalCouncil")]
        public async Task<JsonResult> GetMedicalCouncilAsync(int jobRoleId)
        {
            var medicalCouncil = await this.registrationService.GetMedicalCouncilByJobRoleIdAsync(jobRoleId);

            if (medicalCouncil != null)
            {
                return this.Json(medicalCouncil);
            }

            return this.Json("No medical council found!");
        }

        /// <summary>
        /// Get a list of locations by search term.
        /// </summary>
        /// <param name="criteria">Location search term.</param>
        /// <returns>List of locations.</returns>
        [HttpGet]
        [Route("LocationBySearchCriteria/{criteria}")]
        public async Task<JsonResult> LocationSearchAsync(string criteria)
        {
            var locations = await this.registrationService.LocationSearchAsync(criteria);

            if (locations != null)
            {
                return this.Json(locations);
            }

            return this.Json("No locations found!");
        }
    }
}