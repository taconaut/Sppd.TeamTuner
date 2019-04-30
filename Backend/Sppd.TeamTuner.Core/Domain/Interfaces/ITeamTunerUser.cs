using System;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Enumerations;

namespace Sppd.TeamTuner.Core.Domain.Interfaces
{
    /// <summary>
    ///     Interface for team tuner user
    /// </summary>
    public interface ITeamTunerUser
    {
        Guid Id { get; }

        string Name { get; }

        string SppdName { get; }

        byte[] PasswordHash { get; }

        byte[] PasswordSalt { get; }

        string Email { get; }

        string ApplicationRole { get; }

        Guid? TeamId { get; }

        Team Team { get; }

        string TeamRole { get; }

        Guid? FederationId { get; }

        Federation Federation { get; }

        string FederationRole { get; }

        /// <summary>
        ///     Determines who will be able to see the user profile.
        /// </summary>
        UserProfileVisibility ProfileVisibility { get; }
    }
}