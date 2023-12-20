// <copyright file="IGmcLrmpRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The GmcLrmpRepository interface.
    /// </summary>
    public interface IGmcLrmpRepository
    {
        /// <summary>
        /// The get by last name and gmc number.
        /// </summary>
        /// <param name="lastname">
        /// The lastname.
        /// </param>
        /// <param name="medicalCouncilNumber">
        /// The medical council number.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<GmcLrmp> GetByLastNameAndGMCNumber(string lastname, string medicalCouncilNumber);
    }
}
