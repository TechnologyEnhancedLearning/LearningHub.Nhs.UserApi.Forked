namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The location type map.
    /// </summary>
    public class LocationTypeMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<LocationType>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<LocationType> modelBuilder)
        {
            modelBuilder.ToTable("locationTypeTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("locationTypeID");

            modelBuilder.Property(e => e.Name)
                .HasColumnName("locationType");

            modelBuilder.Property(e => e.CountryId)
                .HasColumnName("countryId");

            modelBuilder.Property(e => e.HealthService)
                .HasColumnName("healthService");

            modelBuilder.Property(e => e.HealthBoard)
                .HasColumnName("healthBoard");

            modelBuilder.Property(e => e.PrimaryTrust)
                .HasColumnName("primaryTrust");

            modelBuilder.Property(e => e.SecondaryTrust)
                .HasColumnName("secondaryTrust");
        }
    }
}
