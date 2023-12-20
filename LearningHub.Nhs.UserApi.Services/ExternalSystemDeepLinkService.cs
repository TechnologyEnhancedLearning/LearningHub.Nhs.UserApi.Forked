// <copyright file="ExternalSystemDeepLinkService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.UserApi.Repository.Interface.LH;
    using LearningHub.Nhs.UserApi.Services.Interface;

    /// <summary>
    /// The external system deeplink service.
    /// </summary>
    public class ExternalSystemDeepLinkService : IExternalSystemDeepLinkService
    {
        private readonly IExternalSystemDeepLinkRepository extSystemDeepLinkRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemDeepLinkService"/> class.
        /// </summary>
        /// <param name="extSystemDeepLinkRepo">The external system deep link repository.</param>
        public ExternalSystemDeepLinkService(IExternalSystemDeepLinkRepository extSystemDeepLinkRepo)
        {
            this.extSystemDeepLinkRepo = extSystemDeepLinkRepo;
        }

        /// <inheritdoc/>
        public async Task<ExternalSystemDeepLink> GetByCodeAsync(string code)
        {
            return await this.extSystemDeepLinkRepo.GetByCodeAsync(code);
        }
    }
}