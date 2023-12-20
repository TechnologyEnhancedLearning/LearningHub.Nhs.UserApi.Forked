// <copyright file="ExceptionMiddleware.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// The exception middleware.
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// The next.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ExceptionMiddleware> logger;

        /// <summary>
        /// The hosting env.
        /// </summary>
        private readonly IWebHostEnvironment hostingEnv;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="hostingEnv">
        /// The hosting env.
        /// </param>
        /// <param name="next">
        /// The next.
        /// </param>
        public ExceptionMiddleware(
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment hostingEnv,
            RequestDelegate next)
        {
            this.next = next;
            this.logger = logger;
            this.hostingEnv = hostingEnv;
        }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// The handle exception async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            this.logger.LogError(exception, exception.Message);

            var response = context.Response;
            var customException = exception as LearningHubException;
            var statusCode = (int)HttpStatusCode.InternalServerError;

            var message = this.hostingEnv.IsDevelopment() ? exception.Message : "Unexpected error";
            var description = this.hostingEnv.IsDevelopment() ? exception.StackTrace : "Unexpected error";

            if (customException != null)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse(message, description))).ConfigureAwait(false);
        }
    }
}
