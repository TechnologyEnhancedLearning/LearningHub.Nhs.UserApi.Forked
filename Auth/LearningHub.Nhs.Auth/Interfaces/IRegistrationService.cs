namespace LearningHub.Nhs.Auth.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Auth.ViewModels.Sso;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The registration info service interface.
    /// </summary>
    public interface IRegistrationService
    {
        /// <summary>
        /// Get a list of job roles by staff group id.
        /// </summary>
        /// <param name="staffGroupId">The staffGroupId.</param>
        /// <returns>A list of job roles.</returns>
        Task<List<JobRoleBasicViewModel>> GetByStaffGroupIdAsync(int staffGroupId);

        /// <summary>
        /// Gets a MedicalCouncil by jobRoleId.
        /// </summary>
        /// <param name="jobRoleId">The jobRoleId.</param>
        /// <returns>A MedicalCouncil.</returns>
        Task<MedicalCouncil> GetMedicalCouncilByJobRoleIdAsync(int jobRoleId);

        /// <summary>
        /// Gets a list of grades for a specific job role.
        /// </summary>
        /// <param name="jobRoleId">The job role id.</param>
        /// <returns>List of grades.</returns>
        Task<List<GenericListViewModel>> GetGradesForJobRoleAsync(int jobRoleId);

        /// <summary>
        /// Get a list of all specialities.
        /// </summary>
        /// <returns>A list of all specialities.</returns>
        Task<List<GenericListViewModel>> GetSpecialtiesAsync();

        /// <summary>
        /// Gets a list of all staff groups.
        /// </summary>
        /// <returns>List of staff groups.</returns>
        Task<List<StaffGroup>> GetStaffGroupsAsync();

        /// <summary>
        /// Get a list of locations by search term.
        /// </summary>
        /// <param name="criteria">Location search term.</param>
        /// <returns>List of locations.</returns>
        Task<List<LocationBasicViewModel>> LocationSearchAsync(string criteria);

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">The registration request.</param>
        /// <param name="clientId">The client id.</param>
        /// <returns>LearningHubValidationResult.</returns>
        Task<LearningHubValidationResult> RegisterUser(RegisterUserViewModel request, int clientId);

        /// <summary>
        /// Link an existing user.
        /// </summary>
        /// <param name="username">Existig user username.</param>
        /// <param name="password">Existig user Password.</param>
        /// <param name="clientId">External client id.</param>
        /// <param name="clientCode">The client code.</param>
        /// <returns>LearningHubValidationResult.</returns>
        Task<LoginResultInternal> LinkUserToSso(string username, string password, int clientId, string clientCode);
    }
}