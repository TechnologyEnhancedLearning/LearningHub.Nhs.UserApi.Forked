namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The user security question map.
    /// </summary>
    public class UserSecurityQuestionMap : BaseEntityMap<UserSecurityQuestion>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<UserSecurityQuestion> modelBuilder)
        {
            modelBuilder.ToTable("userSecurityQuestionTBL", "dbo");
            modelBuilder.Property(e => e.Id).HasColumnName("userSecurityQuestionId");
            modelBuilder.Property(e => e.UserId).HasColumnName("userId");
            modelBuilder.Property(e => e.SecurityQuestionId).HasColumnName("securityQuestionId");
            modelBuilder.Property(e => e.SecurityQuestionAnswerHash).HasColumnName("securityQuestionAnswerHash");
            modelBuilder.Property(e => e.Deleted).HasColumnName("deleted");
            modelBuilder.Property(e => e.AmendUserId).HasColumnName("amendUserId");
            modelBuilder.Property(e => e.AmendDate).HasColumnName("amendDate");
        }
    }
}
