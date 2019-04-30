using System.Collections.Generic;
using System.Linq;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    /// <summary>
    ///     A <see cref="Federation" /> is a group of <see cref="Team" />s
    /// </summary>
    /// <seealso cref="DescriptiveEntity" />
    public class Federation : DescriptiveEntity
    {
        private ICollection<Team> _teams;

        public ICollection<Team> Teams
        {
            get => _teams ?? (_teams = new List<Team>());
            set => _teams = value;
        }

        /// <summary>
        ///     The federation users.
        /// </summary>
        public IEnumerable<TeamTunerUser> Users => Teams.SelectMany(t => t.Users);

        /// <summary>
        ///     The federation members.
        /// </summary>
        public IEnumerable<TeamTunerUser> Members => Users.Where(u => Equals(u.TeamRole, CoreConstants.Authorization.Roles.MEMBER));

        /// <summary>
        ///     The federation co-leaders.
        /// </summary>
        public IEnumerable<TeamTunerUser> CoLeaders => Users.Where(u => Equals(u.FederationRole, CoreConstants.Authorization.Roles.CO_LEADER));

        /// <summary>
        ///     The federation leader.
        /// </summary>
        public TeamTunerUser Leader => Users.SingleOrDefault(u => Equals(u.FederationRole, CoreConstants.Authorization.Roles.LEADER));
    }
}