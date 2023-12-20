// <copyright file="LocationService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The location service.
    /// </summary>
    public class LocationService : ILocationService
    {
        /// <summary>
        /// The location repository.
        /// </summary>
        private readonly ILocationRepository locationRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<LocationService> logger;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="locationRepository">
        /// The location repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public LocationService(ILocationRepository locationRepository, ILogger<LocationService> logger, IMapper mapper)
        {
            this.locationRepository = locationRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<LocationBasicViewModel>> GetBySearchCriteriaAsync(string searchTerm, bool showArchived, int maxRecords)
        {
            var locations = this.locationRepository.GetAllWithType()
                .Where(l => (showArchived || l.Active)
                            && (
                                l.Name.Contains(searchTerm)
                                || l.SubName.Contains(searchTerm)
                                || l.Address1.Contains(searchTerm)
                                || l.PostCode.Contains(searchTerm)
                                || l.NhsCode.Contains(searchTerm)))
                .OrderBy(l => l.Name)
                .ThenBy(l => l.SubName)
                .ThenBy(l => l.Address1)
                .ThenBy(l => l.Address2)
                .ThenBy(l => l.Address3)
                .ThenBy(l => l.Address4)
                .ThenBy(l => l.County)
                .ThenBy(l => l.PostCode)
                .Take(maxRecords);

            var locationList = await this.mapper.ProjectTo<LocationBasicViewModel>(locations).ToListWithNoLockAsync();

            return locationList;
        }

        /// <inheritdoc/>
        public async Task<LocationBasicViewModel> GetByIdAsync(int id)
        {
            var location = await this.locationRepository.GetByIdAsync(id);

            return this.mapper.Map<LocationBasicViewModel>(location);
        }
    }
}
