using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     Resolves repositories
    /// </summary>
    public interface IRepositoryResolver
    {
        /// <summary>
        ///     Resolves a repository according to the repository and entity type.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The repository.</returns>
        TRepository Resolve<TRepository, TEntity>()
            where TRepository : IRepository<TEntity>
            where TEntity : BaseEntity;

        /// <summary>
        ///     Resolves a generic repository according to the entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The generic repository.</returns>
        IRepository<TEntity> ResolveFor<TEntity>()
            where TEntity : BaseEntity;
    }
}