using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// Validation exception containing entity and corresponding invalid attribute names.
    /// </summary>
    public class EntityValidationException : Exception 
    {
        /// <summary>
        /// Invalid attribute names.
        /// </summary>
        public readonly IEnumerable<string> InvalidAttributeNames;

        /// <summary>
        /// Entity that caused validation to fail.
        /// </summary>
        public readonly Entity Entity;
        
        /// <summary>
        /// Validation exception constructor.
        /// </summary>
        /// <param name="entity">Invalid entity</param>
        /// <param name="invalidAttributeNames">Validation result - invalid attribute names</param>
        public EntityValidationException(Entity entity, IEnumerable<string> invalidAttributeNames) 
        {
            Entity = entity;
            InvalidAttributeNames = invalidAttributeNames;
        }
    }
}