// <copyright file="CountryServiceTests.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Automapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using EntityFrameworkCore.Testing.Common;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.UnitTests.Helpers;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    /// <summary>
    /// The country service tests.
    /// </summary>
    public class CountryServiceTests
    {
        /// <summary>
        /// The get by id async_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByIdAsync_Valid()
        {
            int countryId = 1;
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);

            countryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestCountries().FirstOrDefault(c => c.Id == countryId)));

            var countryService = new CountryService(countryRepositoryMock.Object, null, null);

            var country = await countryService.GetByIdAsync(countryId);

            Assert.IsType<Country>(country);
            Assert.Equal(1, country.Id);
        }

        /// <summary>
        /// The get by id async_ invalid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByIdAsync_Invalid()
        {
            int countryId = 100;
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);

            countryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestCountries().FirstOrDefault(c => c.Id == countryId)));

            var countryService = new CountryService(countryRepositoryMock.Object, null, null);

            var country = await countryService.GetByIdAsync(countryId);

            Assert.Null(country);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        [Fact]
        public void GetAll_Valid()
        {
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);

            countryRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestCountriesAsyncMock().Object);

            var countryService = new CountryService(countryRepositoryMock.Object, null, null);

            var countryList = countryService.GetAll();

            Assert.IsType<List<Country>>(countryList);
            Assert.Equal(10, countryList.Count());
            Assert.Equal(7, countryList.First().Id);
            Assert.Equal(13, countryList.Last().Id);
        }

        /// <summary>
        /// The get filtered async_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetFilteredAsync_Valid()
        {
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);

            countryRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestCountriesAsyncMock().Object);
            var items = countryRepositoryMock.Object.GetAll()
                .Where(w => w.Name.ToLower().Contains("al".ToLower()))
                .OrderBy(o => o.DisplayOrder);
            var mappedItems = this.NewMapper().ProjectTo<GenericListViewModel>(items);
            var mapResult = new AsyncEnumerable<GenericListViewModel>(mappedItems);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(
                    s => s.ProjectTo<GenericListViewModel>(It.IsAny<IQueryable<Country>>(), It.IsAny<object>()))
                .Returns(mapResult);

            var countryService = new CountryService(countryRepositoryMock.Object, null, mapperMock.Object);

            var countryList = await countryService.GetFilteredAsync("al");

            Assert.IsType<List<GenericListViewModel>>(countryList);
            Assert.Equal(3, countryList.Count);
            Assert.Equal(4, countryList.First().Id);
            Assert.Equal(13, countryList.Last().Id);
        }

        /// <summary>
        /// The get filtered async_ empty.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetFilteredAsync_Empty()
        {
            var countryRepositoryMock = new Mock<ICountryRepository>(MockBehavior.Strict);

            countryRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestCountriesAsyncMock().Object);
            var items = countryRepositoryMock.Object.GetAll()
                .Where(w => w.Name.ToLower().Contains("NOTHING".ToLower()))
                .OrderBy(o => o.DisplayOrder);
            var mappedItems = this.NewMapper().ProjectTo<GenericListViewModel>(items);
            var mapResult = new AsyncEnumerable<GenericListViewModel>(mappedItems);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(
                    s => s.ProjectTo<GenericListViewModel>(It.IsAny<IQueryable<Country>>(), It.IsAny<object>()))
                .Returns(mapResult);

            var countryService = new CountryService(countryRepositoryMock.Object, null, mapperMock.Object);

            var countryList = await countryService.GetFilteredAsync("NOTHING");

            Assert.IsType<List<GenericListViewModel>>(countryList);
            Assert.Empty(countryList);
        }

        /// <summary>
        /// The new mapper.
        /// </summary>
        /// <returns>
        /// The <see cref="IMapper"/>.
        /// </returns>
        private IMapper NewMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ElfhMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            return mapper;
        }

        /// <summary>
        /// The test countries async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<Country>> TestCountriesAsyncMock()
        {
            var countryRecords = this.TestCountries().AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<Country>>();

            mockCountryDbSet.As<IAsyncEnumerable<Country>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<Country>(countryRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<Country>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Country>(countryRecords.Provider));

            mockCountryDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(countryRecords.Expression);
            mockCountryDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(countryRecords.ElementType);
            mockCountryDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(() => countryRecords.GetEnumerator());

            return mockCountryDbSet;
        }

        /// <summary>
        /// The test countries.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<Country> TestCountries()
        {
            return new List<Country>()
            {
                new Country()
                {
                    Id = 1,
                    Name = "England",
                    Alpha2 = "GB",
                    Numeric = "826",
                    EUVatRate = 20,
                    DisplayOrder = 2,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-01-11T12:36:20.5300000Z"),
                },
                new Country()
                {
                    Id = 2,
                    Name = "Scotland",
                    Alpha2 = "GB",
                    Numeric = "826",
                    EUVatRate = 2.000000000000000e+001,
                    DisplayOrder = 5,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-01-11T12:36:20.5430000Z"),
                },
                new Country()
                {
                    Id = 3,
                    Name = "Northern Ireland",
                    Alpha2 = "GB",
                    Numeric = "826",
                    EUVatRate = 2.000000000000000e+001,
                    DisplayOrder = 4,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-01-11T12:36:20.5830000Z"),
                },
                new Country()
                {
                    Id = 4,
                    Name = "Wales",
                    Alpha2 = "GB",
                    Numeric = "826",
                    EUVatRate = 2.000000000000000e+001,
                    DisplayOrder = 6,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-01-11T12:36:20.5930000Z"),
                },
                new Country()
                {
                    Id = 7,
                    Name = "Channel Islands",
                    Alpha2 = "UK",
                    Numeric = "830",
                    EUVatRate = 0.000000000000000e+000,
                    DisplayOrder = 1,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-03-11T00:00:00Z"),
                },
                new Country()
                {
                    Id = 9,
                    Name = "Isle of Man",
                    Alpha2 = "GB",
                    Alpha3 = "IMN",
                    Numeric = "833",
                    EUVatRate = 2.000000000000000e+001,
                    DisplayOrder = 3,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-03-11T00:00:00Z"),
                },
                new Country()
                {
                    Id = 10,
                    Name = "AFGHANISTAN",
                    Alpha2 = "AF",
                    Alpha3 = "AFG",
                    Numeric = "004",
                    EUVatRate = 0.000000000000000e+000,
                    DisplayOrder = 8,
                    Deleted = false,
                    AmendUserId = 0,
                    AmendDate = DateTimeOffset.Parse("2012-02-10T12:38:25Z"),
                },
                new Country()
                {
                    Id = 11,
                    Name = "ÅLAND ISLANDS",
                    Alpha2 = "AX",
                    Alpha3 = "ALA",
                    Numeric = "248",
                    EUVatRate = 0.000000000000000e+000,
                    DisplayOrder = 9,
                    Deleted = false,
                    AmendUserId = 0,
                    AmendDate = DateTimeOffset.Parse("2012-02-10T12:38:25Z"),
                },
                new Country()
                {
                    Id = 12,
                    Name = "ALBANIA",
                    Alpha2 = "AL",
                    Alpha3 = "ALB",
                    Numeric = "008",
                    EUVatRate = 0.000000000000000e+000,
                    DisplayOrder = 10,
                    Deleted = false,
                    AmendUserId = 0,
                    AmendDate = DateTimeOffset.Parse("2012-02-10T12:38:25Z"),
                },
                new Country()
                {
                    Id = 13,
                    Name = "ALGERIA",
                    Alpha2 = "DZ",
                    Alpha3 = "DZA",
                    Numeric = "012",
                    EUVatRate = 0.000000000000000e+000,
                    DisplayOrder = 11,
                    Deleted = false,
                    AmendUserId = 0,
                    AmendDate = DateTimeOffset.Parse("2012-02-10T12:38:25Z"),
                },
            };
        }
    }
}
