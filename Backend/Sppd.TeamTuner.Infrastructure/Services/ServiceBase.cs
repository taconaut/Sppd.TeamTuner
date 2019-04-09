using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Utils.Extensions;

namespace Sppd.TeamTuner.Infrastructure.Services
{
    /// <summary>Base service implementation offering CRUD operations</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class ServiceBase<TEntity> : IServiceBase<TEntity>
        where TEntity : BaseEntity
    {
        /// <summary>
        ///     Gets the unit of work. Use it to save or rollback changes
        /// </summary>
        protected IUnitOfWork UnitOfWork { get; }

        /// <summary>
        ///     Gets the repository.
        /// </summary>
        protected IRepository<TEntity> Repository { get; }

        protected ServiceBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }

        public virtual Task<TEntity> GetByIdAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            Repository.Add(entity);
            await UnitOfWork.CommitAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(Guid entityId)
        {
            await Repository.DeleteAsync(entityId);
            await UnitOfWork.CommitAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IEnumerable<string> propertyNames)
        {
            var storedEntity = await Repository.GetAsync(entity.Id);
            storedEntity.MapProperties(entity, propertyNames);
            Repository.Update(storedEntity);
            await UnitOfWork.CommitAsync();
            return storedEntity;
        }
    }
}