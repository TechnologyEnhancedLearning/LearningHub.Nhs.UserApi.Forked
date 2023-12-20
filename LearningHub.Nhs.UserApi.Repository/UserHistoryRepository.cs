// <copyright file="UserHistoryRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The user history repository.
    /// </summary>
    public class UserHistoryRepository : IUserHistoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserHistoryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        public UserHistoryRepository(ElfhHubDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <inheritdoc/>
        public async Task<UserHistory> GetByIdAsync(int id)
        {
            return await this.DbContext.UserHistory.AsNoTracking().FirstOrDefaultWithNoLockAsync(n => n.Id == id);
        }

        /// <inheritdoc/>
        public async Task<List<UserHistoryStoredProcResult>> GetByUserIdAsync(int userId)
        {
            var param0 = new SqlParameter("@p0", SqlDbType.Int) { Value = userId };
            var param1 = new SqlParameter("@p1", SqlDbType.Int) { Value = 1 };
            var param2 = new SqlParameter("@p2", SqlDbType.Int) { Value = short.MaxValue };
            var param3 = new SqlParameter("@p3", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var result = await this.DbContext.Set<UserHistoryStoredProcResult>().FromSqlRaw(
                             "dbo.proc_UserHistoryLoadForUser @p0, @p1, @p2, @p3 output", param0, param1, param2, param3).AsNoTracking().ToListWithNoLockAsync();

            return result;
        }

        /// <inheritdoc/>
        public async Task CreateAsync(int userId, int tenantId, UserHistoryViewModel userHistoryVM)
        {
            try
            {
                var sqlParams = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@UserHistoryTypeId", SqlDbType.Int) { Value = userHistoryVM.UserHistoryTypeId },
                    new SqlParameter("@Detail", SqlDbType.VarChar) { Value = userHistoryVM.Detail ?? (object)DBNull.Value },
                    new SqlParameter("@UserAgent", SqlDbType.VarChar) { Value = userHistoryVM.UserAgent ?? (object)DBNull.Value },
                    new SqlParameter("@BrowserName", SqlDbType.VarChar) { Value = userHistoryVM.BrowserName ?? (object)DBNull.Value },
                    new SqlParameter("@BrowserVersion", SqlDbType.VarChar) { Value = userHistoryVM.BrowserVersion ?? (object)DBNull.Value },
                    new SqlParameter("@UrlReferer", SqlDbType.VarChar) { Value = userHistoryVM.UrlReferer ?? (object)DBNull.Value },
                    new SqlParameter("@LoginIP", SqlDbType.VarChar) { Value = userHistoryVM.LoginIP ?? (object)DBNull.Value },
                    new SqlParameter("@LoginSuccessFul", SqlDbType.Bit) { Value = userHistoryVM.LoginSuccessFul ?? (object)DBNull.Value },
                    new SqlParameter("@TenantId", SqlDbType.Int) { Value = tenantId },
                    new SqlParameter("@AmendUserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@AmendDate", SqlDbType.DateTimeOffset) { Value = DateTimeOffset.Now },
                };

                string sql = "proc_UserHistoryInsert @UserId, @UserHistoryTypeId, @Detail, @UserAgent, @BrowserName, @BrowserVersion, @UrlReferer, @LoginIP, @LoginSuccessFul, @TenantId, @AmendUserId, @AmendDate";

                await this.DbContext.Database.ExecuteSqlRawAsync(sql, sqlParams);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<UserHistoryStoredProcResults> GetPagedByUserIdAsync(int userId, int startPage, int pageSize)
        {
            var retVal = new UserHistoryStoredProcResults();

            var param0 = new SqlParameter("@p0", SqlDbType.Int) { Value = userId };
            var param1 = new SqlParameter("@p1", SqlDbType.Int) { Value = startPage };
            var param2 = new SqlParameter("@p2", SqlDbType.Int) { Value = pageSize };
            var param3 = new SqlParameter("@p3", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var result = await this.DbContext.Set<UserHistoryStoredProcResult>().FromSqlRaw(
                             "dbo.proc_UserHistoryLoadForLearningHubUser @p0, @p1, @p2, @p3 output", param0, param1, param2, param3).AsNoTracking().ToListWithNoLockAsync();

            retVal.Results = result;
            retVal.TotalResults = (int)param3.Value;

            return retVal;
        }
    }
}