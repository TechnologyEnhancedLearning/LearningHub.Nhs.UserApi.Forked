namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The email template map.
    /// </summary>
    public class EmailTemplateMap : BaseEntityMap<EmailTemplate>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<EmailTemplate> modelBuilder)
        {
            modelBuilder.ToTable("emailTemplateTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("emailTemplateId");
            modelBuilder.Property(e => e.EmailTemplateTypeId).HasColumnName("emailTemplateTypeId");
            modelBuilder.Property(e => e.ProgrammeComponentId).HasColumnName("programmeComponentId");
            modelBuilder.Property(e => e.Title).HasColumnName("title");
            modelBuilder.Property(e => e.Subject).HasColumnName("subject");
            modelBuilder.Property(e => e.Body).HasColumnName("body");
            modelBuilder.Property(e => e.TenantId).HasColumnName("tenantId");

            modelBuilder.HasOne(d => d.EmailTemplateType)
                        .WithMany(p => p.EmailTemplate)
                        .HasForeignKey(d => d.EmailTemplateTypeId)
                        .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
