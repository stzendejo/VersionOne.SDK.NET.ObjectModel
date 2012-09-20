using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for getting stories.
    /// </summary>
    public class StoryFilter : PrimaryWorkitemFilter {
        internal override Type EntityType {
            get { return typeof (Story); }
        }

        /// <summary>
        /// Name of person or organization requesting this Story.  Must be complete match.
        /// </summary>
        public readonly ICollection<string> RequestedBy = new List<string>();

        /// <summary>
        /// Build number associated with this Story.  Must be complete match.
        /// </summary>
        public readonly ICollection<string> Build = new List<string>();

        /// <summary>
        /// This Story's Risk.
        /// </summary>
        public readonly ICollection<string> Risk = new List<string>();

        /// <summary>
        /// This Story's Type.
        /// </summary>
        public readonly ICollection<string> Type = new List<string>();

        /// <summary>
        /// Member assigned as a customer for this Story.
        /// </summary>
        public readonly ICollection<Member> Customer = new List<Member>();

        /// <summary>
        /// Stories that this Story depends on.
        /// </summary>
        public readonly ICollection<Story> DependsOnStories = new List<Story>();

        /// <summary>
        /// Stories that depend on this Story.
        /// </summary>
        public readonly ICollection<Story> DependentStories = new List<Story>();

        /// <summary>
        /// The (optional) Retrospective this Story was Identified in.
        /// </summary>
        public readonly ICollection<Retrospective> IdentifiedIn = new List<Retrospective>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("RequestedBy", RequestedBy);
            builder.Simple("LastVersion", Build);
            builder.Relation("Customer", Customer);
            builder.Relation("Dependencies", DependsOnStories);
            builder.Relation("IdentifiedIn", IdentifiedIn);

            builder.MultiRelation("Dependants", DependentStories);

            builder.ListRelation<WorkitemRisk>("Risk", Risk);
            builder.ListRelation<StoryType>("Category", Type);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive
                                     ? new TokenTerm("AssetState='Active';AssetType='Story'")
                                     : new TokenTerm("AssetState='Closed';AssetType='Story'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Story'"));
            }
        }
    }
}