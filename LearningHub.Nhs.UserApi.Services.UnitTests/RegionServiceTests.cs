// <copyright file="RegionServiceTests.cs" company="HEE.nhs.uk">
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
    /// The region service tests.
    /// </summary>
    public class RegionServiceTests
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
            int regionId = 1;
            var regionRepositoryMock = new Mock<IRegionRepository>(MockBehavior.Strict);

            regionRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestRegions().FirstOrDefault(c => c.Id == regionId)));

            var regionService = new RegionService(regionRepositoryMock.Object, null, null);

            var region = await regionService.GetByIdAsync(regionId);

            Assert.IsType<Region>(region);
            Assert.Equal(1, region.Id);
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
            int regionId = 100;
            var regionRepositoryMock = new Mock<IRegionRepository>(MockBehavior.Strict);

            regionRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestRegions().FirstOrDefault(c => c.Id == regionId)));

            var regionService = new RegionService(regionRepositoryMock.Object, null, null);

            var region = await regionService.GetByIdAsync(regionId);

            Assert.Null(region);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetAll_Valid()
        {
            var regionRepositoryMock = new Mock<IRegionRepository>(MockBehavior.Strict);

            regionRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestRegionsAsyncMock().Object);
            var items = regionRepositoryMock.Object.GetAll()
                .OrderBy(r => r.DisplayOrder);
            var mappedItems = this.NewMapper().ProjectTo<GenericListViewModel>(items);
            var mapResult = new AsyncEnumerable<GenericListViewModel>(mappedItems);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(
                    s => s.ProjectTo<GenericListViewModel>(It.IsAny<IQueryable<Region>>(), It.IsAny<object>()))
                .Returns(mapResult);

            var countryService = new RegionService(regionRepositoryMock.Object, null, mapperMock.Object);

            var regionList = await countryService.GetAllAsync();

            Assert.IsType<List<GenericListViewModel>>(regionList);
            Assert.Equal(9, regionList.Count);
            Assert.Equal(5, regionList.First().Id);
            Assert.Equal(13, regionList.Last().Id);
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
        /// The test regions async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<Region>> TestRegionsAsyncMock()
        {
            var regionRecords = this.TestRegions().Where(r => !r.Deleted).AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<Region>>();

            mockCountryDbSet.As<IAsyncEnumerable<Region>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<Region>(regionRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<Region>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Region>(regionRecords.Provider));

            mockCountryDbSet.As<IQueryable<Region>>().Setup(m => m.Expression).Returns(regionRecords.Expression);
            mockCountryDbSet.As<IQueryable<Region>>().Setup(m => m.ElementType).Returns(regionRecords.ElementType);
            mockCountryDbSet.As<IQueryable<Region>>().Setup(m => m.GetEnumerator()).Returns(() => regionRecords.GetEnumerator());

            return mockCountryDbSet;
        }

        /// <summary>
        /// The test regions.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<Region> TestRegions()
        {
            return new List<Region>()
            {
                new Region()
                {
                    Id = 1,
                    Name = "North",
                    DisplayOrder = 1,
                    Deleted = true,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9958129Z"),
                },
                new Region()
                {
                    Id = 2,
                    Name = "South",
                    DisplayOrder = 2,
                    Deleted = true,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9958129Z"),
                },
                new Region()
                {
                    Id = 3,
                    Name = "West",
                    DisplayOrder = 3,
                    Deleted = true,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9958129Z"),
                },
                new Region()
                {
                    Id = 4,
                    Name = "East",
                    DisplayOrder = 4,
                    Deleted = true,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9958129Z"),
                },
                new Region()
                {
                    Id = 5,
                    Name = "East of England",
                    DisplayOrder = 1,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 6,
                    Name = "East Midlands",
                    DisplayOrder = 2,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 7,
                    Name = "London",
                    DisplayOrder = 3,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 8,
                    Name = "North East",
                    DisplayOrder = 4,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 9,
                    Name = "North West",
                    DisplayOrder = 5,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 10,
                    Name = "South East",
                    DisplayOrder = 6,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 11,
                    Name = "South West",
                    DisplayOrder = 7,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 12,
                    Name = "West Midlands",
                    DisplayOrder = 8,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
                new Region()
                {
                    Id = 13,
                    Name = "Yorkshire and the Humber",
                    DisplayOrder = 9,
                    Deleted = false,
                    AmendUserId = 4,
                    AmendDate = DateTimeOffset.Parse("2018-01-30T23:31:46.9968136Z"),
                },
            };
        }
    }
}
