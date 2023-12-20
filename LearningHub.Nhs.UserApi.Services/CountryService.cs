// <copyright file="CountryService.cs" company="HEE.nhs.uk">
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
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The country service.
    /// </summary>
    public class CountryService : ICountryService
    {
        /// <summary>
        /// The country repository.
        /// </summary>
        private readonly ICountryRepository countryRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<CountryService> logger;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryService"/> class.
        /// </summary>
        /// <param name="countryRepository">
        /// The country repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public CountryService(ICountryRepository countryRepository, ILogger<CountryService> logger, IMapper mapper)
        {
            this.countryRepository = countryRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Country> GetByIdAsync(int id)
        {
            var country = await this.countryRepository.GetByIdAsync(id);

            return country;
        }

        /// <inheritdoc/>
        public List<Country> GetAll()
        {
            var countryList = this.countryRepository.GetAll()
                .OrderBy(r => r.DisplayOrder)
                .ToListWithNoLock();

            return countryList;
        }

        /// <inheritdoc/>
        public async Task<List<GenericListViewModel>> GetFilteredAsync(string filter)
        {
            var items = this.countryRepository.GetAll()
                .Where(c => c.Name.ToLower().Contains(filter.ToLower()))
                .OrderBy(r => r.DisplayOrder);

            var countryList = await this.mapper.ProjectTo<GenericListViewModel>(items).ToListWithNoLockAsync();

            return countryList;
        }
    }
}
