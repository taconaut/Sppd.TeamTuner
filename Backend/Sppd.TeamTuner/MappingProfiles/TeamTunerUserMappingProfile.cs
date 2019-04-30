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
            CreateEntityToDtoMap<UserAuthorizationResponseDto>();

            // DTO -> Entity
            CreateMap<UserCreateRequestDto, TeamTunerUser>()
                .ForMember(dest => dest.ApplicationRole, opt => opt.MapFrom(src => CoreConstants.Authorization.Roles.USER));
            CreateMap<AuthorizationRequestDto, TeamTunerUser>();
            CreateMap<UserUpdateRequestDto, TeamTunerUser>();

            CreateDtoToEntityMap<UserResponseDto>();
            CreateDtoToEntityMap<UserUpdateRequestDto>();
        }
    }
}