namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The specialty map.
    /// </summary>
    public class SpecialtyMap : BaseEntityMap<Specialty>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<Specialty> modelBuilder)
        {
            modelBuilder.ToTable("specialtyTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("specialtyId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Name).HasColumnName("specialtyName");

            modelBuilder.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
