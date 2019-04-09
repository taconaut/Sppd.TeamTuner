using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Generic service offering CRUD operations for entities extending <see cref="BaseEntity" />.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IServiceBase<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        ///     Gets all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        ///     Gets the entity with the specified <see cref="entityId" />
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>The entity if it could be found; otherwise null</returns>
        Task<TEntity> GetByIdAsync(Guid entityId);

        /// <summary>
        ///     Creates an entity and commits changes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The created entity</returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        ///     Updates the entity and commits changes.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="propertyNames">
        ///     The properties to update. If null is being specified all properties which are not part
        ///     of <see cref="BaseEntity" /> will be updated.
        /// </param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        ///     Deletes the entity and commits changes.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        Task DeleteAsync(Guid entityId);
    }
}