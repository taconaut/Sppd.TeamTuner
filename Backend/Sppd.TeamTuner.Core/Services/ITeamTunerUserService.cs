using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Services for <see cref="TeamTunerUser" /> handling.
    /// </summary>
    /// <seealso cref="IServiceBase{TeamTunerUser}" />
    public interface ITeamTunerUserService : IServiceBase<TeamTunerUser>
    {
        /// <summary>
        ///     Authenticates user.
        /// </summary>
        /// <param name="username">The user name.</param>
        /// <param name="passwordMd5">The Md5 hash of the password.</param>
        /// <returns>The user if it could be authenticated; otherwise an Exception will be thrown.</returns>
        Task<TeamTunerUser> AuthenticateAsync(string username, string passwordMd5);

        /// <summary>
        ///     Adds a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordMd5">The Md5 hash of the password.</param>
        Task AddAsync(TeamTunerUser user, string passwordMd5);

        /// <summary>
        ///     Creates the user and commits the changes to the DB.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="passwordMd5">The Md5 hash of the password.</param>
        /// <returns>The created user.</returns>
        Task<TeamTunerUser> CreateAsync(TeamTunerUser user, string passwordMd5);

        /// <summary>
        ///     Gets all users being part of the team.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A list of users.</returns>
        Task<IEnumerable<TeamTunerUser>> GetByTeamIdAsync(Guid teamId);

        /// <summary>
        ///     Gets the card levels.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The card levels of the user</returns>
        Task<IEnumerable<CardLevel>> GetCardLevelsAsync(Guid userId);

        /// <summary>
        ///     Sets the card level.
        /// </summary>
        /// <param name="cardLevel">The card level.</param>
        /// <returns>The <see cref="CardLevel" /></returns>
        Task<CardLevel> SetCardLevelAsync(CardLevel cardLevel);

        /// <summary>
        ///     Confirms the email for the given code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns><c>True</c> if the email could be confirmed; otherwise <c>false</c></returns>
        Task<bool> VerifyEmailAsync(string code);

        /// <summary>
        ///     Sends the same mail, previously sent by <see cref="SendEmailVerificationAsync" />.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns><c>True</c> if the mail could be sent; otherwise <c>false</c>.</returns>
        Task<bool> ResendEmailVerificationAsync(string code);

        /// <summary>
        ///     Gets the pending team membership request for the user with id being <see cref="userId" />.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The pending <see cref="TeamMembershipRequest" /> if it exists; otherwise <c>NULL</c></returns>
        Task<TeamMembershipRequest> GetPendingTeamMembershipRequest(Guid userId);

        /// <summary>
        ///     Removes the user from the team.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The updated <see cref="TeamTunerUser" />.</returns>
        Task<TeamTunerUser> LeaveTeam(Guid userId);

        /// <summary>
        ///     Updates the team role for the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="role">The role.</param>
        Task UpdateTeamRoleAsync(Guid userId, string role);
    }
}