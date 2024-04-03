namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The tenant url map.
    /// </summary>
    public class TenantUrlMap : BaseEntityMap<TenantUrl>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model Builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<TenantUrl> modelBuilder)
        {
            modelBuilder.ToTable("tenantUrlTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("tenantUrlId");
            modelBuilder.Property(e => e.TenantId).HasColumnName("tenantId");
            modelBuilder.Property(e => e.UrlHostName).HasColumnName("urlHostName");
            modelBuilder.Property(e => e.UseHostForAuth).HasColumnName("useHostForAuth");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.HasOne(k => k.Tenant).WithMany(m => m.TenantUrl).HasForeignKey(f => f.TenantId)
                .HasConstraintName("FK_tenantUrlTBL_tenantTBL");
        }
    }
}
