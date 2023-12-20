// <copyright file="GradeRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The grade repository.
    /// </summary>
    public class GradeRepository : GenericElfhRepository<Grade>, IGradeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GradeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">
        /// The db context.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public GradeRepository(ElfhHubDbContext dbContext, ILogger<Grade> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<Grade> GetByIdAsync(int id)
        {
            return await this.DbContext.Grade.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public IQueryable<Grade> GetByJobRole(int jobRoleId)
        {
            return this.DbContext.Set<JobRoleGrade>()
                .Include(jrg => jrg.Grade)
                .Where(jrg => jrg.JobRoleId == jobRoleId && !jrg.Deleted).AsNoTracking()
                .Select(jrg => jrg.Grade);
        }
    }
}
