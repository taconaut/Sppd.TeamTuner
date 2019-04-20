using System;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamJoinRequestResponseDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }
    }
}