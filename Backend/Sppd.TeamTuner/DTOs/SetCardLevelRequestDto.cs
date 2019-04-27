using System;

namespace Sppd.TeamTuner.DTOs
{
    public class SetCardLevelRequestDto
    {
        public Guid UserId { get; set; }

        public Guid CardId { get; set; }

        public int Level { get; set; }
    }
}