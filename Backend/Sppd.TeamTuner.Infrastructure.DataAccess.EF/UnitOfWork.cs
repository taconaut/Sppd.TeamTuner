using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    /// <summary>
    ///     Implements an unit of work.
    /// </summary>
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly TeamTunerContext _context;

        public UnitOfWork(TeamTunerContext context)
        {
            _context = context;
        }

        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            var changedEntries = _context.ChangeTracker.Entries()
                                         .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}