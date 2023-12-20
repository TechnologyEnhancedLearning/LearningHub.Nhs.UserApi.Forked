// <copyright file="GdcRegisterMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The gdc register map.
    /// </summary>
    public class GdcRegisterMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<GdcRegister>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<GdcRegister> modelBuilder)
        {
            modelBuilder.ToTable("gdcRegisterTBL", "dbo")
                .HasKey(e => e.RegNumber);

            modelBuilder.Property(e => e.RegNumber)
                .HasColumnName("reg_number")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Dentist).HasColumnName("Dentist");

            modelBuilder.Property(e => e.Title)
                .HasColumnName("Title")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Surname)
                .HasColumnName("Surname")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.Forenames)
                .HasColumnName("Forenames")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.Honorifics)
                .HasColumnName("honorifics")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.HouseName)
                .HasColumnName("house_name")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.AddressLine1)
                .HasColumnName("address_line1")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.AddressLine2)
                .HasColumnName("address_line2")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.AddressLine3)
                .HasColumnName("address_line3")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.AddressLine4)
                .HasColumnName("address_line4")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.Town)
                .HasColumnName("Town")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.County)
                .HasColumnName("County")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.PostCode)
                .HasColumnName("PostCode")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Country)
                .HasColumnName("Country")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Regdate)
                .HasColumnName("regdate")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Qualifications)
                .HasColumnName("qualifications")
                .HasMaxLength(1000);

            modelBuilder.Property(e => e.DcpTitles)
                .HasColumnName("dcp_titles")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Specialties)
                .HasColumnName("specialties")
                .HasMaxLength(100);

            modelBuilder.Property(e => e.Condition)
                .HasColumnName("condition")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Suspension)
                .HasColumnName("suspension")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.DateProcessed).HasColumnName("dateProcessed");

            modelBuilder.Property(e => e.Action)
                .HasColumnName("action")
                .HasMaxLength(1);
        }
    }
}
