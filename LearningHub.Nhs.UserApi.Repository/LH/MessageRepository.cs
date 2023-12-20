// <copyright file="MessageRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.LH
{
  using System;
  using System.Data;
  using System.Threading.Tasks;
  using LearningHub.Nhs.Models.Entities.Messaging;
  using LearningHub.Nhs.UserApi.Repository;
  using LearningHub.Nhs.UserApi.Repository.Interface.LH;
  using Microsoft.Data.SqlClient;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// The user password validation token repository.
  /// </summary>
  public class MessageRepository : GenericLHRepository<Message>, IMessageRepository
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="tzOffsetManager">The Timezone offset manager.</param>
    public MessageRepository(LearningHubDbContext context, ITimezoneOffsetManager tzOffsetManager)
        : base(context, tzOffsetManager)
    {
    }

    /// <summary>
    /// The CreateEmailAsync.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="body">The body.</param>
    /// <param name="recipientUserId">The recipient user id.</param>
    /// <returns>The task.</returns>
    public async Task CreateEmailAsync(int userId, string subject, string body, int recipientUserId)
    {
      try
      {
        var param0 = new SqlParameter("@p0", SqlDbType.NVarChar) { Value = subject };
        var param1 = new SqlParameter("@p1", SqlDbType.NVarChar) { Value = body };
        var param2 = new SqlParameter("@p2", SqlDbType.Int) { Value = recipientUserId };
        var param3 = new SqlParameter("@p3", SqlDbType.Int) { Value = userId };
        var param4 = new SqlParameter("@p4", SqlDbType.Int) { Value = this.TimezoneOffsetManager.UserTimezoneOffset ?? (object)DBNull.Value };

        await this.DbContext.Database.ExecuteSqlRawAsync("messaging.CreateEmailForUser @p0, @p1, @p2, @p3, @p4", param0, param1, param2, param3, param4);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    /// <summary>
    /// The CreateEmailAsync.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="body">The body.</param>
    /// <param name="recipientEmailAddress">The recipientEmailAddress.</param>
    /// <returns>The task.</returns>
    public async Task CreateEmailAsync(int userId, string subject, string body, string recipientEmailAddress)
    {
      try
      {
        var param0 = new SqlParameter("@p0", SqlDbType.NVarChar) { Value = subject };
        var param1 = new SqlParameter("@p1", SqlDbType.NVarChar) { Value = body };
        var param2 = new SqlParameter("@p2", SqlDbType.NVarChar) { Value = recipientEmailAddress };
        var param3 = new SqlParameter("@p3", SqlDbType.Int) { Value = userId };
        var param4 = new SqlParameter("@p4", SqlDbType.Int) { Value = this.TimezoneOffsetManager.UserTimezoneOffset ?? (object)DBNull.Value };

        await this.DbContext.Database.ExecuteSqlRawAsync("messaging.CreateEmailForEmailAddress @p0, @p1, @p2, @p3, @p4", param0, param1, param2, param3, param4);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}