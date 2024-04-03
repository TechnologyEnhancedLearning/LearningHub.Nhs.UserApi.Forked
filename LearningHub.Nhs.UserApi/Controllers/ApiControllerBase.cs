namespace LearningHub.Nhs.UserApi.Controllers
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Extensions;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The api elfh controller base.
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        private const int PortalAdminId = 4;

        /// <summary>
        /// The current user.
        /// </summary>
        private User currentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="userService">
        /// The user service.
        /// </param>
        protected ApiControllerBase(IElfhUserService userService)
        {
            this.ElfhUserService = userService;
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                var currentUserId = this.User.Identity.GetCurrentUserId();
                return currentUserId == 0 ? PortalAdminId : currentUserId;
            }
        }

        /// <summary>
        /// Gets the elfh user service.
        /// </summary>
        protected IElfhUserService ElfhUserService { get; }

        /// <summary>
        /// The get current user async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        internal async Task<User> GetCurrentUserAsync()
        {
            if (this.currentUser == null)
            {
                this.currentUser = await this.ElfhUserService.GetByIdAsync(this.CurrentUserId);
                this.HttpContext.User.Identity.GetCurrentUserId();
            }

            return this.currentUser;
        }
    }
}