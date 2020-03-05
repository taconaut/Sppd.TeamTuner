using System.Linq;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="TeamCard" />.
    /// </summary>
    /// <seealso cref="CardMappingProfileBase" />
    internal class TeamCardMappingProfile : CardMappingProfileBase
    {
        public TeamCardMappingProfile()
        {
            // Entity -> DTO
            CreateBaseCardToDtoMap<TeamCard, TeamCardResponseDto>()
                .ForMember(dst => dst.Levels, exp => exp.MapFrom(src => src.Levels.ToDictionary(l => l.Key, l => l.Value.Count())));
        }
    }
}