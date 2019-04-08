using System;

using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.Resolvers
{
    internal class RepositoryResolver : IRepositoryResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TRepository Resolve<TRepository, TEntity>()
            where TRepository : IRepository<TEntity>
            where TEntity : BaseEntity
        {
            return _serviceProvider.GetService<TRepository>();
        }

        public IRepository<TEntity> ResolveFor<TEntity>()
            where TEntity : BaseEntity
        {
            return _serviceProvider.GetService<IRepository<TEntity>>();
        }
    }
}