// <copyright file="ILoginWizardRuleRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The LoginWizardRuleRepository interface.
    /// </summary>
    public interface ILoginWizardRuleRepository : IGenericElfhRepository<LoginWizardRule>
    {
        /// <summary>
        /// The get active.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<LoginWizardRule> GetActive();
    }
}
