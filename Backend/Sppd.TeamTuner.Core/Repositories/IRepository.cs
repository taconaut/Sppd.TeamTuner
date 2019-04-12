using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     Generic repository offering basic CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        /// <summary>
        ///     Gets the entity having the specified id.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="includeProperties">
        ///     Specifies the navigation properties to include; if NULL, no navigation properties will
        ///     be included.
        /// </param>
        /// <returns>The entity if it could be found; otherwise a <see cref="EntityNotFoundException" /> will be thrown.</returns>
        Task<TEntity> GetAsync(Guid entityId, IEnumerable<string> includeProperties = null);

        /// <summary>
        ///     Gets all entities.
        /// </summary>
        /// <param name="includeProperties">
        ///     Specifies the navigation properties to include; if NULL, no navigation properties will
        ///     be included.
        /// </param>
        /// <returns>A list containing all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<string> includeProperties = null);

        /// <summary>
        ///     Adds the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        ///     Updates the entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        ///     Deletes the entity.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        Task DeleteAsync(Guid entityId);
    }
}