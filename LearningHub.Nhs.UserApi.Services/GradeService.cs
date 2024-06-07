namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The grade service.
    /// </summary>
    public class GradeService : IGradeService
    {
        /// <summary>
        /// The grade repository.
        /// </summary>
        private readonly IGradeRepository gradeRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<GradeService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeService"/> class.
        /// </summary>
        /// <param name="gradeRepository">
        /// The grade repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public GradeService(IGradeRepository gradeRepository, ILogger<GradeService> logger)
        {
            this.gradeRepository = gradeRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Grade> GetByIdAsync(int id)
        {
            var grade = await this.gradeRepository.GetByIdAsync(id);

            return grade;
        }

        /// <inheritdoc/>
        public List<Grade> GetAll()
        {
            var gradeList = this.gradeRepository.GetAll()
                .OrderBy(r => r.DisplayOrder)
                .ToListWithNoLock();

            return gradeList;
        }

        /// <inheritdoc/>
        public async Task<List<Grade>> GetByJobRole(int jobRoleId)
        {
            var gradeList = this.gradeRepository.GetByJobRole(jobRoleId).OrderBy(r => r.DisplayOrder);

            return await gradeList.ToListWithNoLockAsync();
        }
    }
}