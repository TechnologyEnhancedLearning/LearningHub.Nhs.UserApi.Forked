// <copyright file="ElfhUserRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Dto;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Extentions;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Shared;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The user repository.
    /// </summary>
    public class ElfhUserRepository : GenericElfhRepository<User>, IElfhUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhUserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="logger">The logger.</param>
        public ElfhUserRepository(ElfhHubDbContext dbContext, ILogger<User> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<User> GetByIdAsync(int id, UserIncludeCollectionsEnum[] includeCollections = null)
        {
            return await this.DbContext.User.IncludeCollections(includeCollections).AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public async Task<User> GetByUsernameAsync(string username, UserIncludeCollectionsEnum[] includeCollections = null)
        {
            return await this.DbContext.User.IncludeCollections(includeCollections).AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync(n => n.UserName == username);
        }

        /// <inheritdoc/>
        public async Task<int> GetUserIdByUsernameAsync(string username)
        {
            return await this.DbContext.User.AsNoTracking().Where(n => n.UserName == username).Select(n => n.Id).FirstOrDefaultWithNoLockAsync();
        }

        /// <inheritdoc/>
        public async Task<User> GetByOpenAthensIdAsync(string openAthensId)
        {
            return await this.DbContext.User.AsNoTracking()
                            .FirstOrDefaultWithNoLockAsync(f => f.UserAttributes.Any(a => a.TextValue == openAthensId));
        }

        /// <inheritdoc/>
        public async Task<bool> DoesEmailExistAsync(string emailAddress)
        {
            return await this.DbContext.User.AnyAsync(n => n.EmailAddress == emailAddress);
        }

        /// <inheritdoc/>
        public IQueryable<User> GetUsersForEmail(string emailAddress)
        {
            return this.DbContext.User.AsNoTracking().Where(x => x.EmailAddress == emailAddress);
        }

        /// <inheritdoc/>
        public async Task<int> NumberOfUsersForEmailAsync(string emailAddress)
        {
            return await this.DbContext.User.CountWithNoLockAsync(x => x.EmailAddress == emailAddress);
        }

        /// <inheritdoc/>
        public bool DoesUserNameExist(string username)
        {
            return this.DbContext.User.Any(n => n.UserName == username);
        }

        /// <inheritdoc/>
        public bool DoesUserExist(int medicalCouncilId, string medicalCouncilPrefix, string medicalCouncilNumber, string emailAddress)
        {
            var userExists = (from u in this.DbContext.User
                              join ue in this.DbContext.UserEmployment on u.Id equals ue.UserId into uej
                              from x in uej.DefaultIfEmpty()
                              where u.EmailAddress == emailAddress
                                  || (u.UserName == medicalCouncilPrefix + medicalCouncilNumber)
                                  || (x.MedicalCouncilNo == medicalCouncilNumber && x.MedicalCouncilId == medicalCouncilId)
                              select u).Any();

            return userExists;
        }

        /// <inheritdoc/>
        public IQueryable<User> GetAllWithUserEmployment()
        {
            return this.DbContext.Set<User>().Include(u => u.UserEmployment).AsNoTracking();
        }

        /// <inheritdoc/>
        public bool IsEIntegrityUser(int userId)
        {
            return this.DbContext.UserUserGroup.AsNoTracking()
                .Any(
                    uug => uug.UserId == userId &&
                           uug.UserGroup.Code == "eint");
        }

        /// <inheritdoc/>
        public bool IsBasicUser(int userId)
        {
            return this.DbContext.UserUserGroup
                .Any(
                    uug => uug.UserId == userId &&
                           uug.UserGroup.Code == "basic");
        }

        /// <inheritdoc/>
        public bool IsAdminUser(int userId)
        {
            return this.DbContext.UserUserGroup
                .Any(uug => uug.UserId == userId &&
                            uug.UserGroup.Name == "System Administrators");
        }

        /// <inheritdoc/>
        public bool IsLearningHubReadOnlyUser(int userId)
        {
            return this.DbContext.UserUserGroup
                .Any(uug => uug.UserId == userId &&
                            uug.UserGroup.Name == "Learning Hub Read Only");
        }

        /// <inheritdoc/>
        public async Task LinkEmploymentRecordToUser(int userId)
        {
            try
            {
                var param0 = new SqlParameter("@p0", SqlDbType.Int) { Value = userId };

                await this.DbContext.Database.ExecuteSqlRawAsync("proc_LinkEmploymentRecordToUser @p0", param0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// The get user detail for the authentication.
        /// </summary>
        /// <param name = "username">
        /// username.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<UserAuthenticateDto> GetUserDetailForAuthentication(string username)
        {
            var param0 = new SqlParameter("@userName", SqlDbType.VarChar) { Value = username };

            var userAuthenticateDto = await this.DbContext.UserAuthenticateDto.FromSqlRaw("proc_UserDetailForAuthenticationByUserName @userName", param0).AsNoTracking().ToListAsync();

            return userAuthenticateDto.FirstOrDefault();
        }
    }
}