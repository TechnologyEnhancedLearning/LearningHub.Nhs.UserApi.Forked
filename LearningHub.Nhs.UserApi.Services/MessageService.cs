// <copyright file="MessageService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using LearningHub.Nhs.Models.Entities.Messaging;
  using LearningHub.Nhs.UserApi.Repository.Interface.LH;
  using LearningHub.Nhs.UserApi.Services.Interface;
  using Microsoft.Extensions.Logging;

  /// <summary>
  /// The MessageService class.
  /// </summary>
  public class MessageService : IMessageService
  {
    private readonly IMessageRepository messageRepository;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageService"/> class.
    /// </summary>
    /// <param name="messageRepository">The message repository.</param>
    /// <param name="logger">The logger.</param>
    public MessageService(IMessageRepository messageRepository, ILogger<Message> logger)
    {
      this.messageRepository = messageRepository;
      this.logger = logger;
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
      await this.messageRepository.CreateEmailAsync(userId, subject, body, recipientUserId);
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
      await this.messageRepository.CreateEmailAsync(userId, subject, body, recipientEmailAddress);
    }
  }
}
