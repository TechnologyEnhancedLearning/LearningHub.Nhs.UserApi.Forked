namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The login wizard stage activity map.
    /// </summary>
    public class LoginWizardStageActivityMap : IEntityTypeMap
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        public void Map(ModelBuilder builder)
        {
            this.InternalMap(builder.Entity<LoginWizardStageActivity>());
        }

        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected void InternalMap(EntityTypeBuilder<LoginWizardStageActivity> modelBuilder)
        {
            modelBuilder.ToTable("loginWizardStageActivityTBL", "dbo").HasKey(e => e.Id);
            modelBuilder.Property(e => e.Id).HasColumnName("loginWizardStageActivityId");
            modelBuilder.Property(e => e.LoginWizardStageId).HasColumnName("loginWizardStageId");
            modelBuilder.Property(e => e.UserId).HasColumnName("userId");
            modelBuilder.Property(e => e.ActivityDatetime).HasColumnName("activityDatetime");
        }
    }
}
