namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The tenant map.
    /// </summary>
    public class TenantMap : BaseEntityMap<Tenant>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<Tenant> modelBuilder)
        {
            modelBuilder.ToTable("tenantTBL", "dbo");

            modelBuilder.Property(e => e.Id).HasColumnName("tenantId");
            modelBuilder.Property(e => e.Code).HasColumnName("tenantCode");
            modelBuilder.Property(e => e.Name).HasColumnName("tenantName");
            modelBuilder.Property(e => e.Description).HasColumnName("tenantDescription");
            modelBuilder.Property(e => e.ShowFullCatalogInfoMessageInd).HasColumnName("showFullCatalogInfoMessageInd");
            modelBuilder.Property(e => e.CatalogUrl).HasColumnName("catalogUrl");
            modelBuilder.Property(e => e.QuickStartGuideUrl).HasColumnName("quickStartGuideUrl");
            modelBuilder.Property(e => e.SupportFormUrl).HasColumnName("supportFormUrl");
            modelBuilder.Property(e => e.LiveChatStatus).HasColumnName("liveChatStatus");
            modelBuilder.Property(e => e.LiveChatSnippet).HasColumnName("liveChatSnippet");
            modelBuilder.Property(e => e.MyElearningDefaultView).HasColumnName("myElearningDefaultView");
            modelBuilder.Property(e => e.PreLoginCatalogueDefaultView).HasColumnName("preLoginCatalogueDefaultView");
            modelBuilder.Property(e => e.PostLoginCatalogueDefaultView).HasColumnName("postLoginCatalogueDefaultView");
            modelBuilder.Property(e => e.AuthSignInUrlRelative).HasColumnName("authSignInUrlRelative");
            modelBuilder.Property(e => e.AuthSignOutUrlRelative).HasColumnName("authSignOutUrlRelative");
            modelBuilder.Property(e => e.AuthSecret).HasColumnName("authSecret");
        }
    }
}