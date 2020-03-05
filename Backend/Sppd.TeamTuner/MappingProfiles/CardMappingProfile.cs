using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper mapping profile for <see cref="Card" />
    /// </summary>
    /// <seealso cref="EntityMappingProfileBase{Card}" />
    internal class CardMappingProfile : EntityMappingProfileBase<Card>
    {
        public CardMappingProfile()
        {
            // Entity -> DTO
            CreateEntityToDtoMap<CardResponseDto>();
        }
    }
}