namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The grade map.
    /// </summary>
    public class GradeMap : BaseEntityMap<Grade>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<Grade> modelBuilder)
        {
            modelBuilder.ToTable("gradeTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("gradeId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Name).HasColumnName("gradeName");

            modelBuilder.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
