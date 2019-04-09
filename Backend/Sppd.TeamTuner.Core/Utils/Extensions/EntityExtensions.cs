using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Utils.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        ///     Maps the properties from <see cref="entitySource" /> to <see cref="entityDest" />. If
        ///     <see cref="propertyNames" /> has been specified, only those properties will be updated; if it hasn't been
        ///     specified, all public properties, except the ones from <see cref="BaseEntity" /> will be mapped.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityDest">The destination entity.</param>
        /// <param name="entitySource">The source entity.</param>
        /// <param name="propertyNames">The property names to update.</param>
        /// <param name="includeBaseEntityProperties">if set to <c>true</c> [include base entity properties].</param>
        /// <exception cref="BusinessException">
        ///     Unknown property names: {string.Join(", ",
        ///     unknownPropertyNames)}
        /// </exception>
        /// <exception cref="BusinessException">
        ///     Thrown if a property specified in <see cref="propertyNames" /> could not be
        ///     found for <see cref="TEntity" />
        /// </exception>
        public static void MapProperties<TEntity>(this TEntity entityDest, TEntity entitySource, IEnumerable<string> propertyNames = null, bool includeBaseEntityProperties = false)
            where TEntity : BaseEntity
        {
            var entityProperties = GetEntityProperties<TEntity>(includeBaseEntityProperties);
            var filteredEntityProperties = GetFilteredEntityProperties(entityProperties, propertyNames);

            foreach (var propertyInfo in filteredEntityProperties)
            {
                var newValue = propertyInfo.GetValue(entitySource);
                propertyInfo.SetValue(entityDest, newValue);
            }
        }

        private static IEnumerable<PropertyInfo> GetFilteredEntityProperties(IEnumerable<PropertyInfo> entityProperties, IEnumerable<string> propertyNamesToUpdate)
        {
            if (propertyNamesToUpdate == null)
            {
                return entityProperties;
            }

            var toUpdateNames = propertyNamesToUpdate.ToList();
            var unknownPropertyNames = toUpdateNames.Where(pn => !entityProperties.Select(p => p.Name).Contains(pn)).ToList();
            if (unknownPropertyNames.Any())
            {
                throw new BusinessException($"Unknown property names: {string.Join(", ", unknownPropertyNames)}");
            }

            return entityProperties.Where(p => toUpdateNames.Contains(p.Name));
        }

        private static IEnumerable<PropertyInfo> GetEntityProperties<TEntity>(bool includeBaseEntityProperties)
        {
            var baseEntityProperties = typeof(BaseEntity).GetProperties();
            var entityProperties = typeof(TEntity).GetProperties();

            return includeBaseEntityProperties ? entityProperties : entityProperties.Where(pt => !baseEntityProperties.Any(pb => AreEqual(pb, pt)));
        }

        private static bool AreEqual(MemberInfo p1, MemberInfo p2)
        {
            return p1.MetadataToken == p2.MetadataToken && p1.Module.Equals(p2.Module);
        }
    }
}