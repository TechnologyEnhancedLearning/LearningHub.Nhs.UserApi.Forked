// <copyright file="UserUserGroupMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user user group map.
    /// </summary>
    public class UserUserGroupMap : BaseEntityMap<UserUserGroup>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void InternalMap(EntityTypeBuilder<UserUserGroup> modelBuilder)
        {
            modelBuilder.ToTable("UserUserGroupTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userUserGroupId");

            modelBuilder.Property(e => e.UserId).HasColumnName("UserId");

            modelBuilder.Property(e => e.UserGroupId).HasColumnName("UserGroupId");

            modelBuilder.HasOne(d => d.UserGroup)
                .WithMany(p => p.UserUserGroup)
                .HasForeignKey(d => d.UserGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userUserGroup_userGroup");

            modelBuilder.HasOne(d => d.User)
                .WithMany(p => p.UserUserGroup)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userUserGroup_user");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
        }
    }
}