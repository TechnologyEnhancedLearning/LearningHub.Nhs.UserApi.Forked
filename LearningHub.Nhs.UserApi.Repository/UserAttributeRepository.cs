// <copyright file="UserAttributeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The attribute types.
    /// </summary>
    internal enum AttributeTypes
    {
        /// <summary>
        /// The open athens user id.
        /// </summary>
        OpenAthensUserId = 38,

        /// <summary>
        /// The open athens org id.
        /// </summary>
        OpenAthensOrgId = 39,
    }

    /// <summary>
    /// The user attribute repository.
    /// </summary>
    public class UserAttributeRepository : GenericElfhRepository<UserAttribute>, IUserAttributeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAttributeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UserAttributeRepository(ElfhHubDbContext dbContext, ILogger<UserAttribute> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> LinkOpenAthensAccountToElfhUser(OpenAthensToElfhUserLinkDetails linkDetails)
        {
            var existingExtRefs = this.DbContext.UserAttribute
                .Where(w => w.UserId == linkDetails.UserId && w.Deleted == false &&
                            (w.AttributeId == (int)AttributeTypes.OpenAthensUserId || w.AttributeId == (int)AttributeTypes.OpenAthensOrgId));

            if (await existingExtRefs.AnyAsync())
            {
                foreach (var existingExtRef in existingExtRefs)
                {
                    existingExtRef.Deleted = true;
                    await this.UpdateAsync(linkDetails.UserId, existingExtRef);
                }
            }

            var newExtRefOaUserId = new UserAttribute
            {
                UserId = linkDetails.UserId,
                AttributeId = (int)AttributeTypes.OpenAthensUserId,
                Deleted = false,
                TextValue = linkDetails.OaUserId,
            };

            var newExtRefOaOrgId = new UserAttribute
            {
                UserId = linkDetails.UserId,
                AttributeId = (int)AttributeTypes.OpenAthensOrgId,
                Deleted = false,
                TextValue = linkDetails.OaOrgId,
            };

            var oaUserIdCreate = await this.CreateAsync(linkDetails.UserId, newExtRefOaUserId);
            var oaOrgIdCreate = await this.CreateAsync(linkDetails.UserId, newExtRefOaOrgId);

            return oaOrgIdCreate > 0 && oaUserIdCreate > 0;
        }
    }
}
