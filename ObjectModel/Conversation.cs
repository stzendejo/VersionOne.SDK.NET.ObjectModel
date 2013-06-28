using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Conversation entity.
    /// </summary>
    [MetaData("Conversation")]
    public class Conversation : Entity {
        internal Conversation(V1Instance instance) : base(instance) {}
        internal Conversation(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// Child expressions belonging to current conversation.
        /// </summary>
        public ICollection<Expression> ContainedExpressions
        {
            get { return GetMultiRelation<Expression>("ContainedExpressions"); }
        }

        /// <summary>
        /// True if the conversation can be deleted.
        /// </summary>
        public virtual bool CanDelete
        {
            get { return Instance.CanExecuteOperation(this, "Delete"); }
        }

        /// <summary>
        /// Deletes the conversation.
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
        public void Delete()
        {
            Save();
            Instance.ExecuteOperation(this, "Delete");
        }
    }
}