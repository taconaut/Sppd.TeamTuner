using System;

using Sppd.TeamTuner.Core.Domain.Interfaces;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    public class TeamTunerUserRegistrationRequest : BaseEntity, IAuthorizing
    {
        public Guid UserId { get; set; }

        public TeamTunerUser User { get; set; }

        public Guid RegistrationCode { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ITeamTunerUser AuthorizingUser => User;
    }
}