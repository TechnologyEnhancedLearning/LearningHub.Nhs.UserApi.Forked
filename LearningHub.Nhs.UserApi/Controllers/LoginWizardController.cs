namespace LearningHub.Nhs.UserApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// LoginWizard Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginWizardController : ApiControllerBase
    {
        /// <summary>
        /// The login wizard service.
        /// </summary>
        private readonly ILoginWizardService loginWizardService;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<LoginWizardController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWizardController"/> class.
        /// </summary>
        /// <param name="elfhUserService">
        /// The elfh user service.
        /// </param>
        /// <param name="loginWizardService">
        /// The login wizard service.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public LoginWizardController(
            IElfhUserService elfhUserService,
            ILoginWizardService loginWizardService,
            ILogger<LoginWizardController> logger)
            : base(elfhUserService)
        {
            this.loginWizardService = loginWizardService;
            this.logger = logger;
        }

        // GET api/LoginWizard/GetLoginWizardStagesByUserId/id

        /// <summary>
        /// The get login wizard stages by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetLoginWizardStagesByUserId/{userId}")]
        public async Task<IActionResult> GetLoginWizardStagesByUserId(int userId)
        {
            var loginWizard = await this.loginWizardService.GetLoginWizardByUserIdAsync(userId);

            foreach (var s in loginWizard.LoginWizardStages)
            {
                this.logger.LogDebug($"Stage={s.Id} - {s.Description}");

                foreach (var r in s.LoginWizardRules)
                {
                    this.logger.LogDebug($"StageId={s.Id}, Rule={r.Id} - {r.Description}");
                }
            }

            return this.Ok(loginWizard);
        }

        /// <summary>
        /// The create stage activity.
        /// </summary>
        /// <param name="stageAndUser">
        /// The stage and user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [Route("CreateStageActivity")]
        public async Task<IActionResult> CreateStageActivity([FromBody] Tuple<int, int> stageAndUser)
        {
            var vr = await this.loginWizardService.CreateStageActivity(stageAndUser.Item1, stageAndUser.Item2);
            if (vr.IsValid)
            {
                return this.Ok(new ApiResponse(true, vr));
            }
            else
            {
                return this.BadRequest(new ApiResponse(false, vr));
            }
        }

        // GET api/LoginWizard/GetSecurityQuestionsByUserId/id

        /// <summary>
        /// The get security questions by user id.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        [Route("GetSecurityQuestionsByUserId/{userId}")]
        public async Task<IActionResult> GetSecurityQuestionsByUserId(int userId)
        {
            var userSecurityQuestions = await this.loginWizardService.GetSecurityQuestionsByUser(userId);

            return this.Ok(userSecurityQuestions);
        }

        // GET api/LoginWizard/GetSecurityQuestions

        /// <summary>
        /// The get security questions.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("GetSecurityQuestions")]
        public IActionResult GetSecurityQuestions()
        {
            var securityQuestions = this.loginWizardService.GetSecurityQuestions();

            return this.Ok(securityQuestions);
        }

        // Patch api/LoginWizard/StartWizardForUser

        /// <summary>
        /// Update the current user to indicate Logon Wizard stated.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPatch]
        [Route("StartWizardForUser")]
        public async Task<IActionResult> StartWizardForUser([FromBody] UserBasic user)
        {
            if (user.Id != this.CurrentUserId)
            {
                return this.BadRequest();
            }

            await this.loginWizardService.StartWizardForUser(this.CurrentUserId);
            return this.Ok();
        }

        // PUT api/LoginWizard/CompleteWizardForUser

        /// <summary>
        /// Update the current user to indicate Logon Wizard completed.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPatch]
        [Route("CompleteWizardForUser")]
        public async Task<IActionResult> CompleteWizardForUser([FromBody] UserBasic user)
        {
            if (user.Id != this.CurrentUserId)
            {
                return this.BadRequest();
            }

            await this.loginWizardService.CompleteWizardForUser(this.CurrentUserId);
            return this.Ok();
        }
    }
}