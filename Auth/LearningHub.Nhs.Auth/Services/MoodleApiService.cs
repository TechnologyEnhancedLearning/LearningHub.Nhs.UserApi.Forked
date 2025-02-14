namespace LearningHub.Nhs.Auth.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// MoodleApiService.
    /// </summary>
    public class MoodleApiService : IMoodleApiService
    {
        private readonly IMoodleHttpClient moodleHttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodleApiService"/> class.
        /// </summary>
        /// <param name="moodleHttpClient">moodleHttpClient.</param>
        public MoodleApiService(IMoodleHttpClient moodleHttpClient)
        {
            this.moodleHttpClient = moodleHttpClient;
        }

        /// <summary>
        /// GetMoodleUserIdByUsernameAsync.
        /// </summary>
        /// <param name="currentUserId">current User Id.</param>
        /// <returns>UserId from Moodle.</returns>
        public async Task<int> GetMoodleUserIdByUsernameAsync(int currentUserId)
        {
            int moodleUserId = 0;
            string additionalParameters = $"&criteria[0][key]=username&criteria[0][value]={currentUserId}";
            string defaultParameters = this.moodleHttpClient.GetDefaultParameters();

            var client = await this.moodleHttpClient.GetClient();

            string url = $"&wsfunction=core_user_get_users{additionalParameters}";

            HttpResponseMessage response = await client.GetAsync("?" + defaultParameters + url);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var viewmodel = JsonConvert.DeserializeObject<MoodleUserResponseViewModel>(result);

                foreach (var user in viewmodel.Users)
                {
                    if (user.Username == currentUserId.ToString())
                    {
                        moodleUserId = user.Id;
                    }
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                       response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }

            return moodleUserId;
        }
    }
}
