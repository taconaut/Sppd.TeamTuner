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

        /// <summary>
        ///     Override in derived types to specify global includes.
        /// </summary>
        public virtual Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes { get; } = null;

        public Repository(TeamTunerContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetAsync(Guid entityId)
        {
            TEntity entity;
            try
            {
                entity = await GetQueryWithIncludes().SingleAsync(e => e.Id == entityId);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(TEntity), entityId.ToString());
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetQueryWithIncludes().ToListAsync();
        }

        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Set.Update(entity);
        }

        public async Task DeleteAsync(Guid entityId)
        {
            var entityToDelete = await GetAsync(entityId);
            entityToDelete.IsDeleted = true;
            Set.Update(entityToDelete);
        }

        protected IQueryable<TEntity> GetQueryWithIncludes()
        {
            return Includes == null
                ? Set
                : Includes(Set);
        }
    }
}