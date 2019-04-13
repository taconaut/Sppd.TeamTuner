using System;
using System.Collections.Generic;

namespace Sppd.TeamTuner.DTOs
{
    public class UserUpdateRequestDto : IVersionedDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SppdName { get; set; }

        public string Email { get; set; }

        public byte[] Avatar { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> PropertiesToUpdate { get; set; }

        public string Version { get; set; }
    }
}