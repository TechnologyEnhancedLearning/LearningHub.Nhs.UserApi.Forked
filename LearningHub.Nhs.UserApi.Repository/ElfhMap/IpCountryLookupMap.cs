namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The ip country lookup map.
    /// </summary>
    public class IpCountryLookupMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<IpCountryLookup>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<IpCountryLookup> modelBuilder)
        {
            modelBuilder.ToTable("ipCountryLookupTBL", "dbo").HasNoKey();

            modelBuilder.Property(e => e.FromIp).HasColumnName("fromIP");

            modelBuilder.Property(e => e.ToIp).HasColumnName("toIP");

            modelBuilder.Property(e => e.Country).HasColumnName("country");

            modelBuilder.Property(e => e.FromInt).HasColumnName("fromInt");

            modelBuilder.Property(e => e.ToInt).HasColumnName("toInt");
        }
    }
}