namespace Sppd.TeamTuner.Core.Domain.Validation
{
    /// <summary>
    ///     Specifies an entity validation error containing a message and possibly the property name.
    /// </summary>
    public class EntityValidationError
    {
        public string ErrorMessage { get; }

        public string PropertyName { get; }

        public EntityValidationError(string errorMessage, string propertyName = null)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }

        public override string ToString()
        {
            return $"Property={PropertyName}, ErrorMessage={ErrorMessage}";
        }
    }
}