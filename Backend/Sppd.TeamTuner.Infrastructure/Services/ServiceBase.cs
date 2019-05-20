using System;
using System.Collections.Generic;
using System.Linq;
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

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<string> propertiesToInclude = null)
        {
            return Repository.GetAllAsync(propertiesToInclude);
        }

        public virtual Task<TEntity> GetByIdAsync(Guid entityId, IEnumerable<string> propertiesToInclude = null)
        {
            return Repository.GetAsync(entityId, propertiesToInclude);
        }

        public virtual async Task CreateAsync(TEntity entity, bool commitChanges = true)
        {
            Repository.Add(entity);
            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities, bool commitChanges = true)
        {
            Repository.Add(entities);
            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IEnumerable<string> propertyNames, bool commitChanges = true)
        {
            var storedEntity = await Repository.GetAsync(entity.Id);
            storedEntity.MapProperties(entity, propertyNames);
            Repository.Update(storedEntity);
            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }

            return storedEntity;
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames = null, bool commitChanges = true)
        {
            var propertyNamesList = propertyNames?.ToList();

            var result = new List<TEntity>();

            foreach (var entity in entities)
            {
                var storedEntity = await Repository.GetAsync(entity.Id);
                storedEntity.MapProperties(entity, propertyNamesList);
                result.Add(storedEntity);
                Repository.Update(storedEntity);
            }

            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }

            return result;
        }

        public virtual async Task DeleteAsync(Guid entityId, bool commitChanges = true)
        {
            await Repository.DeleteAsync(entityId);
            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }
        }

        public async Task DeleteAsync(IEnumerable<Guid> entityIds, bool commitChanges = true)
        {
            await Repository.DeleteAsync(entityIds);
            if (commitChanges)
            {
                await UnitOfWork.CommitAsync();
            }
        }
    }
}