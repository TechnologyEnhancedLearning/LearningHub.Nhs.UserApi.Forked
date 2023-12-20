// <copyright file="ExternalSystemUserService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;

    /// <summary>
    /// The external system user service.
    /// </summary>
    public class ExternalSystemUserService : IExternalSystemUserService
    {
        private readonly Repository.Interface.LH.IExternalSystemUserRepository extSystemUserRepo;
        private readonly IUserExternalSystemRepository userExternalSystemRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemUserService"/> class.
        /// </summary>
        /// <param name="extSystemUserRepo">The external system user repository.</param>
        /// <param name="userExternalSystemRepo">The ELFH external system user repository.</param>
        public ExternalSystemUserService(
            Repository.Interface.LH.IExternalSystemUserRepository extSystemUserRepo,
            IUserExternalSystemRepository userExternalSystemRepo)
        {
            this.extSystemUserRepo = extSystemUserRepo;
            this.userExternalSystemRepo = userExternalSystemRepo;
        }

        /// <inheritdoc/>
        public async Task<ExternalSystemUser> GetByIdAsync(int userId, int externalSystemId)
        {
            return await this.extSystemUserRepo.GetByIdAsync(userId, externalSystemId);
        }

        /// <inheritdoc/>
        public async Task<UserExternalSystem> GetElfhExternalUserByUserIdAndClientCode(int userId, string clientCode)
        {
            return await this.userExternalSystemRepo.GetUserExternalSystemByUserIdandCodeAsync(userId, clientCode);
        }
    }
}
