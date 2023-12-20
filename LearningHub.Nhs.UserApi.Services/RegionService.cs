// <copyright file="RegionService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The region service.
    /// </summary>
    public class RegionService : IRegionService
    {
        /// <summary>
        /// The region repository.
        /// </summary>
        private readonly IRegionRepository regionRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<RegionService> logger;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionService"/> class.
        /// </summary>
        /// <param name="regionRepository">
        /// The region repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public RegionService(IRegionRepository regionRepository, ILogger<RegionService> logger, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Region> GetByIdAsync(int id)
        {
            var region = await this.regionRepository.GetByIdAsync(id);

            return region;
        }

        /// <inheritdoc/>
        public async Task<List<GenericListViewModel>> GetAllAsync()
        {
            var items = this.regionRepository.GetAll()
                .OrderBy(r => r.DisplayOrder);

            var regionList = await this.mapper.ProjectTo<GenericListViewModel>(items).ToListWithNoLockAsync();

            return regionList;
        }
    }
}
