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
        /// <param name="propertiesToInclude">
        ///     The properties to include. If none have been specified, no navigation properties will
        ///     be loaded.
        /// </param>
        /// <returns>
        ///     All entities.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<string> propertiesToInclude = null);

        /// <summary>
        ///     Gets the entity with the specified <see cref="entityId" />
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="propertiesToInclude">
        ///     The properties to include. If none have been specified, no navigation properties will
        ///     be loaded.
        /// </param>
        /// <returns>
        ///     The entity if it could be found; otherwise null
        /// </returns>
        Task<TEntity> GetByIdAsync(Guid entityId, IEnumerable<string> propertiesToInclude = null);

        /// <summary>
        ///     Creates an entity and commits changes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        /// <returns>The created entity</returns>
        Task CreateAsync(TEntity entity, bool commitChanges = true);

        /// <summary>
        ///     Creates all entities and commits changes.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        Task CreateAsync(IEnumerable<TEntity> entities, bool commitChanges = true);

        /// <summary>
        ///     Updates the entity and commits changes.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="propertyNames">
        ///     The properties to update. If null is being specified all properties which are not part
        ///     of <see cref="BaseEntity" /> will be updated.
        /// </param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, IEnumerable<string> propertyNames = null, bool commitChanges = true);

        /// <summary>
        ///     Updates the entities and commits changes.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="propertyNames">
        ///     The properties to update. If null is being specified all properties which are not part
        ///     of <see cref="BaseEntity" /> will be updated.
        /// </param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        /// <returns>The updated entities</returns>
        Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames = null, bool commitChanges = true);

        /// <summary>
        ///     Deletes the entity and commits changes.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        Task DeleteAsync(Guid entityId, bool commitChanges = true);

        /// <summary>
        ///     Deletes the entities and commits changes.
        /// </summary>
        /// <param name="entityIds">The entity identifiers.</param>
        /// <param name="commitChanges">If true, the changes will be committed</param>
        Task DeleteAsync(IEnumerable<Guid> entityIds, bool commitChanges = true);
    }
}