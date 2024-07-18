namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The login wizard stage map.
    /// </summary>
    public class LoginWizardStageMap : BaseEntityMap<LoginWizardStage>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<LoginWizardStage> modelBuilder)
        {
            modelBuilder.ToTable("loginWizardStageTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("loginWizardStageId");

            modelBuilder.Property(e => e.Description).HasColumnName("description").HasMaxLength(128);

            modelBuilder.Property(e => e.ReasonDisplayText).HasColumnName("reasonDisplayText").HasMaxLength(1024);

            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");

            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");

            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
