// <copyright file="CountryRepository.cs" company="HEE.nhs.uk">
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
    /// The country repository.
    /// </summary>
    public class CountryRepository : GenericElfhRepository<Country>, ICountryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public CountryRepository(ElfhHubDbContext dbContext, ILogger<Country> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<Country> GetByIdAsync(int id)
        {
            return await this.DbContext.Country.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }
    }
}
