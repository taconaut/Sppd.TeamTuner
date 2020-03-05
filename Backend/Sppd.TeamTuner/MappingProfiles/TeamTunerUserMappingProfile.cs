using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Enumerations;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="TeamTunerUser" />.
    /// </summary>
    /// <seealso cref="EntityMappingProfileBase{TeamTunerUser}" />
    internal class TeamTunerUserMappingProfile : EntityMappingProfileBase<TeamTunerUser>
    {
        public TeamTunerUserMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<UserResponseDto>()
                .ForMember(dst => dst.TeamName, exp => exp.MapFrom(src => src.Team == null ? null : src.Team.Name));
            CreateEntityToDtoMap<UserAuthorizationResponseDto>();

            // DTO -> Entity
            CreateMap<UserCreateRequestDto, TeamTunerUser>()
                .ForMember(dest => dest.ApplicationRole, opt => opt.MapFrom(src => CoreConstants.Authorization.Roles.USER))
                // TODO: Allow to configure this in the frontend
                .ForMember(dest => dest.ProfileVisibility, opt => opt.MapFrom(src => UserProfileVisibility.Team));
            CreateMap<AuthorizationRequestDto, TeamTunerUser>();
            CreateMap<UserUpdateRequestDto, TeamTunerUser>();

            CreateDtoToEntityMap<UserResponseDto>();
            CreateDtoToEntityMap<UserUpdateRequestDto>();
        }
    }
}