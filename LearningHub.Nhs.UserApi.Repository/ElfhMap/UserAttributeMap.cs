// <copyright file="UserAttributeMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user attribute map.
    /// </summary>
    public class UserAttributeMap : BaseEntityMap<UserAttribute>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void InternalMap(EntityTypeBuilder<UserAttribute> builder)
        {
            builder.ToTable("userAttributeTBL", "dbo");

            builder.Property(p => p.Id)
                .HasColumnName("userAttributeId");

            builder.Property(p => p.UserId)
                .HasColumnName("userId");

            builder.Property(p => p.AttributeId)
                .HasColumnName("attributeId");

            builder.Property(p => p.IntValue)
                .HasColumnName("intValue");

            builder.Property(p => p.TextValue)
                .HasColumnName("textValue")
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(p => p.BooleanValue)
                .HasColumnName("booleanValue");

            builder.Property(p => p.DateValue)
                .HasColumnName("dateValue");

            builder.HasOne(d => d.Attribute)
                .WithMany(m => m.UserAttributes)
                .HasForeignKey(f => f.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userAttributeTBL_attributeId");

            builder.HasOne(p => p.User)
                .WithMany(m => m.UserAttributes)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userAttributeTBL_userId");
        }
    }
}
