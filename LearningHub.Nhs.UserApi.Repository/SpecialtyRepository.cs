// <copyright file="SpecialtyRepository.cs" company="HEE.nhs.uk">
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
    /// The specialty repository.
    /// </summary>
    public class SpecialtyRepository : GenericElfhRepository<Specialty>, ISpecialtyRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SpecialtyRepository(ElfhHubDbContext dbContext, ILogger<Specialty> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<Specialty> GetByIdAsync(int id)
        {
            return await this.DbContext.Specialty.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
