// <copyright file="JobRoleServiceTests.cs" company="HEE.nhs.uk">
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
    /// The job role service tests.
    /// </summary>
    public class JobRoleServiceTests
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
            int jobRoleId = 57;
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);

            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestJobRoles().FirstOrDefault(c => c.Id == jobRoleId)));

            var jobRoleService = new JobRoleService(jobRoleRepositoryMock.Object, this.NewMapper());

            var jobRole = await jobRoleService.GetByIdAsync(jobRoleId);

            Assert.IsType<JobRoleBasicViewModel>(jobRole);
            Assert.Equal(57, jobRole.Id);
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
            int jobRoleId = 100;
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);

            jobRoleRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestJobRoles().FirstOrDefault(c => c.Id == jobRoleId)));

            var jobRoleService = new JobRoleService(jobRoleRepositoryMock.Object, this.NewMapper());

            var jobRole = await jobRoleService.GetByIdAsync(jobRoleId);

            Assert.Null(jobRole);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        [Fact]
        public void GetAll_Valid()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);

            jobRoleRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestJobRolesAsyncMock().Object);

            var jobRoleService = new JobRoleService(jobRoleRepositoryMock.Object, null);

            var jobRoleList = jobRoleService.GetAll();

            Assert.IsType<List<JobRole>>(jobRoleList);
            Assert.Equal(4, jobRoleList.Count());
            Assert.Equal(56, jobRoleList.First().Id);
            Assert.Equal(60, jobRoleList.Last().Id);
        }

        /// <summary>
        /// The get filtered with staff group async_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetFilteredWithStaffGroupAsync_Valid()
        {
            var jobRoleRepositoryMock = new Mock<IJobRoleRepository>(MockBehavior.Strict);

            jobRoleRepositoryMock.Setup(r => r.GetAllWithStaffGroup()).Returns(this.TestJobRolesAsyncMock().Object);
            var items = jobRoleRepositoryMock.Object.GetAllWithStaffGroup()
                .Where(jr => (jr.Name + " (" + jr.StaffGroup.Name + ")").Contains("Tech"))
                .OrderBy(r => r.DisplayOrder);
            var mappedItems = this.NewMapper().ProjectTo<JobRoleBasicViewModel>(items);
            var mapResult = new AsyncEnumerable<JobRoleBasicViewModel>(mappedItems);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(
                    s => s.ProjectTo<JobRoleBasicViewModel>(It.IsAny<IQueryable<JobRole>>(), It.IsAny<object>()))
                .Returns(mapResult);

            var jobRoleService = new JobRoleService(jobRoleRepositoryMock.Object, mapperMock.Object);

            var jobRoleList = await jobRoleService.GetFilteredWithStaffGroupAsync("Tech");

            Assert.IsType<List<JobRoleBasicViewModel>>(jobRoleList);
            Assert.Single(jobRoleList);
            Assert.Equal("Ambulance Technician (Students)", jobRoleList.First().NameWithStaffGroup);
        }

        /// <summary>
        /// The test job roles async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<JobRole>> TestJobRolesAsyncMock()
        {
            var jobRoleRecords = this.TestJobRoles().Where(r => !r.Deleted).AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<JobRole>>();

            mockCountryDbSet.As<IAsyncEnumerable<JobRole>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<JobRole>(jobRoleRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<JobRole>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<JobRole>(jobRoleRecords.Provider));

            mockCountryDbSet.As<IQueryable<JobRole>>().Setup(m => m.Expression).Returns(jobRoleRecords.Expression);
            mockCountryDbSet.As<IQueryable<JobRole>>().Setup(m => m.ElementType).Returns(jobRoleRecords.ElementType);
            mockCountryDbSet.As<IQueryable<JobRole>>().Setup(m => m.GetEnumerator()).Returns(() => jobRoleRecords.GetEnumerator());

            return mockCountryDbSet;
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
        /// The test job roles.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<JobRole> TestJobRoles()
        {
            return new List<JobRole>()
            {
                 new JobRole()
                {
                    Id = 56,
                    Name = "Administration",
                    DisplayOrder = 1,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:31:01.0870000Z"),
                    StaffGroup = new StaffGroup()
                    {
                        Id = 1,
                        Name = "Medical (GMC) and Dental (GDC)",
                        DisplayOrder = 1,
                        InternalUsersOnly = false,
                        Deleted = false,
                        AmendUserId = 1,
                        AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                    },
                },
                 new JobRole()
                {
                    Id = 57,
                    Name = "Adult Nurse",
                    DisplayOrder = 2,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:31:01.0930000Z"),
                    StaffGroup = new StaffGroup(),
                },
                 new JobRole()
                {
                    Id = 58,
                    Name = "Ambulance Care Assistant",
                    DisplayOrder = 3,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:31:01.1000000Z"),
                    StaffGroup = new StaffGroup()
                    {
                        Id = 2,
                        Name = "Students",
                        DisplayOrder = 2,
                        InternalUsersOnly = false,
                        Deleted = false,
                        AmendUserId = 1,
                        AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                    },
                },
                 new JobRole()
                {
                    Id = 59,
                    Name = "Ambulance Technician",
                    DisplayOrder = 4,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:28:15.8630000Z"),
                    StaffGroup = new StaffGroup()
                    {
                        Id = 1,
                        Name = "Medical (GMC) and Dental (GDC)",
                        DisplayOrder = 1,
                        InternalUsersOnly = false,
                        Deleted = false,
                        AmendUserId = 1,
                        AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                    },
                    MedicalCouncil = new MedicalCouncil()
                    {
                        Name = string.Empty,
                    },
                },
                 new JobRole()
                {
                    Id = 60,
                    Name = "Ambulance Technician",
                    DisplayOrder = 5,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2010-07-06T17:17:57.8600000Z"),
                    StaffGroup = new StaffGroup()
                    {
                        Id = 2,
                        Name = "Students",
                        DisplayOrder = 2,
                        InternalUsersOnly = false,
                        Deleted = false,
                        AmendUserId = 1,
                        AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                    },
                },
            };
        }
    }
}
