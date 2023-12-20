// <copyright file="LearningHubClientStore.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Security.Policy;
    using System.Threading.Tasks;
    using IdentityServer4;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The LearningHubClientStore class.
    /// </summary>
    public class LearningHubClientStore : IClientStore
    {
        private readonly WebSettings webSettings;
        private readonly LearningHubAuthConfig learningHubAuthConfig;
        private readonly IEnumerable<Client> clients;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubClientStore"/> class.
        /// </summary>
        /// <param name="configuration">The Configuration.</param>
        public LearningHubClientStore(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.webSettings = configuration.GetSection(nameof(WebSettings)).Get<WebSettings>();
            this.learningHubAuthConfig = configuration.GetSection(nameof(LearningHubAuthConfig)).Get<LearningHubAuthConfig>();

            this.clients = this.BuildClients();
        }

        /// <summary>
        /// Finds a client by id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns>The client.</returns>
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.Run(() => this.clients.SingleOrDefault(c => c.ClientId == clientId));
        }

        private List<Client> BuildClients()
        {
            var clientList = new List<Client>();
            clientList.AddRange(this.GetClients());
            clientList.AddRange(this.GetElfhClients(clientList));
            return clientList;
        }

        private ICollection<Client> GetElfhClients(List<Client> initialClients)
        {
            var tenantList = new List<Client>();

            foreach (var tenantItem in this.learningHubAuthConfig.IdsClients)
            {
                if (!initialClients.Exists(c => c.ClientId == tenantItem.Key))
                {
                    var scopes = new List<string>
                                {
                                    IdentityServerConstants.StandardScopes.OpenId,
                                    IdentityServerConstants.StandardScopes.Profile,
                                    "roles",
                                };

                    if (tenantItem.Value.Scopes.Any())
                    {
                        scopes.AddRange(tenantItem.Value.Scopes);
                    }

                    var client = new Client
                    {
                        ClientId = tenantItem.Key,
                        ClientName = tenantItem.Value.ClientDescription,
                        ClientSecrets = new List<Secret> { new Secret(tenantItem.Value.AuthSecret.Sha256()) },
                        AllowOfflineAccess = true,
                        AllowedGrantTypes = GrantTypes.Code,
                        RedirectUris = tenantItem.Value.RedirectUris,
                        PostLogoutRedirectUris = tenantItem.Value.PostLogoutUris,
                        RequireConsent = false,
                        UserSsoLifetime = this.learningHubAuthConfig.AuthTimeout * 60,
                        AllowedScopes = scopes,
                        BackChannelLogoutSessionRequired = true,
                        BackChannelLogoutUri = $"https://{tenantItem.Value.ClientUrl}/LogoutBackChannel",
                    };

                    tenantList.Add(client);
                }
            }

            return tenantList;
        }

        private ICollection<Client> GetClients()
        {
            var authClients = this.learningHubAuthConfig.AuthClients;

            var clientList = new List<Client>();

            foreach (var clientApp in authClients)
            {
                if (!string.IsNullOrEmpty(clientApp.Value.ClientName) && !string.IsNullOrEmpty(clientApp.Value.ClientSecret))
                {
                    var baseUrl = clientApp.Value.BaseUrl.EndsWith("/") ? clientApp.Value.BaseUrl.Remove(clientApp.Value.BaseUrl.Length - 1) : clientApp.Value.BaseUrl;

                    var client = new Client
                    {
                        ClientId = clientApp.Key,
                        ClientName = clientApp.Value.ClientName,
                        ClientSecrets = new List<Secret> { new Secret(clientApp.Value.ClientSecret.Sha256()) },
                        AllowedGrantTypes = clientApp.Value.AllowedGrantTypes,
                        RedirectUris = this.AddBaseUrls(baseUrl, clientApp.Value.RedirectUris),
                        PostLogoutRedirectUris = this.AddBaseUrls(baseUrl, clientApp.Value.PostLogoutUris),
                        UserSsoLifetime = this.learningHubAuthConfig.AuthTimeout * 60,
                        AllowedScopes = clientApp.Value.AllowedScopes,
                        BackChannelLogoutSessionRequired = clientApp.Value.BackChannelLogoutSessionRequired,
                        BackChannelLogoutUri = this.AddBaseUrl(baseUrl, clientApp.Value.BackChannelLogoutUri),
                        UpdateAccessTokenClaimsOnRefresh = clientApp.Value.UpdateAccessTokenClaimsOnRefresh,
                        RequireConsent = clientApp.Value.RequireConsent,
                        RequirePkce = clientApp.Value.RequirePkce,
                        AllowOfflineAccess = clientApp.Value.AllowOfflineAccess,
                    };

                    clientList.Add(client);
                }
            }

            return clientList;
        }

        private ICollection<string> AddBaseUrls(string baseUrl, ICollection<string> uris)
        {
            if (uris == null || uris.Count() == 0)
            {
                return new Collection<string>();
            }

            var outputUrls = new Collection<string>();

            foreach (var uri in uris)
            {
                outputUrls.Add(this.AddBaseUrl(baseUrl, uri));
            }

            return outputUrls;
        }

        private string AddBaseUrl(string baseUrl, string uri)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                return uri;
            }

            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            string returnUrl;
            if (uri.StartsWith("http"))
            {
                returnUrl = uri;
            }
            else
            {
                returnUrl = baseUrl + (uri.StartsWith("/") ? uri : uri.Substring(1));
            }

            return returnUrl;
        }
    }
}