using System;
using System.Collections.Generic;

namespace Sppd.TeamTuner.DTOs
{
    internal class TeamResponseDto : DescriptiveDto
    {
        public Guid? FederationId { get; set; }

        public string Leader { get; set; }

        public IEnumerable<string> CoLeaders { get; set; }

        public IEnumerable<string> Members { get; set; }
    }
}