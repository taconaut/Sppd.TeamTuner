using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="TeamCards" />.
    /// </summary>
    /// <seealso cref="Profile" />
    internal class TeamCardsMappingProfile : Profile
    {
        public TeamCardsMappingProfile()
        {
            // Entity -> DTO
            CreateMap<TeamCards, TeamCardsResponseDto>()
                .ForMember(dest => dest.TeamId, exp => exp.MapFrom(src => src.Team.Id));
        }
    }
}