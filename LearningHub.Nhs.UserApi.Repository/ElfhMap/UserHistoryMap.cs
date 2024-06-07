namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user history map.
    /// </summary>
    public class UserHistoryMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Map(ModelBuilder builder)
        {
            InternalMap(builder.Entity<UserHistory>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected static void InternalMap(EntityTypeBuilder<UserHistory> modelBuilder)
        {
            modelBuilder.ToTable("UserHistoryTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("UserHistoryId");

            modelBuilder.Property(e => e.UserHistoryTypeId)
                .HasColumnName("UserHistoryTypeId")
                .IsUnicode(false);

            modelBuilder.Property(e => e.UserId)
                .HasColumnName("UserId")
                .IsUnicode(false);

            modelBuilder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").HasComputedColumnSql("SYSDATETIMEOFFSET()");

            modelBuilder.Property(e => e.TenantId)
                .HasColumnName("TenantId")
                .IsUnicode(false);
        }
    }
}