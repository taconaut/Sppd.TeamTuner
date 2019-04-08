using System;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Core.Domain
{
    /// <summary>
    ///     The system user is being used to perform system operations (e.g. seeding data on startup)
    /// </summary>
    /// <seealso cref="ITeamTunerUser" />
    public class SystemUser : ITeamTunerUser
    {
        public Guid Id => Guid.Parse("6EF20A5A-6A71-4945-80C1-8F690E84D52F");

        public string Name => "System";

        public string SppdName => string.Empty;

        public byte[] PasswordHash => null;

        public byte[] PasswordSalt => null;

        public string Email => "dummy@mail.com";

        public string ApplicationRole => CoreConstants.Auth.Roles.ADMIN;

        public Guid? TeamId => null;

        public Team Team => null;

        public string TeamRole => null;

        public Guid? FederationId => null;

        public Federation Federation => null;

        public string FederationRole => null;
    }
}