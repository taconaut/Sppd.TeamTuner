using System;
using System.Collections.Generic;
using System.Linq;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     A team holds a set of <see cref="TeamTunerUser" />s.
    /// </summary>
    /// <seealso cref="DescriptiveEntity" />
    public class Team : DescriptiveEntity
    {
        private ICollection<TeamTunerUser> _users;

        public Guid? FederationId { get; set; }

        /// <summary>
        ///     The federation, the team belongs to, if any.
        /// </summary>
        public Federation Federation { get; set; }

        /// <summary>
        ///     The users belonging to the team.
        /// </summary>
        public ICollection<TeamTunerUser> Users
        {
            get => _users ?? (_users = new List<TeamTunerUser>());
            set => _users = value;
        }

        /// <summary>
        ///     The team members.
        /// </summary>
        public IEnumerable<TeamTunerUser> Members => Users.Where(u => Equals(u.TeamRole, CoreConstants.Auth.Roles.MEMBER));

        /// <summary>
        ///     The team co-leaders.
        /// </summary>
        public IEnumerable<TeamTunerUser> CoLeaders => Users.Where(u => Equals(u.TeamRole, CoreConstants.Auth.Roles.CO_LEADER));

        /// <summary>
        ///     The team leader.
        /// </summary>
        public TeamTunerUser Leader => Users.SingleOrDefault(u => Equals(u.TeamRole, CoreConstants.Auth.Roles.LEADER));
    }
}