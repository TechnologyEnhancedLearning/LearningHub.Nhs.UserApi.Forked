// <copyright file="UserHistoryAttributeMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user history attribute map.
    /// </summary>
    public class UserHistoryAttributeMap : BaseEntityMap<UserHistoryAttribute>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void InternalMap(EntityTypeBuilder<UserHistoryAttribute> modelBuilder)
        {
            modelBuilder.ToTable("userHistoryAttributeTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userHistoryAttributeId");

            modelBuilder.Property(e => e.UserHistoryId).HasColumnName("userHistoryId");

            modelBuilder.HasOne(d => d.UserHistory)
                .WithMany(p => p.UserHistoryAttribute)
                .HasForeignKey(d => d.UserHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userHistoryAttributeTBL_userHistoryTBL");

            modelBuilder.Property(e => e.AttributeId).HasColumnName("attributeId");

            modelBuilder.Property(e => e.IntValue).HasColumnName("intValue");

            modelBuilder.Property(e => e.TextValue).HasColumnName("textValue").HasMaxLength(1000);

            modelBuilder.Property(e => e.BooleanValue).HasColumnName("booleanValue");

            modelBuilder.Property(e => e.DateValue).HasColumnName("dateValue");
        }
    }
}