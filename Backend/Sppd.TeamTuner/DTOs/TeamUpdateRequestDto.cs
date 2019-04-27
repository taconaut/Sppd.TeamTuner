using System;

namespace Sppd.TeamTuner.DTOs
{
    public class TeamUpdateRequestDto : UpdateRequestDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Avatar { get; set; }

        public Guid? FederationId { get; set; }
    }
}