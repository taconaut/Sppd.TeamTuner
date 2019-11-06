using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     A repository to handle <see cref="TeamTunerUserRegistrationRequest" />.
    /// </summary>
    /// <seealso cref="IRepository{TeamTunerUserRegistrationRequest}" />
    public interface IRegistrationRequestRepository : IRepository<TeamTunerUserRegistrationRequest>
    {
        /// <summary>
        ///     Gets the <see cref="TeamTunerUserRegistrationRequest" /> by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The <see cref="TeamTunerUserRegistrationRequest" />.</returns>
        Task<TeamTunerUserRegistrationRequest> GetByEmailAsync(string email);
    }
}