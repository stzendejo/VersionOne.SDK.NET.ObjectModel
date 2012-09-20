using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for Tasks
    /// </summary>
    public class TaskFilter : SecondaryWorkitemFilter {
        internal override System.Type EntityType {
            get { return typeof (Task); }
        }

        /// <summary>
        /// Build number associated with the Task.  Must be complete match.
        /// </summary>
        public readonly ICollection<string> Build = new List<string>();

        /// <summary>
        /// The Source of this Task
        /// </summary>
        public readonly ICollection<string> Source = new List<string>();

        /// <summary>
        /// The Tasks's Type.  Must be complete match.
        /// </summary>
        public readonly ICollection<string> Type = new List<string>();

        /// <summary>
        /// The Status of the Task
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("LastVersion", Build);

            builder.ListRelation<TaskSource>("Source", Source);
            builder.ListRelation<TaskType>("Category", Type);
            builder.ListRelation<TaskStatus>("Status", Status);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if (HasState)
                if (HasActive)
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='Task'"));
                else
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='Task'"));
            else
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Task'"));
        }
    }
}