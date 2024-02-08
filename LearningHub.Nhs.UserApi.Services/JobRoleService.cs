namespace LearningHub.Nhs.UserApi.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using elfhHub.Nhs.Models.Common;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The job role service.
    /// </summary>
    public class JobRoleService : IJobRoleService
    {
        private readonly IJobRoleRepository jobRoleRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobRoleService"/> class.
        /// </summary>
        /// <param name="jobRoleRepository">
        /// The job role repository.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public JobRoleService(
            IJobRoleRepository jobRoleRepository,
            IMapper mapper)
        {
            this.jobRoleRepository = jobRoleRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<JobRoleBasicViewModel> GetByIdAsync(int id)
        {
            var jobRole = await this.jobRoleRepository.GetByIdAsync(id);

            return this.mapper.Map<JobRoleBasicViewModel>(jobRole);
        }

        /// <inheritdoc/>
        public List<JobRole> GetAll()
        {
            var jobRoleList = this.jobRoleRepository.GetAll()
                .OrderBy(r => r.DisplayOrder)
                .ToListWithNoLock();

            return jobRoleList;
        }

        /// <inheritdoc/>
        public async Task<List<JobRoleBasicViewModel>> GetFilteredWithStaffGroupAsync(string filter)
        {
            var items = this.jobRoleRepository.GetAllWithStaffGroup()
                .Where(jr => jr.StaffGroup != null && !string.IsNullOrEmpty(jr.StaffGroup.Name))
                .Where(jr => (jr.Name + " (" + jr.StaffGroup.Name + ")").Contains(filter))
                .OrderBy(jr => jr.Name).ThenBy(jr => jr.StaffGroup.Name);

            var jobRoleList = await this.mapper.ProjectTo<JobRoleBasicViewModel>(items).ToListWithNoLockAsync();

            return jobRoleList;
        }

        /// <inheritdoc/>
        public async Task<List<JobRoleBasicViewModel>> GetByStaffGroupIdAsync(int staffGroupId)
        {
            var items = this.jobRoleRepository.GetAll()
                .Where(jr => jr.StaffGroupId == staffGroupId)
                .OrderBy(jr => jr.Name).ThenBy(jr => jr.StaffGroup.Name);

            var jobRoleList = await this.mapper.ProjectTo<JobRoleBasicViewModel>(items).ToListWithNoLockAsync();

            return jobRoleList;
        }
    }
}