// <copyright file="IGdcRegisterRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface
{
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;

    /// <summary>
    /// The GdcRegisterRepository interface.
    /// </summary>
    public interface IGdcRegisterRepository
    {
        /// <summary>
        /// The get by last name and gdc number.
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
        Task<GdcRegister> GetByLastNameAndGDCNumber(string lastname, string medicalCouncilNumber);
    }
}
