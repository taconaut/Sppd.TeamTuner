using System;

namespace Sppd.TeamTuner.Core.Exceptions
{
    /// <summary>
    ///     Exception thrown when an entity could not be found.
    /// </summary>
    /// <seealso cref="BusinessException" />
    [Serializable]
    public class EntityNotFoundException : BusinessException
    {
        public Type EntityType { get; }

        public string Identifier { get; }

        public EntityNotFoundException(Type entityType, string identifier)
        {
            EntityType = entityType;
            Identifier = identifier;
        }

        public EntityNotFoundException(string message, Type entityType, string identifier)
            : base(message)
        {
            EntityType = entityType;
            Identifier = identifier;
        }

        public EntityNotFoundException(string message, Exception innerException, Type entityType, string identifier)
            : base(message, innerException)
        {
            EntityType = entityType;
            Identifier = identifier;
        }
    }
}