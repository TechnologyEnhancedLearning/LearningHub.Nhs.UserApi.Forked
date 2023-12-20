// <copyright file="StaffGroupMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The staff group map.
    /// </summary>
    public class StaffGroupMap : BaseEntityMap<StaffGroup>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<StaffGroup> modelBuilder)
        {
            modelBuilder.ToTable("staffGroupTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("staffGroupId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Name).HasColumnName("staffGroupName");

            modelBuilder.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

            modelBuilder.Property(e => e.InternalUsersOnly).HasColumnName("internalUsersOnly");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
