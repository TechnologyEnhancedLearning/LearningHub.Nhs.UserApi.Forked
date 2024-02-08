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
    /// The specialty service tests.
    /// </summary>
    public class SpecialtyServiceTests
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
            int specialtyId = 6;
            var specialtyRepositoryMock = new Mock<ISpecialtyRepository>(MockBehavior.Strict);

            specialtyRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestSpecialties().FirstOrDefault(c => c.Id == specialtyId)));

            var specialtyService = new SpecialtyService(specialtyRepositoryMock.Object, null);

            var specialty = await specialtyService.GetByIdAsync(specialtyId);

            Assert.IsType<Specialty>(specialty);
            Assert.Equal(6, specialty.Id);
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
            int specialtyId = 100;
            var specialtyRepositoryMock = new Mock<ISpecialtyRepository>(MockBehavior.Strict);

            specialtyRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestSpecialties().FirstOrDefault(c => c.Id == specialtyId)));

            var specialtyService = new SpecialtyService(specialtyRepositoryMock.Object, null);

            var specialty = await specialtyService.GetByIdAsync(specialtyId);

            Assert.Null(specialty);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        [Fact]
        public void GetAll_Valid()
        {
            var specialtyRepositoryMock = new Mock<ISpecialtyRepository>(MockBehavior.Strict);

            specialtyRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestSpecialtysAsyncMock().Object);

            var specialtyService = new SpecialtyService(specialtyRepositoryMock.Object, null);

            var specialtyList = specialtyService.GetAll();

            Assert.IsType<List<Specialty>>(specialtyList);
            Assert.Equal(8, specialtyList.Count());
            Assert.Equal(4, specialtyList.First().Id);
            Assert.Equal(13, specialtyList.Last().Id);
        }

        /// <summary>
        /// The test specialtys async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<Specialty>> TestSpecialtysAsyncMock()
        {
            var specialtyRecords = this.TestSpecialties().Where(r => !r.Deleted).AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<Specialty>>();

            mockCountryDbSet.As<IAsyncEnumerable<Specialty>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<Specialty>(specialtyRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<Specialty>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Specialty>(specialtyRecords.Provider));

            mockCountryDbSet.As<IQueryable<Specialty>>().Setup(m => m.Expression).Returns(specialtyRecords.Expression);
            mockCountryDbSet.As<IQueryable<Specialty>>().Setup(m => m.ElementType).Returns(specialtyRecords.ElementType);
            mockCountryDbSet.As<IQueryable<Specialty>>().Setup(m => m.GetEnumerator()).Returns(() => specialtyRecords.GetEnumerator());

            return mockCountryDbSet;
        }

        private List<Specialty> TestSpecialties()
        {
            return new List<Specialty>()
            {
                new Specialty()
                {
                    Id = 1,
                    Name = "Accident and Emergency",
                    DisplayOrder = 0,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.6700000Z"),
                },
                new Specialty()
                {
                    Id = 2,
                    Name = "Acute Medicine",
                    DisplayOrder = 0,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.6800000Z"),
                },
                new Specialty()
                {
                    Id = 3,
                    Name = "Anaesthesia",
                    DisplayOrder = 0,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.6830000Z"),
                },
                new Specialty()
                {
                    Id = 4,
                    Name = "Cardiology",
                    DisplayOrder = 10,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.6900000Z"),
                },
                new Specialty()
                {
                    Id = 5,
                    Name = "Cardiothoracic Surgery",
                    DisplayOrder = 11,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.6970000Z"),
                },
                new Specialty()
                {
                    Id = 6,
                    Name = "Chemical Pathology",
                    DisplayOrder = 12,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7030000Z"),
                },
                new Specialty()
                {
                    Id = 7,
                    Name = "Clinical Genetics",
                    DisplayOrder = 14,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7100000Z"),
                },
                new Specialty()
                {
                    Id = 8,
                    Name = "Clinical Immunology & Allergy",
                    DisplayOrder = 16,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7130000Z"),
                },
                new Specialty()
                {
                    Id = 9,
                    Name = "Clinical Pharmacology & Therapeutics",
                    DisplayOrder = 0,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7200000Z"),
                },
                new Specialty()
                {
                    Id = 10,
                    Name = "Dermatology",
                    DisplayOrder = 27,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7270000Z"),
                },
                new Specialty()
                {
                    Id = 11,
                    Name = "Diabetes and Endocrinology",
                    DisplayOrder = 0,
                    Deleted = true,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7330000Z"),
                },
                new Specialty()
                {
                    Id = 12,
                    Name = "ENT",
                    DisplayOrder = 30,
                    Deleted = false,
                    AmendUserId = 1,
                    AmendDate = DateTimeOffset.Parse("2012-02-13T10:21:27.5600000Z"),
                },
                new Specialty()
                {
                    Id = 13,
                    Name = "Gastroenterology",
                    DisplayOrder = 31,
                    Deleted = false,
                    AmendUserId = 3,
                    AmendDate = DateTimeOffset.Parse("2010-07-15T14:49:36.7430000Z"),
                },
            };
        }
    }
}
