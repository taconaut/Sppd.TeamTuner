namespace Sppd.TeamTuner.Core.Domain.Interfaces
{
    /// <summary>
    ///     Used to save entities before a user has been authorized.
    /// </summary>
    public interface IAuthorizing
    {
        /// <summary>
        ///     Gets the authorizing user.
        /// </summary>
        ITeamTunerUser AuthorizingUser { get; }
    }
}