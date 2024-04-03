namespace LearningHub.Nhs.UserApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LearningHub.Nhs.Models.Common;
    using LearningHub.Nhs.Models.Entities.External;
    using LearningHub.Nhs.Models.Validation;
    using LearningHub.Nhs.UserApi.Repository;
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    /// <summary>
    /// The external system service.
    /// </summary>
    public class ExternalSystemService : IExternalSystemService
    {
        private readonly Repository.Interface.LH.IExternalSystemRepository lhExternalSystemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalSystemService"/> class.
        /// </summary>
        /// <param name="extSystemRepository">The external system Repository.</param>
        public ExternalSystemService(
            Repository.Interface.LH.IExternalSystemRepository extSystemRepository)
        {
            this.lhExternalSystemRepository = extSystemRepository;
        }

        /// <inheritdoc/>
        public async Task<ExternalSystem> GetByCodeAsync(string clientCode)
        {
            var extsystem = await this.lhExternalSystemRepository.GetByCode(clientCode);

            return extsystem;
        }

        /// <inheritdoc/>
        public async Task<ExternalSystem> GetExtSystemById(int id)
        {
            var extsystem = await this.lhExternalSystemRepository.GetExtSystemById(id);

            return extsystem;
        }

        /// <inheritdoc/>
        public async Task<PagedResultSet<ExternalSystem>> GetExternalSystems(int page, int pageSize, string sortColumn = "", string sortDirection = "", string filter = "")
        {
            try
            {
                var filterCriteria = JsonConvert.DeserializeObject<List<PagingColumnFilter>>(filter);

                PagedResultSet<ExternalSystem> result = new PagedResultSet<ExternalSystem>();

                var items = this.lhExternalSystemRepository.GetExternalSystems();

                if (filterCriteria != null)
                {
                    items = this.FilterItems(items, filterCriteria);
                }

                result.TotalItemCount = await items.CountWithNoLockAsync();

                items = this.OrderItems(items, sortColumn, sortDirection);

                items = items.Skip((page - 1) * pageSize).Take(pageSize);

                result.Items = await items.ToListWithNoLockAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> UpdateAsync(int userId, ExternalSystem externalSystem)
        {
            try
            {
                var retVal = await this.ValidateAsync(externalSystem);

                if (retVal.IsValid)
                {
                    await this.lhExternalSystemRepository.UpdateAsync(userId, externalSystem);
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<LearningHubValidationResult> CreateAsync(int userId, ExternalSystem externalSystem)
        {
            var n = externalSystem;

            var retVal = await this.ValidateAsync(n);

            if (retVal.IsValid)
            {
                retVal.CreatedId = await this.lhExternalSystemRepository.CreateAsync(userId, n);
            }

            return retVal;
        }

        private IQueryable<ExternalSystem> OrderItems(IQueryable<ExternalSystem> items, string sortColumn, string sortDirection)
        {
            var descending = sortDirection == "D";

            switch (sortColumn)
            {
                case "CreatedBy":
                    items = descending ? items.OrderByDescending(l => l.CreateUser.UserName) : items.OrderBy(l => l.CreateUser.UserName);
                    break;
                default:
                    items = descending ? items.OrderByDescending(l => l.Id) : items.OrderBy(l => l.Id);
                    break;
            }

            return items;
        }

        private IQueryable<ExternalSystem> FilterItems(IQueryable<ExternalSystem> items, List<PagingColumnFilter> filterCriteria)
        {
            if (filterCriteria.Count == 0)
            {
                return items;
            }

            foreach (var filter in filterCriteria)
            {
                switch (filter.Column)
                {
                    case "Name":
                        items = items.Where(l => l.Name.Contains(filter.Value));
                        break;
                    case "Code":
                        items = items.Where(l => l.Code.Contains(filter.Value));
                        break;
                    case "CreateDate":

                        DateTime val = Convert.ToDateTime(filter.Value).ToUniversalTime();

                        items = items.Where(l => l.CreateDate.ToString("dd/MM/yyyy") == Convert.ToDateTime(filter.Value).ToString("dd/MM/yyyy"));
                        break;
                    default:
                        break;
                }
            }

            return items;
        }

        private async Task<LearningHubValidationResult> ValidateAsync(ExternalSystem externalSystem)
        {
            var externalSystemValidator = new ExternalSystemValidator();
            var clientValidationResult = await externalSystemValidator.ValidateAsync(externalSystem);

            var retVal = new LearningHubValidationResult(clientValidationResult);

            return retVal;
        }
    }
}
