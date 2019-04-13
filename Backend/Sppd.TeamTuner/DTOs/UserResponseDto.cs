using System;

namespace Sppd.TeamTuner.DTOs
{
    public class UserResponseDto : DescriptiveDto
    {
        public string SppdName { get; set; }

        public string Email { get; set; }

        public string ApplicationRole { get; set; }

        public Guid? TeamId { get; set; }

        public string TeamRole { get; set; }

        public Guid? FederationId { get; set; }

        public string FederationRole { get; set; }
    }
}