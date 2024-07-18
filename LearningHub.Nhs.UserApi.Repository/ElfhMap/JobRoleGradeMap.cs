namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The job role grade map.
    /// </summary>
    public class JobRoleGradeMap : BaseEntityMap<JobRoleGrade>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<JobRoleGrade> modelBuilder)
        {
            modelBuilder.ToTable("jobRoleGradeTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("jobRoleGradeId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.JobRoleId).HasColumnName("jobRoleId");

            modelBuilder.Property(e => e.GradeId).HasColumnName("gradeId");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
