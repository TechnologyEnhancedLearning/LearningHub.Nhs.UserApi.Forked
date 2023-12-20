// <copyright file="MedicalCouncilService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;

    /// <summary>
    /// The medical council service.
    /// </summary>
    public class MedicalCouncilService : IMedicalCouncilService
    {
        private readonly IGmcLrmpRepository gmcLrmpRepository;
        private readonly IGdcRegisterRepository gdcRegisterRepository;
        private readonly IMedicalCouncilRepository medicalCouncilRepository;
        private readonly IJobRoleRepository jobRoleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalCouncilService"/> class.
        /// </summary>
        /// <param name="gmcLrmpRepository">The gmc lrmp repository.</param>
        /// <param name="gdcRegisterRepository">The gdc register repository.</param>
        /// <param name="medicalCouncilRepository">The medical council repository.</param>
        /// <param name="jobRoleRepository">The job role repository.</param>
        public MedicalCouncilService(
            IGmcLrmpRepository gmcLrmpRepository,
            IGdcRegisterRepository gdcRegisterRepository,
            IMedicalCouncilRepository medicalCouncilRepository,
            IJobRoleRepository jobRoleRepository)
        {
            this.gmcLrmpRepository = gmcLrmpRepository;
            this.gdcRegisterRepository = gdcRegisterRepository;
            this.medicalCouncilRepository = medicalCouncilRepository;
            this.jobRoleRepository = jobRoleRepository;
        }

        /// <inheritdoc/>
        public async Task<MedicalCouncil> GetByJobRoleIdAsync(int jobRoleId)
        {
            MedicalCouncil medicalCouncil = null;

            JobRole jobRole = await this.jobRoleRepository.GetByIdAsync(jobRoleId);

            if (jobRole.MedicalCouncilId.HasValue)
            {
                medicalCouncil = await this.medicalCouncilRepository.GetByIdAsync(jobRole.MedicalCouncilId.Value);
            }

            return medicalCouncil;
        }

        /// <inheritdoc/>
        public async Task<string> ValidateGDCNumber(string lastname, string medicalCouncilNumber)
        {
            string errorMessage = string.Empty;
            var gdc = await this.gdcRegisterRepository.GetByLastNameAndGDCNumber(lastname, medicalCouncilNumber);

            if (gdc == null)
            {
                errorMessage = "These details do not match the records held by the GDC. Please ensure that the surname and GMC number entered are the same as the details held on the GDC register.";
            }

            return errorMessage;
        }

        /// <inheritdoc/>
        public async Task<string> ValidateGMCNumber(string lastname, string medicalCouncilNumber, int? yearOfQualification, string firstname)
        {
            string errorMessage = string.Empty;
            string status = string.Empty;

            var gmc = await this.gmcLrmpRepository.GetByLastNameAndGMCNumber(lastname, medicalCouncilNumber);

            if (gmc != null && yearOfQualification == null)
            {
                status = gmc.RegistrationStatus;
            }
            else if (gmc != null)
            {
                // If the yearOfQualification has been requested/supplied ensure FirstName, LastName and yearOfQualification match
                if (lastname == gmc.Surname
                    &&
                        gmc.GmcRefNo == medicalCouncilNumber
                    &&
                        ((gmc.GivenName.Contains(" ") && gmc.GivenName.StartsWith(firstname))
                            ||
                            (!gmc.GivenName.Contains(" ") && gmc.GivenName == firstname)))
                {
                    status = gmc.RegistrationStatus;
                }
            }

            if (string.IsNullOrEmpty(status))
            {
                errorMessage = "These details do not match the records held by the GMC. Please ensure that the surname and GMC number entered are the same as the details held on the GMC register.";
            }
            else if (status.ToUpper() != "REGISTERED WITH LICENCE"
                    && status.ToUpper() != "REGISTERED WITHOUT A LICENCE"
                    && status.ToUpper() != "PROVISIONALLY REGISTERED WITH LICENCE"
                    && status.ToUpper() != "PROVISIONALLY REGISTERED WITHOUT A LICENCE")
            {
                errorMessage = "You do not appear to have a valid GMC registration status " + status;
            }

            return errorMessage;
        }

        /// <inheritdoc/>
        public string ValidateNMCNumber(string medicalCouncilNumber)
        {
            if (medicalCouncilNumber != null &&
                Regex.IsMatch(medicalCouncilNumber, "^[0-9]{2}[A-Za-z][0-9]{4}[A-Za-z]$"))
            {
                return string.Empty;
            }

            return "Invalid Nursing and Midwifery Council Number!";
        }
    }
}
