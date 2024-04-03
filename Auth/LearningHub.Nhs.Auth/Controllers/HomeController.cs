namespace LearningHub.Nhs.Auth.Controllers
{
    using System.Threading.Tasks;
    using IdentityServer4.Services;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Filters;
    using LearningHub.Nhs.Auth.Models.Home;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Home Controller operations.
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// The interaction.
        /// </summary>
        private readonly IIdentityServerInteractionService interaction;

        /// <summary>
        /// The environment.
        /// </summary>
        private readonly IWebHostEnvironment environment;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;
        private readonly WebSettings webSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="interaction">Identity Server4 Interaction Service.</param>
        /// <param name="environment">Hosting Environment.</param>
        /// <param name="logger">ILogger instance.</param>
        /// <param name="webSettings">The webSettings.</param>
        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger, WebSettings webSettings)
        {
            this.interaction = interaction;
            this.environment = environment;
            this.logger = logger;
            this.webSettings = webSettings;
        }

        /// <summary>
        /// Shows the landing page.
        /// </summary>
        /// <returns>Default ViewModel.</returns>
        public IActionResult Index()
        {
            if (this.environment.IsDevelopment())
            {
                // only show in development
                this.ViewData["AuthMainTitle"] = "Authentication Service";
                this.ViewData["ClientLogoUrl"] = "https://www.nhs.uk";
                this.ViewData["ClientLogoSrc"] = "/images/nhs-blue.svg";
                this.ViewData["ClientLogoAltText"] = "The NHS";
                return this.View();
            }

            this.logger.LogWarning("Homepage is disabled in production. Returning 404.");
            return this.NotFound();
        }

        /// <summary>
        /// Shows the error page.
        /// </summary>
        /// <returns>Error ViewModel.</returns>
        public async Task<IActionResult> Error()
        {
            this.ViewBag.SupportFormUrl = this.webSettings.SupportForm;
            return this.View("Error");
        }

        /// <summary>
        /// Shows the HealthCheck response.
        /// </summary>
        /// <returns>HealthCheck.</returns>
        public IActionResult HealthCheck()
        {
            return this.Ok();
        }
    }
}