using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;
using Sppd.TeamTuner.Core.Services;

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

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IEnumerable<string> propertiesToUpdate)
        {
            var storedEntity = await Repository.GetAsync(entity.Id);
            MapProperties(storedEntity, entity, propertiesToUpdate);
            Repository.Update(storedEntity);
            await UnitOfWork.CommitAsync();
            return storedEntity;
        }

        /// <summary>
        ///     Maps the properties from <see cref="entitySource" /> to <see cref="entityDest" />. If
        ///     <see cref="propertyNamesToUpdate" /> has been specified, only those properties will be updated; if it hasn't been
        ///     specified, all public properties, except the ones from <see cref="BaseEntity" /> will be mapped.
        /// </summary>
        /// <param name="entityDest">The destination entity.</param>
        /// <param name="entitySource">The source entity.</param>
        /// <param name="propertyNamesToUpdate">The property names to update.</param>
        /// <exception cref="BusinessException">
        ///     Thrown if a property specified in <see cref="propertyNamesToUpdate" /> could not be
        ///     found for <see cref="TEntity" />
        /// </exception>
        private static void MapProperties(TEntity entityDest, TEntity entitySource, IEnumerable<string> propertyNamesToUpdate)
        {
            var baseEntityProperties = typeof(BaseEntity).GetProperties();
            var entityProperties = typeof(TEntity).GetProperties();

            var propertiesToUpdate = entityProperties.Where(pt => !baseEntityProperties.Any(pb => AreEqual(pb, pt)));

            if (propertyNamesToUpdate != null)
            {
                var toUpdateNames = propertyNamesToUpdate.ToList();
                var unknownPropertyNames = toUpdateNames.Where(pn => !propertiesToUpdate.Select(p => p.Name).Contains(pn)).ToList();
                if (unknownPropertyNames.Any())
                {
                    throw new BusinessException($"Unknown property names: {string.Join(", ", unknownPropertyNames)}");
                }

                propertiesToUpdate = propertiesToUpdate.Where(p => toUpdateNames.Contains(p.Name));
            }

            foreach (var propertyInfo in propertiesToUpdate)
            {
                var newValue = propertyInfo.GetValue(entitySource);
                propertyInfo.SetValue(entityDest, newValue);
            }
        }

        private static bool AreEqual(MemberInfo p1, MemberInfo p2)
        {
            return p1.MetadataToken == p2.MetadataToken && p1.Module.Equals(p2.Module);
        }
    }
}