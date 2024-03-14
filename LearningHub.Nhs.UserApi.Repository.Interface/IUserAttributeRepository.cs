namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Common;

    /// <summary>
    /// The UserAttributeRepository interface.
    /// </summary>
    public interface IUserAttributeRepository
    {
        /// <summary>
        /// The link open athens account to elfh user.
        /// </summary>
        /// <param name="linkDetails">
        /// The link details.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> LinkOpenAthensAccountToElfhUser(OpenAthensToElfhUserLinkDetails linkDetails);
    }
}
