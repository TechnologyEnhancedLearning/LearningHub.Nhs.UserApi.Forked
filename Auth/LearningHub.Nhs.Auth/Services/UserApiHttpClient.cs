namespace LearningHub.Nhs.Auth.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Interfaces;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The user api http client.
    /// </summary>
    public class UserApiHttpClient : IUserApiHttpClient
    {
        /// <summary>
        /// The Http client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiHttpClient"/> class.
        /// </summary>
        /// <param name="webSettings">
        /// The web settings.
        /// </param>
        /// <param name="lhAuthConf">
        /// The lh Auth Config.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        public UserApiHttpClient(IOptions<WebSettings> webSettings, IOptions<LearningHubAuthConfig> lhAuthConf, HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (webSettings == null)
            {
                throw new ArgumentNullException(nameof(webSettings));
            }

            if (lhAuthConf == null)
            {
                throw new ArgumentNullException(nameof(lhAuthConf));
            }

            client.BaseAddress = new Uri(webSettings.Value.UserApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Client-Identity-Key", lhAuthConf.Value.AuthClientIdentityKey.ToString());
            this.client = client;
        }

        /// <summary>
        /// The get client.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpClient"/>.
        /// </returns>
        public HttpClient GetClient()
        {
            return this.client;
        }
    }
}
