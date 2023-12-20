// <copyright file="UserPasswordValidationTokenMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user password validation token map.
    /// </summary>
    public class UserPasswordValidationTokenMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<UserPasswordValidationToken>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<UserPasswordValidationToken> modelBuilder)
        {
            modelBuilder.ToTable("UserPasswordValidationTokenTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userPasswordValidationTokenId");

            modelBuilder.Property(e => e.HashedToken)
                .HasColumnName("hashedToken")
                .HasMaxLength(128);

            modelBuilder.Property(e => e.Salt)
                .HasColumnName("salt")
                .HasMaxLength(128);

            modelBuilder.Property(e => e.Lookup)
                .HasColumnName("lookup")
                .HasMaxLength(128);

            modelBuilder.Property(e => e.Expiry).HasColumnName("expiry");
            modelBuilder.Property(e => e.TenantId).HasColumnName("tenantId");
            modelBuilder.Property(e => e.UserId).HasColumnName("userId");
            modelBuilder.Property(e => e.CreatedUserId).HasColumnName("createdUserId");
            modelBuilder.Property(e => e.CreatedDate).HasColumnName("createdDate");

            modelBuilder.HasOne(d => d.User)
                .WithMany(p => p.UserPasswordValidationToken)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userPasswordValidationTokenTBL_userTBL");
        }
    }
}
