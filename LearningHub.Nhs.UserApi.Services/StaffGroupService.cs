// <copyright file="StaffGroupService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The staff group service.
    /// </summary>
    public class StaffGroupService : IStaffGroupService
    {
        /// <summary>
        /// The staff group repository.
        /// </summary>
        private readonly IStaffGroupRepository staffGroupRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<StaffGroupService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffGroupService"/> class.
        /// </summary>
        /// <param name="staffGroupRepository">
        /// The staff group repository.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public StaffGroupService(IStaffGroupRepository staffGroupRepository, ILogger<StaffGroupService> logger)
        {
            this.staffGroupRepository = staffGroupRepository;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<StaffGroup> GetByIdAsync(int id)
        {
            var staffGroup = await this.staffGroupRepository.GetByIdAsync(id);

            return staffGroup;
        }

        /// <inheritdoc/>
        public List<StaffGroup> GetAll()
        {
            var staffGroupList = this.staffGroupRepository.GetAll()
                .OrderBy(r => r.DisplayOrder)
                .ToListWithNoLock();

            return staffGroupList;
        }
    }
}
