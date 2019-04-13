using System;

namespace Sppd.TeamTuner.DTOs
{
    public abstract class EntityDto : IVersionedDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime ModifiedOnUtc { get; set; }

        public string Version { get; set; }
    }
}