// <copyright file="MedicalCouncilRepository.cs" company="HEE.nhs.uk">
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
    /// The medical council repository.
    /// </summary>
    public class MedicalCouncilRepository : GenericElfhRepository<MedicalCouncil>, IMedicalCouncilRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalCouncilRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public MedicalCouncilRepository(ElfhHubDbContext dbContext, ILogger<MedicalCouncil> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<MedicalCouncil> GetByIdAsync(int id)
        {
            return await this.DbContext.MedicalCouncil.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
