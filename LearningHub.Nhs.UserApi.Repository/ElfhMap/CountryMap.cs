namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The country map.
    /// </summary>
    public class CountryMap : BaseEntityMap<Country>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<Country> modelBuilder)
        {
            modelBuilder.ToTable("countryTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("countryId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Name).HasColumnName("countryName");

            modelBuilder.Property(e => e.Alpha2).HasColumnName("alpha2");

            modelBuilder.Property(e => e.Alpha3).HasColumnName("alpha3");

            modelBuilder.Property(e => e.Numeric).HasColumnName("numeric");

            modelBuilder.Property(e => e.EUVatRate).HasColumnName("EUVatRate");

            modelBuilder.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
