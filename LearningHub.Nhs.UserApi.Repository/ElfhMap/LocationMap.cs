// <copyright file="LocationMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The location map.
    /// </summary>
    public class LocationMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<Location>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<Location> modelBuilder)
        {
            modelBuilder.ToTable("locationTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("locationId");

            modelBuilder.Property(e => e.Code)
                .HasColumnName("locationCode")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Name)
                .HasColumnName("locationName")
                .HasMaxLength(200);

            modelBuilder.Property(e => e.SubName)
                .HasColumnName("locationSubName")
                .HasMaxLength(200);

            modelBuilder.Property(e => e.TypeId)
                .HasColumnName("locationTypeId");

            modelBuilder.Property(e => e.Address1)
                .HasColumnName("address1")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Address2)
                .HasColumnName("address2")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Address3)
                .HasColumnName("address3")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Address4)
                .HasColumnName("address4")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Town)
                .HasColumnName("town")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.County)
                .HasColumnName("county")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.PostCode)
                .HasColumnName("postCode")
                .HasMaxLength(8);

            modelBuilder.Property(e => e.Telephone)
                .HasColumnName("telephone")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Acute)
                .HasColumnName("acute");

            modelBuilder.Property(e => e.Ambulance)
                .HasColumnName("ambulance");

            modelBuilder.Property(e => e.Mental)
                .HasColumnName("mental");

            modelBuilder.Property(e => e.Care)
                .HasColumnName("care");

            modelBuilder.Property(e => e.MainHosp)
                .HasColumnName("mainHosp");

            modelBuilder.Property(e => e.NhsCode)
                .HasColumnName("nhsCode")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.ParentId)
                .HasColumnName("parentId");

            modelBuilder.Property(e => e.DataSource)
                .HasColumnName("dataSource")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Active)
                .HasColumnName("active");

            modelBuilder.Property(e => e.ImportExclusion)
                .HasColumnName("importExclusion");

            modelBuilder.Property(e => e.Depth)
                .HasColumnName("depth");

            modelBuilder.Property(e => e.Lineage)
                .HasColumnName("lineage");

            modelBuilder.Property(e => e.Created)
                .HasColumnName("created");

            modelBuilder.Property(e => e.Updated)
                .HasColumnName("updated");

            modelBuilder.Property(e => e.ArchivedDate)
                .HasColumnName("archivedDate");

            modelBuilder.Property(e => e.CountryId)
                .HasColumnName("countryId");

            modelBuilder.Property(e => e.IguId)
                .HasColumnName("iguId");

            modelBuilder.Property(e => e.LetbId)
                .HasColumnName("letbId");

            modelBuilder.Property(e => e.CcgId)
                .HasColumnName("ccgId");

            modelBuilder.Property(e => e.HealthServiceId)
                .HasColumnName("healthServiceId");

            modelBuilder.Property(e => e.HealthBoardId)
                .HasColumnName("healthBoardId");

            modelBuilder.Property(e => e.PrimaryTrustId)
                .HasColumnName("primaryTrustId");

            modelBuilder.Property(e => e.SecondaryTrustId)
                .HasColumnName("secondaryTrustId");

            modelBuilder.Property(e => e.IslandId)
                .HasColumnName("islandId");

            modelBuilder.Property(e => e.OtherNHSOrganisationId)
                .HasColumnName("otherNHSOrganisationId");

            modelBuilder.HasOne(d => d.Type)
                .WithMany(p => p.Locations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
