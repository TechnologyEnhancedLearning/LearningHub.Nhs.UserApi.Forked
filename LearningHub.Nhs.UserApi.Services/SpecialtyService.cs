// <copyright file="SpecialtyService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The specialty service.
    /// </summary>
    public class SpecialtyService : ISpecialtyService
    {
        /// <summary>
        /// The specialty repository.
        /// </summary>
        private readonly ISpecialtyRepository specialtyRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<SpecialtyService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyService"/> class.
        /// </summary>
        /// <param name="specialtyRepository">
        /// The specialty repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SpecialtyService(ISpecialtyRepository specialtyRepository, ILogger<SpecialtyService> logger)
        {
            this.specialtyRepository = specialtyRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public List<Specialty> GetAll()
        {
            var specialtyList = this.specialtyRepository.GetAll()
                .OrderBy(r => r.DisplayOrder)
                .ToListWithNoLock();

            return specialtyList;
        }

        /// <inheritdoc/>
        public async Task<Specialty> GetByIdAsync(int id)
        {
            var specialty = await this.specialtyRepository.GetByIdAsync(id);

            return specialty;
        }
    }
}