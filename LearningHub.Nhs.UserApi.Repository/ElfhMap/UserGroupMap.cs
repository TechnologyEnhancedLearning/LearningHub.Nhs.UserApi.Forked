namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user group map.
    /// </summary>
    public class UserGroupMap : BaseEntityMap<UserGroup>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void InternalMap(EntityTypeBuilder<UserGroup> modelBuilder)
        {
            modelBuilder.ToTable("UserGroupTBL", "dbo");

            modelBuilder.Property(e => e.Id)
                .HasColumnName("userGroupId")
                .ValueGeneratedNever();

            modelBuilder.Property(e => e.Code)
                .HasColumnName("UserGroupCode")
                .HasMaxLength(50)
                .IsUnicode(false);

            modelBuilder.Property(e => e.Description)
                .HasColumnName("UserGroupDescription")
                .HasMaxLength(255)
                .IsUnicode(false);

            modelBuilder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("UserGroupName")
                .HasMaxLength(100)
                .IsUnicode(false);

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
        }
    }
}