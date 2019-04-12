﻿using System;
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

        Task<IEnumerable<TeamTunerUser>> GetByTeamIdAsync(Guid teamId);
    }
}