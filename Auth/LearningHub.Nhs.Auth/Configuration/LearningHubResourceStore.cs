namespace LearningHub.Nhs.Auth.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;

    /// <summary>
    /// The LearningHubResourceStore class.
    /// </summary>
    public class LearningHubResourceStore : IResourceStore
    {
        private readonly IEnumerable<IdentityResource> identityResources;
        private readonly IEnumerable<ApiResource> apiResources;
        private readonly IEnumerable<ApiScope> apiScopes;
        private readonly Resources resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningHubResourceStore"/> class.
        /// </summary>
        public LearningHubResourceStore()
        {
            this.apiResources = new List<ApiResource>
            {
                new ApiResource("learninghubapi", "Learning Hub API") { Scopes = new List<string> { "learninghubapi", }, },
                new ApiResource("userapi", "User API") { Scopes = new List<string> { "userapi" } },
                new ApiResource("learningcredentialsapi", "Learning Credentials API") { Scopes = new List<string> { "learningcredentialsapi" } },
            };

            this.apiScopes = new[]
            {
                new ApiScope("learninghubapi", "Access Learning Hub API"),
                new ApiScope("userapi", "Access User API"),
                new ApiScope("learningcredentialsapi", "Learning Credentials API"),
            };

            this.identityResources = new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "roles", // scope name
                    "Your role(s)",
                    new List<string> { "role" }), // claims
                new IdentityResource
                    {
                        Name = "thirdpartyclient",
                        DisplayName = "Request authorisation to access data",
                        Description = "Scopes for Clients outside Learning Hub",
                        Required = true,
                        UserClaims = new List<string> { "requestaccessdata" },
                    },
                new IdentityResource("openathens", new[] { "openAthensUser" }),
            };

            this.resources = new Resources(this.identityResources, this.apiResources, this.apiScopes);
        }

        /// <summary>
        /// Gets API resources by API resource name.
        /// </summary>
        /// <param name="apiResourceNames">API resource names.</param>
        /// <returns>API resources.</returns>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return Task.Run(() => this.apiResources.Where(ar => apiResourceNames.Contains(ar.Name)));
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        /// <param name="scopeNames">Scope names.</param>
        /// <returns>API resources.</returns>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.Run(() => this.apiResources.Where(ar => ar.Scopes.Intersect(scopeNames).Any()));
        }

        /// <summary>
        /// Gets API scopes by scope name.
        /// </summary>
        /// <param name="scopeNames">Scope names.</param>
        /// <returns>API scopes.</returns>
        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.Run(() => this.apiScopes.Where(s => scopeNames.Contains(s.Name)));
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        /// <param name="scopeNames">Scope names.</param>
        /// <returns>Identity resources.</returns>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.Run(() => this.identityResources.Where(ir => scopeNames.Contains(ir.Name)));
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns>All resources.</returns>
        public Task<Resources> GetAllResourcesAsync()
        {
            return Task.Run(() => this.resources);
        }
    }
}