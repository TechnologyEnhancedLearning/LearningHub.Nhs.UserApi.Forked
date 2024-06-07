namespace LearningHub.Nhs.Auth.Configuration
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The same site cookies service collection extensions.
    /// </summary>
    public static class SameSiteCookiesServiceCollectionExtensions
    {
        /// <summary>
        /// The unspecified.
        /// </summary>
        private const SameSiteMode Unspecified = (SameSiteMode)(-1);

        /// <summary>
        /// The configure non breaking same site cookies.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection ConfigureNonBreakingSameSiteCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });

            return services;
        }

        /// <summary>
        /// The check same site.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        private static void CheckSameSite(HttpContext context, CookieOptions options)
        {
            // SameSite cookies must be secure, when behind a load balancer the immediate web server is running on http
            // so by default the cookie won't be created as secure. We need to sniff out whether we are receiving headers
            // from the load balancer and force the cookie to be secure.
            var forwardedHeaders = new
                                       {
                                           port = context.Request.Headers["X-Forwarded-Port"].ToString(),
                                           protocol = context.Request.Headers["X-Forwarded-Proto"].ToString(),
                                       };
            if (int.TryParse(forwardedHeaders.port, out var portNum)
                && !string.IsNullOrWhiteSpace(forwardedHeaders.protocol))
            {
                if (portNum == 443 && forwardedHeaders.protocol == "https")
                {
                    options.Secure = true;
                }
            }

            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = context.Request.Headers["User-Agent"].ToString();

                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = Unspecified;
                }
            }
        }

        /// <summary>
        /// The disallows same site none.
        /// </summary>
        /// <param name="userAgent">
        /// The user agent.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool DisallowsSameSiteNone(string userAgent)
        {
            if (userAgent.Contains("CPU iPhone OS 12", StringComparison.InvariantCulture) ||
                userAgent.Contains("iPad; CPU OS 12", StringComparison.InvariantCulture))
            {
                return true;
            }

            if (userAgent.Contains("Safari", StringComparison.InvariantCulture) &&
                userAgent.Contains("Macintosh; Intel Mac OS X 10_14", StringComparison.InvariantCulture) &&
                userAgent.Contains("Version/", StringComparison.InvariantCulture))
            {
                return true;
            }

            return false;
        }
    }
}
