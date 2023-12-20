// <copyright file="MedicalCouncilMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The medical council map.
    /// </summary>
    public class MedicalCouncilMap : BaseEntityMap<MedicalCouncil>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<MedicalCouncil> modelBuilder)
        {
            modelBuilder.ToTable("medicalCouncilTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("medicalCouncilId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Name).HasColumnName("medicalCouncilName");
            modelBuilder.Property(e => e.Code).HasColumnName("medicalCouncilCode");
            modelBuilder.Property(e => e.UploadPrefix).HasColumnName("uploadPrefix");

            modelBuilder.Property(e => e.IncludeOnCerts).HasColumnName("includeOnCerts");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
