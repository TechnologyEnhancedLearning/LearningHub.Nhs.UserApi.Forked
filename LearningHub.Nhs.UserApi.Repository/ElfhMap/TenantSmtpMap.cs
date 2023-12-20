// <copyright file="TenantSmtpMap.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The tenant smtp map.
    /// </summary>
    public class TenantSmtpMap : BaseEntityMap<TenantSmtp>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<TenantSmtp> modelBuilder)
        {
            modelBuilder.ToTable("tenantSmtpTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("tenantId");
            modelBuilder.Property(e => e.DeliveryMethod).HasColumnName("deliveryMethod");
            modelBuilder.Property(e => e.PickupDirectoryLocation).HasColumnName("pickupDirectoryLocation");
            modelBuilder.Property(e => e.From).HasColumnName("from");
            modelBuilder.Property(e => e.UserName).HasColumnName("userName");
            modelBuilder.Property(e => e.Password).HasColumnName("password");
            modelBuilder.Property(e => e.EnableSsl).HasColumnName("enableSsl");
            modelBuilder.Property(e => e.Host).HasColumnName("host");
            modelBuilder.Property(e => e.Port).HasColumnName("port");
            modelBuilder.Property(e => e.Active).HasColumnName("active");
        }
    }
}
