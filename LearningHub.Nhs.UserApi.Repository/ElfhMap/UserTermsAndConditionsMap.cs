// <copyright file="UserTermsAndConditionsMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user terms and conditions map.
    /// </summary>
    public class UserTermsAndConditionsMap : BaseEntityMap<UserTermsAndConditions>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<UserTermsAndConditions> modelBuilder)
        {
            modelBuilder.ToTable("UserTermsAndConditionsTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userTermsAndConditionsId");
            modelBuilder.Property(e => e.TermsAndConditionsId).HasColumnName("termsAndConditionsId");
            modelBuilder.Property(e => e.UserId).HasColumnName("userId");
            modelBuilder.Property(e => e.AcceptanceDate).HasColumnName("acceptanceDate");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
