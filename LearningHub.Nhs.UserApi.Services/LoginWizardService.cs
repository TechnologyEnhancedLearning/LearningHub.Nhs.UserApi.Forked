namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using elfhHub.Nhs.Models.Enums;
    using elfhHub.Nhs.Models.Validation;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using LearningHub.Nhs.UserApi.Shared.Configuration;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The login wizard service.
    /// </summary>
    public class LoginWizardService : ILoginWizardService
    {
        private readonly IElfhUserRepository elfhUserRepository;
        private readonly ILoginWizardStageActivityRepository loginWizardStageActivityRepository;
        private readonly IUserEmploymentRepository userEmploymentRepository;
        private readonly ITermsAndConditionsRepository termsAndConditionsRepository;
        private readonly ISecurityQuestionRepository securityQuestionRepository;
        private readonly IUserSecurityQuestionRepository userSecurityQuestionRepository;
        private readonly ILoginWizardRuleRepository loginWizardRuleRepository;
        private readonly ILoginWizardStageRepository loginWizardStageRepository;
        private readonly IUserHistoryService userHistoryService;
        private readonly IUserRoleUpgradeRepository userRoleUpgradeRepository;
        private readonly IOptions<Settings> settings;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginWizardService"/> class.
        /// </summary>
        /// <param name="elfhUserRepository">The elfh user repository.</param>
        /// <param name="loginWizardStageActivityRepository">The login wizard stage activity repository.</param>
        /// <param name="userEmploymentRepository">The user employment repository.</param>
        /// <param name="termsAndConditionsRepository">The terms and conditions repository.</param>
        /// <param name="securityQuestionRepository">The security question repository.</param>
        /// <param name="userSecurityQuestionRepository">The user security question repository.</param>
        /// <param name="loginWizardRuleRepository">The login wizard rule repository.</param>
        /// <param name="loginWizardStageRepository">The login wizard stage repository.</param>
        /// <param name="userHistoryService">The user history service.</param>
        /// <param name="userRoleUpgradeRepository">The user role upgrade repository.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="mapper">The mapper.</param>
        public LoginWizardService(
            IElfhUserRepository elfhUserRepository,
            ILoginWizardStageActivityRepository loginWizardStageActivityRepository,
            IUserEmploymentRepository userEmploymentRepository,
            ITermsAndConditionsRepository termsAndConditionsRepository,
            ISecurityQuestionRepository securityQuestionRepository,
            IUserSecurityQuestionRepository userSecurityQuestionRepository,
            ILoginWizardRuleRepository loginWizardRuleRepository,
            ILoginWizardStageRepository loginWizardStageRepository,
            IUserHistoryService userHistoryService,
            IUserRoleUpgradeRepository userRoleUpgradeRepository,
            IOptions<Settings> settings,
            IMapper mapper)
        {
            this.elfhUserRepository = elfhUserRepository;
            this.loginWizardStageActivityRepository = loginWizardStageActivityRepository;
            this.userEmploymentRepository = userEmploymentRepository;
            this.termsAndConditionsRepository = termsAndConditionsRepository;
            this.securityQuestionRepository = securityQuestionRepository;
            this.userSecurityQuestionRepository = userSecurityQuestionRepository;
            this.loginWizardRuleRepository = loginWizardRuleRepository;
            this.loginWizardStageRepository = loginWizardStageRepository;
            this.userRoleUpgradeRepository = userRoleUpgradeRepository;
            this.userHistoryService = userHistoryService;
            this.settings = settings;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task StartWizardForUser(int userId)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(userId);
            user.LoginWizardInProgress = true;
            await this.elfhUserRepository.UpdateAsync(userId, user);
        }

        /// <inheritdoc/>
        public async Task CompleteWizardForUser(int userId)
        {
            var user = await this.elfhUserRepository.GetByIdAsync(userId);
            user.LoginWizardInProgress = false;
            user.LastLoginWizardCompleted = DateTimeOffset.Now;
            await this.elfhUserRepository.UpdateAsync(userId, user);

            // Add UserHistory entry
            UserHistoryViewModel userHistory = new UserHistoryViewModel()
            {
                UserId = userId,
                Detail = "User completed Login Wizard.",
                UserHistoryTypeId = (int)UserHistoryType.LoginWizardCompleted,
            };
            var uhCreateResult = await this.userHistoryService.CreateAsync(userHistory, userId);
            if (!uhCreateResult.IsValid)
            {
                throw new Exception(string.Join(':', uhCreateResult.Details.ToArray()));
            }
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> CreateStageActivity(int stageId, int userId)
        {
            LoginWizardStageActivity loginWizardStageActivity = new LoginWizardStageActivity()
            {
                LoginWizardStageId = stageId,
                UserId = userId,
            };
            var retVal = await this.ValidateAsync(loginWizardStageActivity);

            if (retVal.IsValid)
            {
                retVal.CreatedId = await this.loginWizardStageActivityRepository.CreateAsync(loginWizardStageActivity);
            }

            return retVal;
        }

        /// <inheritdoc/>
        public async Task<LoginWizardStagesViewModel> GetLoginWizardByUserIdAsync(int userId)
        {
            // upgrade date to full account (blue user)
            DateTimeOffset? userToFullAccountUpgradeDate = null;

            var userRoleUpgrade = await this.userRoleUpgradeRepository.GetByUserIdAsync(userId).Where(n => n.UpgradeDate != null && n.UserHistoryTypeId == (int)UserHistoryType.UserRoleUpgarde).FirstOrDefaultWithNoLockAsync();
            var stageActivity = this.loginWizardStageActivityRepository.GetByUser(userId).OrderByDescending(sa => sa.Id);
            var passwordResetActivity = stageActivity.Where(a => a.LoginWizardStageId == 2).FirstOrDefaultWithNoLock();
            var jobRoleDetailsActivity = stageActivity.Where(a => a.LoginWizardStageId == 4).FirstOrDefaultWithNoLock();
            var placeOfWorkDetailsActivity = stageActivity.Where(a => a.LoginWizardStageId == 5).FirstOrDefaultWithNoLock();
            var personalDetailsActivity = stageActivity.Where(a => a.LoginWizardStageId == 6).FirstOrDefaultWithNoLock();
            var userEmployment = await this.userEmploymentRepository.GetPrimaryForUser(userId);

            bool firstTimeLogin = true;
            bool restrictToSso = false;
            bool passwordResetRequired = false;
            bool hasJobRole = false;
            bool isStudentOrTrainee = false;
            bool placeOfWorkSpecified = false;
            bool isConsultant = false;

            // place of work activity after upgrade to full account(blue user)
            bool noPlaceOfWorkActivityAfterFullAccountUpgrade = default;

            // any upgrade to full account (blue user)
            userToFullAccountUpgradeDate = userRoleUpgrade?.UpgradeDate;

            if (userEmployment != null)
            {
                firstTimeLogin = userEmployment.User.LastLoginWizardCompleted == null;
                restrictToSso = userEmployment.User.RestrictToSso;
                passwordResetRequired = userEmployment.User.MustChangeNextLogin;
                hasJobRole = userEmployment.JobRoleId != null;
                isStudentOrTrainee = new List<int> { 1001, 1003, 1327, 1328 }.Contains(userEmployment.JobRoleId ?? 0);
                placeOfWorkSpecified = userEmployment.LocationId > 1;
                isConsultant = userEmployment.GradeId == 175;
            }

            bool activationPeriodElapsed = false;
            bool eIntegrityUser = this.elfhUserRepository.IsEIntegrityUser(userId);
            bool basicUser = this.elfhUserRepository.IsBasicUser(userId);

            var latestTsAndCs = await this.termsAndConditionsRepository.LatestVersionAsync(this.settings.Value.LearningHubTenantId);
            var latestTsAndCsAcepted = await this.termsAndConditionsRepository.LatestVersionAcceptedAsync(userId, this.settings.Value.LearningHubTenantId);
            bool termsAccepted = latestTsAndCs != null && latestTsAndCsAcepted != null && latestTsAndCsAcepted.Id >= latestTsAndCs.Id;

            int securityQuestionsCompletedCount = await this.userSecurityQuestionRepository.GetByUserId(userId).CountWithNoLockAsync();
            int securityQuestionsRequired = this.settings.Value.SecurityQuestionsRequired;

            var activeRules = this.loginWizardRuleRepository.GetActive();
            List<LoginWizardRule> failedRules = new List<LoginWizardRule>();

            foreach (var rule in activeRules)
            {
                // Rule 1: Terms and Conditions Accepted -first login
                if (rule.Id == 1 && !termsAccepted && firstTimeLogin)
                {
                    failedRules.Add(rule);
                }

                // Rule 2: Terms and Conditions Accepted
                if (rule.Id == 2 && !termsAccepted && !firstTimeLogin)
                {
                    failedRules.Add(rule);
                }

                // Rule 3: Password Reset Required - first time login
                // Doesn't apply to SSO
                if (rule.Id == 3 && !restrictToSso && firstTimeLogin && passwordResetActivity == null)
                {
                    failedRules.Add(rule);
                }

                // Rule 4: Password Reset Required
                // Doesn't apply to SSO
                if (rule.Id == 4 && passwordResetRequired && !restrictToSso && !firstTimeLogin)
                {
                    failedRules.Add(rule);
                }

                // Rule 5: Security Questions Completed - first time login
                // Doesn't apply to SSO
                if (rule.Id == 5 && securityQuestionsCompletedCount < securityQuestionsRequired && !restrictToSso && firstTimeLogin)
                {
                    failedRules.Add(rule);
                }

                // Rule 6: Security Questions Completed
                // Doesn't apply to SSO
                if (rule.Id == 6 && securityQuestionsCompletedCount < securityQuestionsRequired && !restrictToSso && !firstTimeLogin)
                {
                    failedRules.Add(rule);
                }

                // Rule 7: Job Role Specified - first login
                if (rule.Id == 7 && firstTimeLogin && !eIntegrityUser && !hasJobRole && jobRoleDetailsActivity == null)
                {
                    failedRules.Add(rule);
                }

                // Rule 8: Job Role Specified - Trainee / Students
                if (rule.Id == 8)
                {
                    activationPeriodElapsed = jobRoleDetailsActivity == null ||
                                                   jobRoleDetailsActivity.ActivityDatetime.AddDays((int)rule.ActivationPeriod) < DateTimeOffset.Now;
                    if (activationPeriodElapsed && isStudentOrTrainee && !eIntegrityUser && !basicUser)
                    {
                        failedRules.Add(rule);
                    }
                }

                // Rule 9: Place of Work Specified - first login
                if (rule.Id == 9 && firstTimeLogin && !eIntegrityUser && placeOfWorkDetailsActivity == null && !basicUser)
                {
                    failedRules.Add(rule);
                }

                // Rule 10: Place of Work Specified - eLfH User
                if (rule.Id == 10)
                {
                    activationPeriodElapsed = placeOfWorkDetailsActivity == null ||
                                                   placeOfWorkDetailsActivity.ActivityDatetime.AddDays((int)rule.ActivationPeriod) < DateTimeOffset.Now;

                    // check no place of work activity date by the user after full account (blue user) upgrade date
                    if (userToFullAccountUpgradeDate.HasValue)
                    {
                        noPlaceOfWorkActivityAfterFullAccountUpgrade = placeOfWorkDetailsActivity == null ||
                                                   placeOfWorkDetailsActivity?.ActivityDatetime < userToFullAccountUpgradeDate.Value;
                    }

                    if (!eIntegrityUser && !basicUser)
                    {
                        if (noPlaceOfWorkActivityAfterFullAccountUpgrade)
                        {
                            failedRules.Add(rule);
                        }
                        else if (!placeOfWorkSpecified && activationPeriodElapsed && !firstTimeLogin)
                        {
                            failedRules.Add(rule);
                        }
                    }
                }

                // Rule 11: Personal Details Correct - first login
                if (rule.Id == 11 && firstTimeLogin && personalDetailsActivity == null && !basicUser)
                {
                    failedRules.Add(rule);
                }

                // Rule 12: Personal Details Correct - Consultants
                if (rule.Id == 12)
                {
                    activationPeriodElapsed = personalDetailsActivity == null ||
                                                   personalDetailsActivity.ActivityDatetime.AddDays((int)rule.ActivationPeriod) < DateTimeOffset.Now;
                    if (isConsultant && activationPeriodElapsed && !firstTimeLogin && !basicUser)
                    {
                        failedRules.Add(rule);
                    }
                }

                // Rule 13: Personal Details Correct - Non-Consultants
                if (rule.Id == 13)
                {
                    activationPeriodElapsed = personalDetailsActivity == null ||
                                                   personalDetailsActivity.ActivityDatetime.AddDays((int)rule.ActivationPeriod) < DateTimeOffset.Now;
                    if (!isConsultant && activationPeriodElapsed && !firstTimeLogin && !basicUser)
                    {
                        failedRules.Add(rule);
                    }
                }

                //// Rule 14: Technical check - first login -- NOT NEEDED IN Learning Hub logon wizard
                ////if (rule.Id == 14 && firstTimeLogin)
                ////{
                ////    failedRules.Add(rule);
                ////}

                // Rule 15: Check if User has inactive location in their current user employment
                if (rule.Id == 15 &&

                            userEmployment != null && !userEmployment.Location.Active
                        &&
                        (
                            userEmployment.EndDate == null
                            ||
                            userEmployment.EndDate >= DateTimeOffset.Now))
                {
                    failedRules.Add(rule);
                }

                // Rule 16: Job Role verfication after bulk upload override
                if (rule.Id == 16 && !firstTimeLogin && jobRoleDetailsActivity == null)
                {
                    failedRules.Add(rule);
                }
            }

            LoginWizardStagesViewModel loginWizardRulesViewModel = new LoginWizardStagesViewModel();
            loginWizardRulesViewModel.IsFirstLogin = firstTimeLogin;
            loginWizardRulesViewModel.LoginWizardStages = this.loginWizardStageRepository.GetAll()
                                                                                     .Where(s => failedRules.Select(r => r.LoginWizardStageId)
                                                                                     .Contains(s.Id)).ToListWithNoLock();

            loginWizardRulesViewModel.LoginWizardStagesCompleted = new List<LoginWizardStage>();

            foreach (var stage in loginWizardRulesViewModel.LoginWizardStages)
            {
                stage.LoginWizardRules = failedRules.Where(s => s.LoginWizardStageId == stage.Id).ToList();
            }

            return loginWizardRulesViewModel;
        }

        /// <inheritdoc/>
        public List<SecurityQuestion> GetSecurityQuestions()
        {
            return this.securityQuestionRepository.GetAll().ToListWithNoLock();
        }

        /// <inheritdoc/>
        public async Task<SecurityQuestionsViewModel> GetSecurityQuestionsByUser(int userId)
        {
            var model = new SecurityQuestionsViewModel();
            model.SecurityQuestions = this.securityQuestionRepository.GetAll()
                                                                 .Select(q => new SelectListItem() { Value = q.Id.ToString(), Text = q.Text })
                                                                  .ToListWithNoLock();

            var userQuestions = this.userSecurityQuestionRepository.GetByUserId(userId);
            model.UserSecurityQuestions = await this.mapper.ProjectTo<UserSecurityQuestionViewModel>(userQuestions).ToListWithNoLockAsync();

            return model;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(LoginWizardStageActivity loginWizardStageActivity)
        {
            var loginWizardStageActivityValidator = new LoginWizardStageActivityValidator();
            var clientValidationResult = await loginWizardStageActivityValidator.ValidateAsync(loginWizardStageActivity);

            var retVal = new LearningHubValidationResult(clientValidationResult);

            return retVal;
        }
    }
}
