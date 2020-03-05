using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="UserCards" />.
    /// </summary>
    /// <seealso cref="Profile" />
    public class UserCardsMappingProfile : Profile
    {
        public UserCardsMappingProfile()
        {
            // Entity -> DTO
            CreateMap<UserCards, UserCardsResponseDto>()
                .ForMember(dst => dst.UserId, exp => exp.MapFrom(src => src.User.Id));
        }
    }
}