using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    public class SystemInfoMappingProfile : Profile
    {
        public SystemInfoMappingProfile()
        {
            CreateMap<SystemInfo, SystemInfoDto>()
                .ForMember(dst => dst.Version, ctx => ctx.MapFrom(src => src.Version.ToString()));
        }
    }
}