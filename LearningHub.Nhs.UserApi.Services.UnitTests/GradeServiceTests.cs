namespace LearningHub.Nhs.UserApi.Services.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using EntityFrameworkCore.Testing.Common;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.UnitTests.Helpers;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    /// <summary>
    /// The grade service tests.
    /// </summary>
    public class GradeServiceTests
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
            int gradeId = 182;
            var gradeRepositoryMock = new Mock<IGradeRepository>(MockBehavior.Strict);

            gradeRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestGrades().FirstOrDefault(c => c.Id == gradeId)));

            var gradeService = new GradeService(gradeRepositoryMock.Object, null);

            var grade = await gradeService.GetByIdAsync(gradeId);

            Assert.IsType<Grade>(grade);
            Assert.Equal(182, grade.Id);
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
            int gradeId = 100;
            var gradeRepositoryMock = new Mock<IGradeRepository>(MockBehavior.Strict);

            gradeRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                                        .Returns(Task.FromResult(
                                            this.TestGrades().FirstOrDefault(c => c.Id == gradeId)));

            var gradeService = new GradeService(gradeRepositoryMock.Object, null);

            var grade = await gradeService.GetByIdAsync(gradeId);

            Assert.Null(grade);
        }

        /// <summary>
        /// The get all_ valid.
        /// </summary>
        [Fact]
        public void GetAll_Valid()
        {
            var gradeRepositoryMock = new Mock<IGradeRepository>(MockBehavior.Strict);

            gradeRepositoryMock.Setup(r => r.GetAll()).Returns(this.TestGradesAsyncMock().Object);

            var gradeService = new GradeService(gradeRepositoryMock.Object, null);

            var gradeList = gradeService.GetAll();

            Assert.IsType<List<Grade>>(gradeList);
            Assert.Equal(14, gradeList.Count());
            Assert.Equal(175, gradeList.First().Id);
            Assert.Equal(177, gradeList.Last().Id);
        }

        /// <summary>
        /// The get by job role_ valid.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Fact]
        public async Task GetByJobRole_Valid()
        {
            var gradeRepositoryMock = new Mock<IGradeRepository>(MockBehavior.Strict);

            gradeRepositoryMock.Setup(r => r.GetByJobRole(It.IsAny<int>()))
                                        .Returns(new AsyncEnumerable<Grade>(
                                                this.TestGradesAsyncMock().Object.Where(g => g.Id == 180 || g.Id == 181)));

            var gradeService = new GradeService(gradeRepositoryMock.Object, null);

            var gradeList = await gradeService.GetByJobRole(1);

            Assert.IsType<List<Grade>>(gradeList);
            Assert.Equal(2, gradeList.Count);
        }

        /// <summary>
        /// The test grades async mock.
        /// </summary>
        /// <returns>
        /// The <see cref="Mock"/>.
        /// </returns>
        private Mock<DbSet<Grade>> TestGradesAsyncMock()
        {
            var gradeRecords = this.TestGrades().Where(r => !r.Deleted).AsQueryable();

            var mockCountryDbSet = new Mock<DbSet<Grade>>();

            mockCountryDbSet.As<IAsyncEnumerable<Grade>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<Grade>(gradeRecords.GetEnumerator()));

            mockCountryDbSet.As<IQueryable<Grade>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Grade>(gradeRecords.Provider));

            mockCountryDbSet.As<IQueryable<Grade>>().Setup(m => m.Expression).Returns(gradeRecords.Expression);
            mockCountryDbSet.As<IQueryable<Grade>>().Setup(m => m.ElementType).Returns(gradeRecords.ElementType);
            mockCountryDbSet.As<IQueryable<Grade>>().Setup(m => m.GetEnumerator()).Returns(() => gradeRecords.GetEnumerator());

            return mockCountryDbSet;
        }

        /// <summary>
        /// The test grades.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<Grade> TestGrades()
        {
            return new List<Grade>()
            {
                new Grade()
                {
                    Id = 175,
                    Name = "Consultant",
                    DisplayOrder = 23,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6200000Z"),
                },
                new Grade()
                {
                    Id = 176,
                    Name = "Associate Specialist",
                    DisplayOrder = 4,
                    Deleted = true,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6300000Z"),
                },
                new Grade()
                {
                    Id = 177,
                    Name = "Staff Grade",
                    DisplayOrder = 52,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6400000Z"),
                },
                new Grade()
                {
                    Id = 178,
                    Name = "GP",
                    DisplayOrder = 33,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6470000Z"),
                },
                new Grade()
                {
                    Id = 179,
                    Name = "Specialist (post CCT)",
                    DisplayOrder = 43,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6570000Z"),
                },
                new Grade()
                {
                    Id = 180,
                    Name = "F1",
                    DisplayOrder = 24,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6600000Z"),
                },
                new Grade()
                {
                    Id = 181,
                    Name = "F2",
                    DisplayOrder = 25,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6670000Z"),
                },
                new Grade()
                {
                    Id = 182,
                    Name = "ST1\\/CT1",
                    DisplayOrder = 45,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6730000Z"),
                },
                new Grade()
                {
                    Id = 183,
                    Name = "ST2\\/CT2",
                    DisplayOrder = 46,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6800000Z"),
                },
                new Grade()
                {
                    Id = 184,
                    Name = "ST3\\/CT3",
                    DisplayOrder = 47,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6870000Z"),
                },
                new Grade()
                {
                    Id = 185,
                    Name = "ST4",
                    DisplayOrder = 48,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.6930000Z"),
                },
                new Grade()
                {
                    Id = 186,
                    Name = "ST5",
                    DisplayOrder = 49,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.7000000Z"),
                },
                new Grade()
                {
                    Id = 187,
                    Name = "ST6",
                    DisplayOrder = 50,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.7030000Z"),
                },
                new Grade()
                {
                    Id = 188,
                    Name = "ST7",
                    DisplayOrder = 51,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.7100000Z"),
                },
                new Grade()
                {
                    Id = 189,
                    Name = "FTSTA1",
                    DisplayOrder = 28,
                    Deleted = false,
                    AmendUserId = 2,
                    AmendDate = DateTimeOffset.Parse("2010-07-13T13:28:58.7370000Z"),
                },
            };
        }
    }
}
