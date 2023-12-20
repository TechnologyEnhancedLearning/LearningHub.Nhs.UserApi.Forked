// <copyright file="SecurityQuestionMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The security question map.
    /// </summary>
    public class SecurityQuestionMap : BaseEntityMap<SecurityQuestion>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<SecurityQuestion> modelBuilder)
        {
            modelBuilder.ToTable("securityQuestionTBL", "dbo");
            modelBuilder.Property(e => e.Id).HasColumnName("securityQuestionId");
            modelBuilder.Property(e => e.Text).HasColumnName("questionText");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
