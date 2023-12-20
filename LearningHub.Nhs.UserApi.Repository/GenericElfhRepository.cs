// <copyright file="GenericElfhRepository.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using elfhHub.Nhs.Models.Entities;
    using LearningHub.Nhs.UserApi.Repository.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The generic repository.
    /// </summary>
    /// <typeparam name="TEntity">Input type.</typeparam>
    public class GenericElfhRepository<TEntity> : IGenericElfhRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericElfhRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The db context.</param>
        /// <param name="logger">The logger.</param>
        public GenericElfhRepository(ElfhHubDbContext dbContext, ILogger<TEntity> logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the db context.
        /// </summary>
        protected ElfhHubDbContext DbContext { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger<TEntity> Logger { get; }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll()
        {
            return this.DbContext.Set<TEntity>().AsNoTracking();
        }

        /// <inheritdoc/>
        public virtual async Task<int> CreateAsync(int userId, TEntity entity)
        {
            await this.DbContext.Set<TEntity>().AddAsync(entity);
            this.SetAuditFieldsForCreate(userId, entity);

            try
            {
                await this.DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ue)
            {
                this.Logger.LogError(
                    "Error creating record to database for entity {entityname}. UserId: {lhuserid}. Error message: {err}",
                    nameof(TEntity),
                    userId,
                    ue.Message);
                throw;
            }

            this.DbContext.Entry(entity).State = EntityState.Detached;

            return entity.Id;
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(int userId, TEntity entity)
        {
            this.DbContext.Set<TEntity>().Update(entity);

            this.SetAuditFieldsForUpdate(userId, entity);

            await this.DbContext.SaveChangesAsync();

            this.DbContext.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// The set audit fields for create.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="entity">The entity.</param>
        public void SetAuditFieldsForCreate(int userId, EntityBase entity)
        {
            var amendDate = DateTimeOffset.Now;
            entity.Deleted = false;
            ////entity.CreatedDate = amendDate;
            entity.AmendUserId = userId;
            entity.AmendDate = amendDate;
        }

        /// <summary>
        /// The set audit fields for update.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="entity">The entity.</param>
        public void SetAuditFieldsForUpdate(int userId, EntityBase entity)
        {
            entity.AmendUserId = userId;
            entity.AmendDate = DateTimeOffset.Now;

            if (this.DbContext.Entry(entity).Metadata.FindProperty("CreatedDate") != null)
            {
                this.DbContext.Entry(entity).Property("CreatedDate").IsModified = false;
            }
        }

        /// <summary>
        /// The set audit fields for delete.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void SetAuditFieldsForDelete(int userId, EntityBase entity)
        {
            entity.Deleted = true;
            entity.AmendUserId = userId;
            entity.AmendDate = DateTimeOffset.Now;
            if (this.DbContext.Entry(entity).Metadata.FindProperty("CreatedDate") != null)
            {
                this.DbContext.Entry(entity).Property("CreatedDate").IsModified = false;
            }
        }
    }
}