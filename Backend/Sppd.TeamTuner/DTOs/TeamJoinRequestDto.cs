using System;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamJoinRequestDto
    {
        public Guid TeamId { get; set; }

        public Guid UserId { get; set; }

        public string Comment { get; set; }
    }
}