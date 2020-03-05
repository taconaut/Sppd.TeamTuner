using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="SystemInfo" />.
    /// </summary>
    /// <seealso cref="Profile" />
    public class SystemInfoMappingProfile : Profile
    {
        public SystemInfoMappingProfile()
        {
            CreateMap<SystemInfo, SystemInfoDto>()
                .ForMember(dst => dst.Version, ctx => ctx.MapFrom(src => src.Version.ToString()));
        }
    }
}