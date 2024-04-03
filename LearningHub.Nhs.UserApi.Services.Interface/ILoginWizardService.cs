namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.Models.Validation;

    /// <summary>
    /// The LoginWizardService interface.
    /// </summary>
    public interface ILoginWizardService
    {
        /// <summary>
        /// The get login wizard by user id async.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LoginWizardStagesViewModel> GetLoginWizardByUserIdAsync(int userId);

        /// <summary>
        /// The create stage activity.
        /// </summary>
        /// <param name="stageId">
        /// The stage id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<LearningHubValidationResult> CreateStageActivity(int stageId, int userId);

        /// <summary>
        /// The get security questions by user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<SecurityQuestionsViewModel> GetSecurityQuestionsByUser(int userId);

        /// <summary>
        /// The get security questions.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        List<SecurityQuestion> GetSecurityQuestions();

        /// <summary>
        /// The start wizard for user.
        /// </summary>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task StartWizardForUser(int currentUserId);

        /// <summary>
        /// The complete wizard for user.
        /// </summary>
        /// <param name="currentUserId">
        /// The current user id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CompleteWizardForUser(int currentUserId);
    }
}
