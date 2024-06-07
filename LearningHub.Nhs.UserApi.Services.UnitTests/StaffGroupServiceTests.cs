namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.UnitTests.Helpers;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    /// <summary>
    /// The staff group service tests.
    /// </summary>
    public class StaffGroupServiceTests
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
            int staffGroupId = 1;
            var staffGroupRepositoryMock = new Mock<IStaffGroupRepository>(MockBehavior.Strict);

            staffGroupRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestStaffGroups().FirstOrDefault(c => c.Id == staffGroupId)));

            var staffGroupService = new StaffGroupService(staffGroupRepositoryMock.Object, null);

            var staffGroup = await staffGroupService.GetByIdAsync(staffGroupId);

            Assert.IsType<StaffGroup>(staffGroup);
            Assert.Equal(1, staffGroup.Id);
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
            int staffGroupId = 100;
            var staffGroupRepositoryMock = new Mock<IStaffGroupRepository>(MockBehavior.Strict);

            staffGroupRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestStaffGroups().FirstOrDefault(c => c.Id == staffGroupId)));

            var staffGroupService = new StaffGroupService(staffGroupRepositoryMock.Object, null);

            var staffGroup = await staffGroupService.GetByIdAsync(staffGroupId);

            Assert.Null(staffGroup);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        [Fact]
        public void GetAll_Valid()
        {
            var staffGroupRepositoryMock = new Mock<IStaffGroupRepository>(MockBehavior.Strict);

            staffGroupRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestStaffGroupsAsyncMock().Object);

            var countryService = new StaffGroupService(staffGroupRepositoryMock.Object, null);

            var staffGroupList = countryService.GetAll();

            Assert.IsType<List<StaffGroup>>(staffGroupList);
            Assert.Equal(11, staffGroupList.Count());
            Assert.Equal(1, staffGroupList.First().Id);
            Assert.Equal(11, staffGroupList.Last().Id);
        }

        /// <summary>
        /// The test staff groups async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<StaffGroup>> TestStaffGroupsAsyncMock()
        {
            var staffGroupRecords = this.TestStaffGroups().Where(r => !r.Deleted).AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<StaffGroup>>();

            mockCountryDbSet.As<IAsyncEnumerable<StaffGroup>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<StaffGroup>(staffGroupRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<StaffGroup>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<StaffGroup>(staffGroupRecords.Provider));

            mockCountryDbSet.As<IQueryable<StaffGroup>>().Setup(m => m.Expression).Returns(staffGroupRecords.Expression);
            mockCountryDbSet.As<IQueryable<StaffGroup>>().Setup(m => m.ElementType).Returns(staffGroupRecords.ElementType);
            mockCountryDbSet.As<IQueryable<StaffGroup>>().Setup(m => m.GetEnumerator()).Returns(() => staffGroupRecords.GetEnumerator());

            return mockCountryDbSet;
        }

        /// <summary>
        /// The test staff groups.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<StaffGroup> TestStaffGroups()
        {
            return new List<StaffGroup>()
            {
                new StaffGroup()
                {
                    Id = 1,
                    Name = "Medical (GMC) and Dental (GDC)",
                    DisplayOrder = 1,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 2,
                    Name = "Students",
                    DisplayOrder = 2,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 3,
                    Name = "Nursing and Midwifery Registered",
                    DisplayOrder = 3,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 4,
                    Name = "Allied Health Professionals",
                    DisplayOrder = 4,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 5,
                    Name = "Additional Professional, Scientific and Technical",
                    DisplayOrder = 5,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 6,
                    Name = "Healthcare Scientists",
                    DisplayOrder = 6,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 7,
                    Name = "Additional Clinical Services",
                    DisplayOrder = 7,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 8,
                    Name = "Administrative and Clerical",
                    DisplayOrder = 8,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 9,
                    Name = "Estates and Ancillary",
                    DisplayOrder = 9,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 10,
                    Name = "Supplementary Roles",
                    DisplayOrder = 10,
                    InternalUsersOnly = false,
                    Deleted = true,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2011-10-10T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 11,
                    Name = "Pharmaceutical",
                    DisplayOrder = 11,
                    InternalUsersOnly = false,
                    Deleted = false,
                    AmendUserId = 91751,
                    AmendDate = DateTimeOffset.Parse("2012-05-01T00:00:00Z"),
                },
                new StaffGroup()
                {
                    Id = 12,
                    Name = "e-LfH Staff",
                    DisplayOrder = 1,
                    InternalUsersOnly = true,
                    Deleted = false,
                    AmendDate = DateTimeOffset.Parse("2012-05-01T00:00:00Z"),
                },
            };
        }
    }
}
