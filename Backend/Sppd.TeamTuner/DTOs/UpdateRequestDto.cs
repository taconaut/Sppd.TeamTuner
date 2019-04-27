using System;
using System.Collections.Generic;

namespace Sppd.TeamTuner.DTOs
{
    public class UpdateRequestDto : IVersionedDto
    {
        public Guid Id { get; set; }

        public IEnumerable<string> PropertiesToUpdate { get; set; }

        public string Version { get; set; }
    }
}