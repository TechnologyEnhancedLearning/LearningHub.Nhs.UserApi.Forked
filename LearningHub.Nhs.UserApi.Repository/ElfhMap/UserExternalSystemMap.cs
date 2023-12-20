// <copyright file="UserExternalSystemMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// UserExternalSystemMap.
    /// </summary>
    public class UserExternalSystemMap : BaseEntityMap<UserExternalSystem>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<UserExternalSystem> modelBuilder)
        {
            modelBuilder.ToTable("userExternalSystemTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userExternalSystemId");

            modelBuilder.Property(e => e.UserId);

            modelBuilder.Property(e => e.ExternalSystemId);

            modelBuilder.HasOne(d => d.ExternalSystem)
                .WithMany()
                .HasForeignKey(d => d.ExternalSystemId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}