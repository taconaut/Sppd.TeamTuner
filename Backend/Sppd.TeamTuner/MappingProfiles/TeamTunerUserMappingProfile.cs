using AutoMapper;

using Sppd.TeamTuner.Core;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    internal class TeamTunerUserMappingProfile : Profile
    {
        public TeamTunerUserMappingProfile()
        {
            // Entity -> DTO
            CreateMap<TeamTunerUser, UserDto>();
            CreateMap<TeamTunerUser, UserAuthenticateDto>();

            // DTO -> Entity
            CreateMap<UserCreateDto, TeamTunerUser>()
                .ForMember(dest => dest.ApplicationRole, opt => opt.MapFrom(src => CoreConstants.Auth.Roles.USER));
            CreateMap<UserUpdateDto, TeamTunerUser>();
            CreateMap<UserLoginDto, TeamTunerUser>();
            CreateMap<UserDto, TeamTunerUser>();
        }
    }
}