namespace LearningHub.Nhs.Auth.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.ViewModels.Sso;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Validation;
    using Newtonsoft.Json;

    /// <summary>
    /// The GradeService.
    /// </summary>
    public class RegistrationService : BaseService, IRegistrationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        /// <param name="userApiHttpClient">The user api http client.</param>
        public RegistrationService(IUserApiHttpClient userApiHttpClient)
        {
            this.UserApiHttpClient = userApiHttpClient;
        }

        /// <inheritdoc/>
        public async Task<List<StaffGroup>> GetStaffGroupsAsync()
        {
            return await this.GetAsync<List<StaffGroup>>("StaffGroup/GetAll");
        }

        /// <inheritdoc/>
        public async Task<MedicalCouncil> GetMedicalCouncilByJobRoleIdAsync(int jobRoleId)
        {
            return await this.GetAsync<MedicalCouncil>($"MedicalCouncil/GetByJobRoleId/{jobRoleId}");
        }

        /// <inheritdoc/>
        public async Task<List<JobRoleBasicViewModel>> GetByStaffGroupIdAsync(int staffGroupId)
        {
            return await this.GetAsync<List<JobRoleBasicViewModel>>($"JobRole/GetByStaffGroupId/{staffGroupId}");
        }

        /// <inheritdoc/>
        public async Task<List<GenericListViewModel>> GetGradesForJobRoleAsync(int jobRoleId)
        {
            return await this.GetAsync<List<GenericListViewModel>>($"Grade/GetByJobRole/{jobRoleId}");
        }

        /// <inheritdoc/>
        public async Task<List<GenericListViewModel>> GetSpecialtiesAsync()
        {
            return await this.GetAsync<List<GenericListViewModel>>("Specialty/GetAll");
        }

        /// <inheritdoc/>
        public async Task<List<LocationBasicViewModel>> LocationSearchAsync(string criteria)
        {
            return await this.GetAsync<List<LocationBasicViewModel>>($"Location/GetBySearchCriteria/{criteria}");
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> RegisterUser(RegisterUserViewModel request, int clientId)
        {
            var vm = new RegistrationRequestViewModel
            {
                IsExternalUser = true,
                ExternalSystemId = clientId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                JobRoleId = request.JobRoleId.Value,
                GradeId = request.GradeId ?? 0,
                MedicalCouncilNumber = request.MedicalCouncilNumber,
                SpecialtyId = request.SpecialtyId ?? 0,
                LocationId = request.LocationId ?? 0,
                CountryId = 1,
            };

            var client = this.UserApiHttpClient.GetClient();

            using var reqContent = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("ElfhUser/RegisterUser", reqContent).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LearningHubValidationResult>(content);
        }

        /// <inheritdoc/>
        public async Task<LoginResultInternal> LinkUserToSso(string username, string password, int clientId, string clientCode)
        {
            var client = this.UserApiHttpClient.GetClient();

            var vm = new LinkUserToSsoRequestViewModel
            {
                Username = username,
                Password = password,
                ExternalSystemId = clientId,
                ExternalSystemCode = clientCode,
            };

            using var reqContent = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("ElfhUser/LinkUserToSso", reqContent).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LoginResultInternal>(content);
        }

        private async Task<T> GetAsync<T>(string request)
        {
            T obj = default(T);

            var client = this.UserApiHttpClient.GetClient();

            var response = await client.GetAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                obj = JsonConvert.DeserializeObject<T>(result);
            }

            return obj;
        }
    }
}