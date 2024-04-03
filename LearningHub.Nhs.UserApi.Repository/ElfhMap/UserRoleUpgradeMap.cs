namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user user group map.
    /// </summary>
    public class UserRoleUpgradeMap : BaseEntityMap<UserRoleUpgrade>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void InternalMap(EntityTypeBuilder<UserRoleUpgrade> modelBuilder)
        {
            modelBuilder.ToTable("UserRoleUpgradeTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("userRoleUpgradeId");

            modelBuilder.Property(e => e.UserId).HasColumnName("userId");

            modelBuilder.Property(e => e.EmailAddress).HasColumnName("emailAddress");
            modelBuilder.Property(e => e.CreateUserId).HasColumnName("createUserId");
            modelBuilder.Property(e => e.CreateDate).HasColumnName("createDate");
            modelBuilder.Property(e => e.UpgradeDate).HasColumnName("upgradeDate");
            modelBuilder.Property(e => e.UserHistoryTypeId).HasColumnName("userHistoryTypeId");

            modelBuilder.HasOne(d => d.User)
           .WithMany(p => p.UserRoleUpgrade)
           .HasForeignKey(d => d.UserId)
           .OnDelete(DeleteBehavior.ClientSetNull)
           .HasConstraintName("FK_UserRoleUpgrade_user");

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
        }
    }
}