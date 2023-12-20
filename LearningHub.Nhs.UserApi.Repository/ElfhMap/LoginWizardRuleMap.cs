// <copyright file="LoginWizardRuleMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The login wizard rule map.
    /// </summary>
    public class LoginWizardRuleMap : BaseEntityMap<LoginWizardRule>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<LoginWizardRule> modelBuilder)
        {
            modelBuilder.ToTable("loginWizardRuleTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("loginWizardRuleId");
            modelBuilder.Property(e => e.LoginWizardStageId).HasColumnName("loginWizardStageId");
            modelBuilder.Property(e => e.LoginWizardRuleCategoryId).HasColumnName("loginWizardRuleCategoryId");
            modelBuilder.Property(e => e.Description).HasColumnName("description").HasMaxLength(128);
            modelBuilder.Property(e => e.ReasonDisplayText).HasColumnName("reasonDisplayText").HasMaxLength(1024);
            modelBuilder.Property(e => e.ActivationPeriod).HasColumnName("activationPeriod");
            modelBuilder.Property(e => e.Required).HasColumnName("required");
            modelBuilder.Property(e => e.Active).HasColumnName("active");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
