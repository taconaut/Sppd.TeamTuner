using System.Threading.Tasks;

namespace Sppd.TeamTuner.Core
{
    /// <summary>
    ///     Defines an unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Commits the changes to DB.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        ///     Resets all changes in current context.
        /// </summary>
        void Rollback();
    }
}