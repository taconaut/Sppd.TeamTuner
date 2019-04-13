using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Authorization
{
    /// <summary>
    ///     Provides tokens
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        ///     Gets the token which will be set as bearer in the HTTP Authorization header.
        /// </summary>
        /// <param name="user">The user for whom to create the token.</param>
        /// <returns>Token serialized as string.</returns>
        string GetToken(ITeamTunerUser user);
    }
}