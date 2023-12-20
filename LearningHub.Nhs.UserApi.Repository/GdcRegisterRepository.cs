// <copyright file="GdcRegisterRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The gdc register repository.
    /// </summary>
    public class GdcRegisterRepository : IGdcRegisterRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GdcRegisterRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        public GdcRegisterRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<GdcRegister> GetByLastNameAndGDCNumber(string lastname, string medicalCouncilNumber)
        {
            return await this.DbContext.GdcRegister.AsNoTracking().FirstOrDefaultWithNoLockAsync(g => g.Surname == lastname && g.RegNumber == medicalCouncilNumber);
        }
    }
}
