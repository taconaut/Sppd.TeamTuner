using System;

namespace Sppd.TeamTuner.DTOs
{
    public class CardLevelResponseDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CardId { get; set; }

        public int Level { get; set; }
    }
}