using System.Threading.Tasks;

namespace Sppd.TeamTuner.Core.Services
{
    public interface ICardImportService
    {
        /// <summary>
        ///     Imports the cards asynchronously.
        /// </summary>
        Task DoImportAsync();
    }
}