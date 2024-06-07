namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Attribute = elfhHub.Nhs.Models.Entities.Attribute;

    /// <summary>
    /// The attribute map.
    /// </summary>
    public class AttributeMap : BaseEntityMap<Attribute>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void InternalMap(EntityTypeBuilder<Attribute> builder)
        {
            builder.ToTable("attributeTBL", "dbo");

            builder.Property(p => p.Id)
                .HasColumnName("attributeId")
                .ValueGeneratedNever();

            builder.Property(p => p.AttributeTypeId).HasColumnName("attributeTypeId");

            builder.Property(p => p.Name)
                .HasColumnName("attributeName")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(p => p.Description)
                .HasColumnName("attributeDescription")
                .HasMaxLength(400)
                .IsUnicode(false);

            builder.HasOne(p => p.AttributeType)
                .WithMany(m => m.Attributes)
                .HasForeignKey(f => f.AttributeTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_attributeTBL_attributeTypeId");
        }
    }
}
