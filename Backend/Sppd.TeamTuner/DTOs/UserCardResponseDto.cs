using System;

namespace Sppd.TeamTuner.DTOs
{
    public class UserCardResponseDto
    {
        public Guid CardId { get; set; }

        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public Guid ThemeId { get; set; }

        public Guid RarityId { get; set; }

        public Guid TypeId { get; set; }

        public int? Level { get; set; }
    }
}