namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;

    /// <summary>
    /// The IpCountryLookupRepository interface.
    /// </summary>
    public interface IIpCountryLookupRepository
    {
        /// <summary>
        /// Checks whether IP addess is from UK.
        /// </summary>
        /// <param name="ipAddress">Ip address.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<bool> IsUKIpAddress(long ipAddress);
    }
}
