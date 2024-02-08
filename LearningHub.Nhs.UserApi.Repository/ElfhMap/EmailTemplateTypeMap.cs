namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The email template type map.
    /// </summary>
    public class EmailTemplateTypeMap : BaseEntityMap<EmailTemplateType>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<EmailTemplateType> modelBuilder)
        {
            modelBuilder.ToTable("emailTemplateTypeTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("emailTemplateTypeId");
            modelBuilder.Property(e => e.Name).HasColumnName("emailTemplateTypeName");
            modelBuilder.Property(e => e.AvailableTags).HasColumnName("availableTags");
        }
    }
}
