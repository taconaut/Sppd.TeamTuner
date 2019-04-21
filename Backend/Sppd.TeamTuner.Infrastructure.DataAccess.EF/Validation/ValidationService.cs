using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Domain.Validation;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Core.Validation;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Validation
{
    /// <summary>
    ///     Validates <see cref="BaseEntity" /> by:<br />
    ///     - checking attributes
    ///     - calling <see cref="IValidatable.Validate" />
    /// </summary>
    internal class ValidationService : IValidationService
    {
        private readonly IValidationContext _validationContext;
        private readonly Lazy<ChangeTracker> _changeTracker;

        public ValidationService(IValidationContext validationContext, IServiceProvider serviceProvider)
        {
            _validationContext = validationContext;
            _changeTracker = new Lazy<ChangeTracker>(() => serviceProvider.GetService<TeamTunerContext>().ChangeTracker);
        }

        public EntityValidationResultCollection ValidateAllChangedEntities()
        {
            return ValidateMultipleEntities(_changeTracker.Value.Entries().Where(IsEntityEntryChanged).Select(e => e.Entity).OfType<BaseEntity>());
        }

        private EntityValidationResult ValidateSingleEntity(BaseEntity entity)
        {
            var allValidationErrors = ValidateDataAnnotationAttributes(entity).Union(ValidateValidatable(entity));
            var validationErrors = new ReadOnlyCollection<EntityValidationError>(allValidationErrors.ToList());
            return new EntityValidationResult(entity, validationErrors);
        }

        private EntityValidationResultCollection ValidateMultipleEntities(IEnumerable<BaseEntity> entities)
        {
            var allEntityValidationResults = entities.Select(ValidateSingleEntity);
            var validationResults = new ReadOnlyCollection<EntityValidationResult>(allEntityValidationResults.ToArray());
            return new EntityValidationResultCollection(validationResults);
        }

        private static bool IsEntityEntryChanged(EntityEntry entry)
        {
            return entry.State == EntityState.Added || entry.State == EntityState.Modified;
        }

        private static IEnumerable<EntityValidationError> ValidateDataAnnotationAttributes(BaseEntity entity)
        {
            var dataAnnotationValidationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var dataAnnotationResults = new Collection<ValidationResult>();
            Validator.TryValidateObject(entity, dataAnnotationValidationContext, dataAnnotationResults, true);

            foreach (var dataAnnotationResult in dataAnnotationResults)
            {
                var errorMessage = dataAnnotationResult.ErrorMessage;
                if (errorMessage == null)
                {
                    throw new NotSupportedException(
                        $"Not supported data annotation validation in entity '{entity.GetType().Name}'.");
                }

                yield return new EntityValidationError(errorMessage, dataAnnotationResult.MemberNames.Single());
            }
        }

        private IEnumerable<EntityValidationError> ValidateValidatable(BaseEntity entity)
        {
            if (entity is IValidatable validatable)
            {
                return validatable.Validate(_validationContext).ToList();
            }

            return Enumerable.Empty<EntityValidationError>();
        }
    }
}