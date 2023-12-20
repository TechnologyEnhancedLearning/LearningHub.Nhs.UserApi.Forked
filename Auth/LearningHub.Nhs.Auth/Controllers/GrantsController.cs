// <copyright file="GrantsController.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Extensions;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using LearningHub.Nhs.Auth.Filters;
    using LearningHub.Nhs.Auth.Models.Grants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The grants controller.
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class GrantsController : Controller
    {
        /// <summary>
        /// The interaction.
        /// </summary>
        private readonly IIdentityServerInteractionService interaction;

        /// <summary>
        /// The clients.
        /// </summary>
        private readonly IClientStore clients;

        /// <summary>
        /// The resources.
        /// </summary>
        private readonly IResourceStore resources;

        /// <summary>
        /// The events.
        /// </summary>
        private readonly IEventService events;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantsController"/> class.
        /// </summary>
        /// <param name="interaction">
        /// The interaction.
        /// </param>
        /// <param name="clients">
        /// The clients.
        /// </param>
        /// <param name="resources">
        /// The resources.
        /// </param>
        /// <param name="events">
        /// The events.
        /// </param>
        public GrantsController(
            IIdentityServerInteractionService interaction,
            IClientStore clients,
            IResourceStore resources,
            IEventService events)
        {
            this.interaction = interaction;
            this.clients = clients;
            this.resources = resources;
            this.events = events;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return this.View("Index", await this.BuildViewModelAsync());
        }

        /// <summary>
        /// The revoke.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await this.interaction.RevokeUserConsentAsync(clientId);
            await this.events.RaiseAsync(new GrantsRevokedEvent(this.User.GetSubjectId(), clientId));

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// The build view model async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<GrantsViewModel> BuildViewModelAsync()
        {
            var grants = await this.interaction.GetAllUserGrantsAsync();

            var list = new List<GrantViewModel>();
            foreach (var grant in grants)
            {
                var client = await this.clients.FindClientByIdAsync(grant.ClientId);
                if (client != null)
                {
                    var resourcesByScope = await this.resources.FindResourcesByScopeAsync(grant.Scopes);

                    var item = new GrantViewModel()
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resourcesByScope.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resourcesByScope.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                    };

                    list.Add(item);
                }
            }

            return new GrantsViewModel
            {
                Grants = list,
            };
        }
    }
}