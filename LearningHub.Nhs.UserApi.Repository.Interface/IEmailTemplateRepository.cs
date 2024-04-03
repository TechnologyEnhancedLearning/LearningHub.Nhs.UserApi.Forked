namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The EmailTemplateRepository interface.
    /// </summary>
    public interface IEmailTemplateRepository : IGenericElfhRepository<EmailTemplate>
    {
        /// <summary>
        /// The get by type and tenant async.
        /// </summary>
        /// <param name="emailTemplateTypeId">
        /// The email template type id.
        /// </param>
        /// <param name="tenantId">
        /// The tenant id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<EmailTemplate> GetByTypeAndTenantAsync(int emailTemplateTypeId, int tenantId);
    }
}
