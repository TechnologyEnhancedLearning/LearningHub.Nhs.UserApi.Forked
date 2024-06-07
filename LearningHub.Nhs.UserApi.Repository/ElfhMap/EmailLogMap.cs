namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The email log map.
    /// </summary>
    public class EmailLogMap : BaseEntityMap<EmailLog>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<EmailLog> modelBuilder)
        {
            modelBuilder.ToTable("emailLogTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("emailLogId");
            modelBuilder.Property(e => e.EmailTemplateId).HasColumnName("emailTemplateId");
            modelBuilder.Property(e => e.ToUserId).HasColumnName("toUserId");
            modelBuilder.Property(e => e.CreatedDate).HasColumnName("createdDate");
            modelBuilder.Property(e => e.FromEmailAddress).HasColumnName("fromEmailAddress");
            modelBuilder.Property(e => e.ToEmailAddress).HasColumnName("toEmailAddress");
            modelBuilder.Property(e => e.Subject).HasColumnName("subject");
            modelBuilder.Property(e => e.Body).HasColumnName("body");
            modelBuilder.Property(e => e.Retries).HasColumnName("retries");
            modelBuilder.Property(e => e.SentDate).HasColumnName("sentDate");
            modelBuilder.Property(e => e.AttachmentFile).HasColumnName("attachmentFile");
            modelBuilder.Property(e => e.ProgrammeComponentId).HasColumnName("programmeComponentId");
            modelBuilder.Property(e => e.TenantId).HasColumnName("tenantId");
            modelBuilder.Property(e => e.ProcessDate).HasColumnName("processDate");
            modelBuilder.Property(e => e.Priority).HasColumnName("priority");

            modelBuilder.HasOne(d => d.EmailTemplate)
                        .WithMany(p => p.EmailLog)
                        .HasForeignKey(d => d.EmailTemplateId)
                        .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.HasOne(d => d.ToUser)
                        .WithMany(p => p.EmailLog)
                        .HasForeignKey(d => d.ToUserId)
                        .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
