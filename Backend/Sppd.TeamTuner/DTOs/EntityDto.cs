using System;

namespace Sppd.TeamTuner.DTOs
{
    public abstract class EntityDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime ModifiedOnUtc { get; set; }
    }
}