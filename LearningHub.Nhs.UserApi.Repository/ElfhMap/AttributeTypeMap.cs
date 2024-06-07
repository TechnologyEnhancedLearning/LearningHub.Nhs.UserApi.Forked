namespace LearningHub.Nhs.UserApi.Repository.ElfhMap
{
    using elfhHub.Nhs.Models.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The attribute type map.
    /// </summary>
    public class AttributeTypeMap : BaseEntityMap<AttributeType>
    {
        /// <summary>
        /// The internal map.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        protected override void InternalMap(EntityTypeBuilder<AttributeType> builder)
        {
            builder.ToTable("attributeTypeTBL", "dbo");

            builder.Property(p => p.Id)
                .HasColumnName("attributeTypeId")
                .ValueGeneratedNever();

            builder.Property(p => p.TypeName)
                .HasColumnName("attributeTypeName")
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
