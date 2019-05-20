using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        /// <summary>
        ///     Gets the entity set.
        /// </summary>
        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        /// <summary>
        ///     Gets the DB context.
        /// </summary>
        protected TeamTunerContext Context { get; }

        public Repository(TeamTunerContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetAsync(Guid entityId, IEnumerable<string> includeProperties = null)
        {
            TEntity entity;
            try
            {
                entity = await GetQueryableWithIncludes(includeProperties).SingleAsync(e => e.Id == entityId);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(TEntity), entityId.ToString());
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<string> includeProperties = null)
        {
            return await GetQueryableWithIncludes(includeProperties).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public async Task DeleteAsync(Guid entityId)
        {
            var entityToDelete = await GetAsync(entityId);
            entityToDelete.IsDeleted = true;
        }

        public async Task DeleteAsync(IEnumerable<Guid> entityIds)
        {
            foreach (var entityId in entityIds)
            {
                await DeleteAsync(entityId);
            }
        }

        protected IQueryable<TEntity> GetQueryableWithIncludes(IEnumerable<string> includeProperties = null)
        {
            IQueryable<TEntity> queryable = Set;

            if (includeProperties == null)
            {
                return queryable;
            }

            foreach (var propertyName in includeProperties)
            {
                queryable = queryable.Include(propertyName);
            }

            return queryable;
        }
    }
}