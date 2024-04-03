namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The job role map.
    /// </summary>
    public class JobRoleMap : BaseEntityMap<JobRole>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<JobRole> modelBuilder)
        {
            modelBuilder.ToTable("jobRoleTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("jobRoleId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.StaffGroupId).HasColumnName("staffGroupId");

            modelBuilder.Property(e => e.Name).HasColumnName("jobRoleName");

            modelBuilder.Property(e => e.MedicalCouncilId).HasColumnName("medicalCouncilId");

            modelBuilder.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
