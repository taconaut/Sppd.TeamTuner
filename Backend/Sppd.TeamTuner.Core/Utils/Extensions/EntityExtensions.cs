using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Utils.Extensions
{
    /// <summary>
    ///     Extension methods for entities
    /// </summary>
    public static class EntityExtensions
    {
        private static readonly PropertyInfo s_versionPropertyInfo = typeof(BaseEntity).GetProperty(nameof(BaseEntity.Version));

        /// <summary>
        ///     Maps the properties from <see cref="entitySource" /> to <see cref="entityDest" />. If
        ///     <see cref="propertyNames" /> has been specified, only those properties will be updated; if it hasn't, all public
        ///     properties will be mapped.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityDest">The destination entity.</param>
        /// <param name="entitySource">The source entity.</param>
        /// <param name="propertyNames">The property names to update.</param>
        /// <param name="includeBaseEntityProperties">
        ///     if set to <c>true</c>, properties of <see cref="BaseEntity" /> will also be
        ///     mapped.
        /// </param>
        /// <exception cref="BusinessException">
        ///     Thrown if a property specified in <see cref="propertyNames" /> could not be found for <see cref="TEntity" />
        /// </exception>
        public static void MapProperties<TEntity>(this TEntity entityDest, TEntity entitySource, IEnumerable<string> propertyNames = null, bool includeBaseEntityProperties = false)
            where TEntity : BaseEntity
        {
            var entityProperties = GetEntityProperties<TEntity>(includeBaseEntityProperties);
            var entityPropertiesToUpdate = GetEntityPropertiesToUpdate(entityProperties, propertyNames);

            foreach (var propertyInfo in entityPropertiesToUpdate)
            {
                var newValue = propertyInfo.GetValue(entitySource);
                propertyInfo.SetValue(entityDest, newValue);
            }
        }

        private static IEnumerable<PropertyInfo> GetEntityPropertiesToUpdate(IEnumerable<PropertyInfo> entityProperties, IEnumerable<string> propertyNamesToUpdate)
        {
            var propertyNames = propertyNamesToUpdate?.ToList();
            if (propertyNames == null || !propertyNames.Any())
            {
                return entityProperties;
            }

            // Always map Version
            propertyNames.Add(nameof(BaseEntity.Version));

            // Throw if an unknown property is found
            var unknownPropertyNames = propertyNames.Where(pn => !entityProperties.Select(p => p.Name).Contains(pn)).ToList();
            if (unknownPropertyNames.Any())
            {
                throw new BusinessException($"Unknown property names: {string.Join(", ", unknownPropertyNames)}");
            }

            return entityProperties.Where(p => propertyNames.Contains(p.Name));
        }

        private static IEnumerable<PropertyInfo> GetEntityProperties<TEntity>(bool includeBaseEntityProperties)
        {
            var entityProperties = typeof(TEntity).GetProperties();

            return includeBaseEntityProperties
                ? entityProperties
                : entityProperties.Where(p => IsBaseEntityProperty(typeof(BaseEntity).GetProperties(), p) || MustInclude(p));
        }

        private static bool MustInclude(MemberInfo propertyInfo)
        {
            return AreEqual(propertyInfo, s_versionPropertyInfo);
        }

        private static bool IsBaseEntityProperty(IEnumerable<PropertyInfo> baseEntityProperties, MemberInfo pt)
        {
            return !baseEntityProperties.Any(pb => AreEqual(pb, pt));
        }

        private static bool AreEqual(MemberInfo p1, MemberInfo p2)
        {
            return p1.MetadataToken == p2.MetadataToken && p1.Module.Equals(p2.Module);
        }
    }
}