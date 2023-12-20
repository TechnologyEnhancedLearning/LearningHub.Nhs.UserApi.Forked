// <copyright file="IMessageService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
  using System.Threading.Tasks;

  /// <summary>
  /// The IMessageService interface.
  /// </summary>
  public interface IMessageService
  {
    /// <summary>
    /// The CreateEmailAsync.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="body">The body.</param>
    /// <param name="recipientUserId">The recipient user id.</param>
    /// <returns>The task.</returns>
    Task CreateEmailAsync(int userId, string subject, string body, int recipientUserId);

    /// <summary>
    /// The CreateEmailAsync.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="body">The body.</param>
    /// <param name="recipientEmailAddress">The recipientEmailAddress.</param>
    /// <returns>The task.</returns>
    Task CreateEmailAsync(int userId, string subject, string body, string recipientEmailAddress);
  }
}
