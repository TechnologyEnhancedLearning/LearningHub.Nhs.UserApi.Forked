namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The system setting map.
    /// </summary>
    public class SystemSettingMap : BaseEntityMap<SystemSetting>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<SystemSetting> modelBuilder)
        {
            modelBuilder.ToTable("systemSettingTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("systemSettingId");
            modelBuilder.Property(e => e.Name).HasColumnName("systemSettingName");
            modelBuilder.Property(e => e.IntValue).HasColumnName("intValue");
            modelBuilder.Property(e => e.TextValue).HasColumnName("textValue");
            modelBuilder.Property(e => e.BooleanValue).HasColumnName("booleanValue");
            modelBuilder.Property(e => e.DateValue).HasColumnName("dateValue");
        }
    }
}
