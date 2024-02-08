namespace LearningHub.Nhs.Auth.Services
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Models.Entities.External;
    using Newtonsoft.Json;

    /// <summary>
    /// The external system service.
    /// </summary>
    public class ExternalSystemService : BaseService, IExternalSystemService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemService"/> class.
        /// </summary>
        /// <param name="userApiHttpClient">
        /// The UserApi http client.
        /// </param>
        public ExternalSystemService(IUserApiHttpClient userApiHttpClient)
        {
            this.UserApiHttpClient = userApiHttpClient;
        }

        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        public async Task<ExternalSystem> GetExternalSystem(string clientCode)
        {
            return await this.GetAsync<ExternalSystem>($"ExternalSystem/GetExternalSystem/{clientCode}");
        }

        /// <summary>
        /// Get external system deep link entity by code.
        /// </summary>
        /// <param name="code">The end client code.</param>
        /// <returns>The external system deep link.</returns>
        public async Task<ExternalSystemDeepLink> GetExternalClientDeepLink(string code)
        {
            return await this.GetAsync<ExternalSystemDeepLink>($"ExternalSystem/GetExternalSystemDeepLink/{code}");
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
