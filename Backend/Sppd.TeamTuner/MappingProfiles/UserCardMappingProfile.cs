using System;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="UserCard" />.
    /// </summary>
    /// <seealso cref="CardMappingProfileBase" />
    internal class UserCardMappingProfile : CardMappingProfileBase
    {
        public UserCardMappingProfile()
        {
            // Entity -> DTO
            CreateBaseCardToDtoMap<UserCard, UserCardResponseDto>()
                .ForMember(dst => dst.Level, exp => exp.MapFrom(src => src.Level == null ? (int?) null : src.Level.Level))
                .ForMember(dst => dst.LevelLastModified, exp => exp.MapFrom(src => src.Level == null ? (DateTime?) null : src.Level.ModifiedOnUtc));
        }
    }
}