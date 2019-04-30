using Sppd.TeamTuner.Core.Domain.Enumerations;

namespace Sppd.TeamTuner.DTOs
{
    public class UserUpdateRequestDto : UpdateRequestDto
    {
        public string Name { get; set; }

        public string SppdName { get; set; }

        public string Email { get; set; }

        public byte[] Avatar { get; set; }

        public string Description { get; set; }

        public UserProfileVisibility ProfileVisibility { get; set; }
    }
}