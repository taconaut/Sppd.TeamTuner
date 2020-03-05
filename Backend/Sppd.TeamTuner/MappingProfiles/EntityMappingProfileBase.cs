using AutoMapper;

using Microsoft.AspNetCore.WebUtilities;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.DTOs;

namespace Sppd.TeamTuner.MappingProfiles
{
    /// <summary>
    ///     Automapper base mapping profile for entities extending <see cref="BaseEntity" />.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="Profile" />
    internal abstract class EntityMappingProfileBase<TEntity> : Profile
        where TEntity : BaseEntity
    {
        protected IMappingExpression<TDto, TEntity> CreateDtoToEntityMap<TDto>()
            where TDto : IVersionedDto
        {
            return CreateMap<TDto, TEntity>()
                .ForMember(dst => dst.Version, exp => exp.MapFrom(src => Base64UrlTextEncoder.Decode(src.Version)));
        }

        protected IMappingExpression<TEntity, TDto> CreateEntityToDtoMap<TDto>()
            where TDto : IVersionedDto
        {
            return CreateMap<TEntity, TDto>()
                .ForMember(dst => dst.Version, exp => exp.MapFrom(src => Base64UrlTextEncoder.Encode(src.Version)));
        }
    }
}