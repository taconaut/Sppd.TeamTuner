using AutoMapper;

using Sppd.TeamTuner.Core.Domain.Objects;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper base mapping profile for classes extending <see cref="BaseCard" />
    /// </summary>
    /// <seealso cref="Profile" />
    internal abstract class CardMappingProfileBase : Profile
    {
        protected IMappingExpression<TBaseCard, TCardResponseBaseDto> CreateBaseCardToDtoMap<TBaseCard, TCardResponseBaseDto>()
            where TCardResponseBaseDto : CardResponseBaseDto
            where TBaseCard : BaseCard
        {
            return CreateMap<TBaseCard, TCardResponseBaseDto>()
                   .ForMember(dst => dst.CardId, exp => exp.MapFrom(src => src.Card.Id))
                   .ForMember(dst => dst.CardName, exp => exp.MapFrom(src => src.Card.Name))
                   .ForMember(dst => dst.ThemeId, exp => exp.MapFrom(src => src.Card.ThemeId))
                   .ForMember(dst => dst.RarityId, exp => exp.MapFrom(src => src.Card.RarityId))
                   .ForMember(dst => dst.CharacterTypeId, exp => exp.MapFrom(src => src.Card.CharacterTypeId))
                   .ForMember(dst => dst.TypeId, exp => exp.MapFrom(src => src.Card.TypeId));
            ;
        }
    }
}