// <copyright file="TermsAndConditionsMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The terms and conditions map.
    /// </summary>
    public class TermsAndConditionsMap : BaseEntityMap<TermsAndConditions>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<TermsAndConditions> modelBuilder)
        {
            modelBuilder.ToTable("TermsAndConditionsTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("termsAndConditionsId");
            modelBuilder.Property(e => e.CreatedDate).HasColumnName("createdDate");
            modelBuilder.Property(e => e.Description).HasColumnName("description").HasMaxLength(512);
            modelBuilder.Property(e => e.Details).HasColumnName("details");
            modelBuilder.Property(e => e.TenantId).HasColumnName("tenantId");
            modelBuilder.Property(e => e.Active).HasColumnName("active");
            modelBuilder.Property(e => e.Reportable).HasColumnName("reportable");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
