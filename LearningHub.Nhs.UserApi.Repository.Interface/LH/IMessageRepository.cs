// <copyright file="IMessageRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface.LH
{
  using System.Threading.Tasks;

  /// <summary>
  /// The EmailTemplateRepository interface.
  /// </summary>
  public interface IMessageRepository
  {
    /// <summary>
    /// Creates an email to be sent.
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
