using System;
using System.ComponentModel.DataAnnotations;

namespace Sppd.TeamTuner.Core.Domain.Entities
{
    public class TeamJoinRequest : BaseEntity
    {
        public Team Team { get; set; }

        public Guid TeamId { get; set; }

        public TeamTunerUser User { get; set; }

        public Guid UserId { get; set; }

        [StringLength(CoreConstants.StringLength.TeamJoinRequest.COMMENT)]
        public string Comment { get; set; }
    }
}