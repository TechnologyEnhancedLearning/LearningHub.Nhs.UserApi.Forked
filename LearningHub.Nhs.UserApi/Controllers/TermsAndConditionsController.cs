namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// TermsAndConditions operations.
    /// </summary>
    [Authorize(Policy = "AuthorizeOrCallFromLH")]
    [Route("api/[controller]")]
    [ApiController]
    public class TermsAndConditionsController : ApiControllerBase
    {
        /// <summary>
        /// The terms and conditions service.
        /// </summary>
        private ITermsAndConditionsService termsAndConditionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TermsAndConditionsController"/> class.
        /// </summary>
        /// <param name="elfhUserService">
        /// The elfh user service.
        /// </param>
        /// <param name="termsAndConditionsService">
        /// The terms and conditions service.
        /// </param>
        public TermsAndConditionsController(
            IElfhUserService elfhUserService,
            ITermsAndConditionsService termsAndConditionsService)
            : base(elfhUserService)
        {
            this.termsAndConditionsService = termsAndConditionsService;
        }

        // GET api/TermsAndConditions/LatestVersion/id

        /// <summary>
        /// Get latest version of Terms And Conditions by tenantId.
        /// </summary>
        /// <param name="tenantId">
        /// The tenant id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("LatestVersion/{tenantId}")]
        public async Task<IActionResult> LatestVersion(int tenantId)
        {
            var termsAndConditions = await this.termsAndConditionsService.GetLatestVersionAsync(tenantId);

            return this.Ok(termsAndConditions);
        }

        /// <summary>
        /// Create a new UserTermsAndConditions record.
        /// </summary>
        /// <param name="userTermsAndConditions">
        /// The user terms and conditions.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("AcceptByUser")]
        public async Task<IActionResult> AcceptByUser([FromBody] UserTermsAndConditionsViewModel userTermsAndConditions)
        {
            var vr = await this.termsAndConditionsService.AcceptByUser(userTermsAndConditions, this.CurrentUserId);
            if (vr.IsValid)
            {
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }
    }
}