// <copyright file="UserService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Enums;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.Models;
    using LearningHub.Nhs.Models.Common;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using UAParser;

    /// <summary>
    /// The user service.
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userApiHttpClient">
        /// The user api http client.
        /// </param>
        public UserService(IUserApiHttpClient userApiHttpClient)
        {
            this.UserApiHttpClient = userApiHttpClient;
        }

        /// <inheritdoc/>
        public async Task<UserBasicViewModel> GetBasicUserByUserIdAsync(string subjectId)
        {
            UserBasicViewModel viewmodel = null;

            var client = this.UserApiHttpClient.GetClient();
            var request = $"ElfhUser/GetBasicByUserId/{subjectId}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserBasicViewModel>(result);
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<UserViewModel> GetUserByUserIdAsync(string id)
        {
            UserViewModel viewmodel = null;

            var client = this.UserApiHttpClient.GetClient();
            var request = $"ElfhUser/GetByUserId/{id}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserViewModel>(result);
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<UserBasicViewModel> GetUserByUserNameAsync(string username)
        {
            UserBasicViewModel viewmodel = null;

            var client = this.UserApiHttpClient.GetClient();
            var request = $"ElfhUser/GetByUsername/{username}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserBasicViewModel>(result);
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<int> GetUserIdByUserNameAsync(string username)
        {
            var client = this.UserApiHttpClient.GetClient();

            // In asp.net " " (space) in the url PATH segment is considered invalid url,
            // it has to be encoded properly, "HttpUtility.UrlEncode" does not encode space character correctly
            // in the url PATH, below added fix for that
            var request = $"ElfhUser/GetUserIdByUsername/{HttpUtility.UrlEncode(username).Replace("+", "%20")}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var userId = int.Parse(await response.Content.ReadAsStringAsync());
                return userId;
            }
            else
            {
                throw new Exception("Invalid username!");
            }
        }

        /// <inheritdoc/>
        public async Task<UserBasic> GetUserByOAUserIdAsync(string oaUserId)
        {
            UserBasic viewmodel = null;

            var client = this.UserApiHttpClient.GetClient();
            var request = $"ElfhUser/GetByOpenAthensId/{oaUserId}";
            var response = await client.GetAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                viewmodel = JsonConvert.DeserializeObject<UserBasic>(result);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized
                        ||
                     response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("AccessDenied");
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<LoginResult> AuthenticateUserAsync(string username, string password)
        {
            LoginResult viewmodel = null;

            var client = this.UserApiHttpClient.GetClient();

            var request = "Authentication/Authenticate";

            var login = new { Username = username, Password = password };
            using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8))
            {
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(request, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    viewmodel = JsonConvert.DeserializeObject<LoginResult>(result);
                }
            }

            return viewmodel;
        }

        /// <inheritdoc/>
        public async Task<LoginResultInternal> AuthenticateSsoUserAsync(int userId, int externalSystemId, string clientCode)
        {
            LoginResultInternal vm = null;

            var client = this.UserApiHttpClient.GetClient();

            var login = new { userId, externalSystemId, clientCode };

            using (var conetnt = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"))
            {
                var response = await client.PostAsync("authentication/authenticate-sso", conetnt).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    vm = JsonConvert.DeserializeObject<LoginResultInternal>(result);
                }
            }

            return vm;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserRoleAsync(int id)
        {
            string roleName = string.Empty;

            var client = this.UserApiHttpClient.GetClient();
            var request = $"ElfhUser/GetUserRoleName/{id}";
            var response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                roleName = JsonConvert.DeserializeObject<string>(result);
            }

            return roleName;
        }

        /// <inheritdoc/>
        public async Task AddLogonToUserHistory(string detail, int userId, UserHistoryType userHistoryType, bool loginSuccessFull, HttpRequest request, string externalReferer)
        {
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(request.Headers["User-Agent"]);
            var referer = externalReferer ?? string.Empty;
            var userHistory = new UserHistoryViewModel
            {
                UserId = userId,
                UserHistoryTypeId = (int)userHistoryType,
                Detail = detail,
                UserAgent = request.Headers["User-Agent"],
                BrowserName = clientInfo.UA.Family,
                BrowserVersion = clientInfo.UA.Major + "." + clientInfo.UA.Minor + "." + clientInfo.UA.Patch,
                UrlReferer = referer.Length < 1000 ? referer : referer.Substring(0, 999),
                LoginSuccessFul = loginSuccessFull,
                LoginIP = request.HttpContext.Connection.RemoteIpAddress.ToString(),
            };

            await this.StoreUserHistoryAsync(userHistory);
        }

        /// <inheritdoc/>
        public async Task StoreUserHistoryAsync(UserHistoryViewModel userHistory)
        {
            var client = this.UserApiHttpClient.GetClient();
            var request = "UserHistory";

            using HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(userHistory), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(request, httpContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                if (!apiResponse.Success)
                {
                    throw new Exception("Failed to store UserHistory: " + JsonConvert.SerializeObject(userHistory));
                }
            }
        }
    }
}
