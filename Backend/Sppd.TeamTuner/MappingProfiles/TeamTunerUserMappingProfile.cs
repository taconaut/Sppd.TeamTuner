using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamTunerUserMappingProfile : BaseMappingProfile<TeamTunerUser>
    {
        public TeamTunerUserMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<UserResponseDto>();
            CreateEntityToDtoMap<UserLoginResponseDto>();

            // DTO -> Entity
            CreateMap<UserCreateRequestDto, TeamTunerUser>()
                .ForMember(dest => dest.ApplicationRole, opt => opt.MapFrom(src => CoreConstants.Auth.Roles.USER));
            CreateMap<UserLoginRequestDto, TeamTunerUser>();

            CreateDtoToEntityMap<UserResponseDto>();
            CreateDtoToEntityMap<UserUpdateRequestDto>();
        }
    }
}