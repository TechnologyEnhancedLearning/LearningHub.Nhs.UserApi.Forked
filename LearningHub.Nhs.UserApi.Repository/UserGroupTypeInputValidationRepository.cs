// <copyright file="UserGroupTypeInputValidationRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user group type input validation repository.
    /// </summary>
    public class UserGroupTypeInputValidationRepository : GenericElfhRepository<UserGroupTypeInputValidation>, IUserGroupTypeInputValidationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroupTypeInputValidationRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserGroupTypeInputValidationRepository(ElfhHubDbContext dbContext, ILogger<UserGroupTypeInputValidation> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public bool IsEmailValidForUserGroup(string emailAddress, int userGroupId)
        {
            return this.DbContext.UserGroupTypeInputValidation.AsNoTracking().Any(
                                                                x => x.UserGroupId == userGroupId &&
                                                                EF.Functions.Like(emailAddress, x.ValidationTextValue.Replace("*", "%")));
        }
    }
}
