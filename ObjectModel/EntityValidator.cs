using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// Entity validator that can be used on-demand and is hooked into entity Save/Create actions
    /// </summary>
    public class EntityValidator 
    {
        private readonly V1Instance V1Instance;
        private readonly RequiredFieldValidator validator;

        /// <summary>
        /// Entity validator constructor.
        /// </summary>
        /// <param name="instance">V1Instance to retrieve APIClient required field validator</param>
        public EntityValidator(V1Instance instance) 
        {
            V1Instance = instance;
            validator = V1Instance.GetRequiredFieldValidator();
        }

        /// <summary>
        /// Validate single attribute of an entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="attribute">Name of attribute to be validated</param>
        /// <returns>true, if attribute value is valid; false, otherwise.</returns>
        public bool Validate(Entity entity, string attribute) 
        {
            Asset asset = V1Instance.GetAsset(entity.InstanceKey) ?? V1Instance.GetAsset(entity) ;
            IAttributeDefinition attributeDefinition = asset.AssetType.GetAttributeDefinition(attribute);
            return validator.Validate(asset, attributeDefinition);
        }

        /// <summary>
        /// Validate single Entity.
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>Collection of invalid attribute names</returns>
        public ICollection<string> Validate(Entity entity) 
        {
            Asset asset = V1Instance.GetAsset(entity.InstanceKey) ?? V1Instance.GetAsset(entity);
            return Validate(asset);
        }

        /// <summary>
        /// Validate a collection of entities.
        /// </summary>
        /// <param name="entities">Entities to validate</param>
        /// <returns>Dictionary where Entities are keys, and corresponding validation results are values (<see cref="Validate(Entity)"/>).</returns>
        public IDictionary<Entity, ICollection<string>> Validate(IEnumerable<Entity> entities) {
            IDictionary<Entity, ICollection<string>> results = new Dictionary<Entity, ICollection<string>>();

            foreach (Entity entity in entities) 
            {
                results.Add(entity, Validate(entity));
            }

            return results;
        }

        /// <summary>
        /// Pass asset to required field validator.
        /// We cannot resolve asset by AssetID because in newly created entities it is null until Commit() is actually executed, that's why it is used directly.
        /// </summary>
        /// <param name="asset">Asset to validate.</param>
        /// <returns>Collection of invalid attribute names</returns>
        internal ICollection<string> Validate(Asset asset) 
        {            
            ICollection<IAttributeDefinition> result = validator.Validate(asset);
            return GetAttributeNames(result);
        }

        private static ICollection<string> GetAttributeNames(ICollection<IAttributeDefinition> attributeDefinitions) 
        {
            ICollection<string> attributeNames = new List<string>(attributeDefinitions.Count);
            
            foreach (IAttributeDefinition attributeDefinition in attributeDefinitions) 
            {
                attributeNames.Add(attributeDefinition.Name);
            }

            return attributeNames;
        }
    }
}