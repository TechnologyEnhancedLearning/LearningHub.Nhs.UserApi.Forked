// <copyright file="IMedicalCouncilService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The MedicalCouncilService interface.
    /// </summary>
    public interface IMedicalCouncilService
    {
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
        Task<string> ValidateGMCNumber(string lastname, string medicalCouncilNumber, int? yearOfQualification, string firstname);

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
        Task<string> ValidateGDCNumber(string lastname, string medicalCouncilNumber);

        /// <summary>
        /// The validate nmc number.
        /// </summary>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string ValidateNMCNumber(string medicalCouncilNumber);

        /// <summary>
        /// Get medical council by jobRoleId.
        /// </summary>
        /// <param name="jobRoleId">The jobRoleId.</param>
        /// <returns>The medical council.</returns>
        Task<MedicalCouncil> GetByJobRoleIdAsync(int jobRoleId);
    }
}
