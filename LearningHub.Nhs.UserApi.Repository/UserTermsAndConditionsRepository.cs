// <copyright file="UserTermsAndConditionsRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user terms and conditions repository.
    /// </summary>
    public class UserTermsAndConditionsRepository : GenericElfhRepository<UserTermsAndConditions>, IUserTermsAndConditionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTermsAndConditionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserTermsAndConditionsRepository(ElfhHubDbContext dbContext, ILogger<UserTermsAndConditions> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public override async Task<int> CreateAsync(int userId, UserTermsAndConditions userTermsAndConditions)
        {
            userTermsAndConditions.AcceptanceDate = DateTimeOffset.Now;
            return await base.CreateAsync(userId, userTermsAndConditions);
        }
    }
}
