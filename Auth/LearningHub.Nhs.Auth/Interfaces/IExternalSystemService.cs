namespace LearningHub.Nhs.Auth.Interfaces
{
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Entities.External;

    /// <summary>
    /// The ExternalSystemService interface.
    /// </summary>
    public interface IExternalSystemService
    {
        /// <summary>
        /// Get external system entity by client code.
        /// </summary>
        /// <param name="clientCode">The client code.</param>
        /// <returns>The <see cref="ExternalSystem"/>.</returns>
        Task<ExternalSystem> GetExternalSystem(string clientCode);

        /// <summary>
        /// Get external system deep link entity by code.
        /// </summary>
        /// <param name="code">The end client code.</param>
        /// <returns>The external system deep link.</returns>
        Task<ExternalSystemDeepLink> GetExternalClientDeepLink(string code);
    }
}