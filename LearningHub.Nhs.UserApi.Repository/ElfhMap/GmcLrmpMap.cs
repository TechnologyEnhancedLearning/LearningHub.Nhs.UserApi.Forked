namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The gmc lrmp map.
    /// </summary>
    public class GmcLrmpMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<GmcLrmp>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<GmcLrmp> modelBuilder)
        {
            modelBuilder.ToTable("gmclrmpTBL", "dbo")
                .HasKey(e => e.GmcRefNo);

            modelBuilder.Property(e => e.GmcRefNo)
                .HasColumnName("GMC_Ref_No")
                .HasMaxLength(50);

            modelBuilder.Property(e => e.Surname)
                .HasColumnName("Surname")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.GivenName)
                .HasColumnName("Given_Name")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.YearOfQualification).HasColumnName("Year_Of_Qualification");

            modelBuilder.Property(e => e.GPRegisterDate)
                .HasColumnName("GP_Register_Date")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.RegistrationStatus)
                .HasColumnName("Registration_Status")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.OtherNames)
                .HasColumnName("Other_Names")
                .HasMaxLength(255);

            modelBuilder.Property(e => e.DateProcessed).HasColumnName("dateProcessed");

            modelBuilder.Property(e => e.Action)
                .HasColumnName("action")
                .HasMaxLength(1);
        }
    }
}