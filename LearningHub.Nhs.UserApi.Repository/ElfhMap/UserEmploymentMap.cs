// <copyright file="UserEmploymentMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user employment map.
    /// </summary>
    public class UserEmploymentMap : BaseEntityMap<UserEmployment>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<UserEmployment> modelBuilder)
        {
            modelBuilder.ToTable("userEmploymentTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userEmploymentId");

            modelBuilder.Property(e => e.UserId).HasColumnName("userId");
            modelBuilder.Property(e => e.JobRoleId).HasColumnName("jobRoleId");
            modelBuilder.Property(e => e.SpecialtyId).HasColumnName("specialtyId");
            modelBuilder.Property(e => e.GradeId).HasColumnName("gradeId");
            modelBuilder.Property(e => e.SchoolId).HasColumnName("schoolId");
            modelBuilder.Property(e => e.LocationId).HasColumnName("locationId");
            modelBuilder.Property(e => e.MedicalCouncilId).HasColumnName("medicalCouncilId");
            modelBuilder.Property(e => e.MedicalCouncilNo).HasColumnName("medicalCouncilNo").HasMaxLength(50);
            modelBuilder.Property(e => e.StartDate).HasColumnName("startDate");
            modelBuilder.Property(e => e.EndDate).HasColumnName("endDate");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.Archived).HasColumnName("archived");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("AmendDate");

            modelBuilder.HasOne(d => d.User)
                        .WithMany(p => p.UserEmployment)
                        .HasForeignKey(d => d.UserId)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasOne(d => d.Location)
                        .WithMany(p => p.UserEmployment)
                        .HasForeignKey(d => d.LocationId)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasQueryFilter(e => !e.Deleted);
        }
    }
}
