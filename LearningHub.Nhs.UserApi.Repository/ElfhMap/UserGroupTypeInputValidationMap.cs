// <copyright file="UserGroupTypeInputValidationMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user group type input validation map.
    /// </summary>
    public class UserGroupTypeInputValidationMap : BaseEntityMap<UserGroupTypeInputValidation>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<UserGroupTypeInputValidation> modelBuilder)
        {
            modelBuilder.ToTable("userGroupTypeInputValidationTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userGroupTypeInputValidationId");

            modelBuilder.Property(e => e.UserGroupId).HasColumnName("userGroupId");

            modelBuilder.Property(e => e.UserGroupTypePrefix).HasColumnName("userGroupTypePrefix").HasMaxLength(10);

            modelBuilder.Property(e => e.UserGroupTypeId).HasColumnName("userGroupTypeId");

            modelBuilder.Property(e => e.ValidationTextValue).HasColumnName("validationTextValue").HasMaxLength(1000);

            modelBuilder.Property(e => e.ValidationMethod).HasColumnName("validationMethod");

            modelBuilder.Property(e => e.CreatedUserId).HasColumnName("createdUserId");

            modelBuilder.Property(e => e.CreatedDate).HasColumnName("createdDate");
        }
    }
}
