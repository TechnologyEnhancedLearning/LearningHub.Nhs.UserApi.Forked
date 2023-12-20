// <copyright file="RegionRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The region repository.
    /// </summary>
    public class RegionRepository : GenericElfhRepository<Region>, IRegionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public RegionRepository(ElfhHubDbContext dbContext, ILogger<Region> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<Region> GetByIdAsync(int id)
        {
            return await this.DbContext.Region.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
