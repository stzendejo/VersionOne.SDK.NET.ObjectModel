using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for getting Epics.
    /// </summary>
    public class EpicFilter : WorkitemFilter {
        internal override Type EntityType {
            get { return typeof (Epic); }
        }

        /// <summary>
        /// Filter on Status.
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// Filter on Source.
        /// </summary>
        public readonly ICollection<string> Source = new List<string>();

        /// <summary>
        /// Filter on Theme assigned.
        /// </summary>
        public readonly ICollection<Theme> Theme = new List<Theme>();

        /// <summary>
        /// Filter on Risk.
        /// </summary>
        public readonly NullableDoubleSearcher Risk = new NullableDoubleSearcher();

        /// <summary>
        /// Filter on Swag.
        /// </summary>
        public readonly NullableDoubleSearcher Swag = new NullableDoubleSearcher();

        /// <summary>
        /// Filter on Value.
        /// </summary>
        public readonly NullableDoubleSearcher Value = new NullableDoubleSearcher();

        /// <summary>
        /// Filter on Type.
        /// </summary>
        public readonly ICollection<string> Type = new List<string>();

        /// <summary>
        /// Filter on Priority.
        /// </summary>
        public readonly ICollection<string> Priority = new List<string>();

        /// <summary>
        /// Filter on Parent.
        /// </summary>
        public readonly ICollection<Epic> Parent = new List<Epic>();

        /// <summary>
        /// Filter on Goals.
        /// </summary>
        public readonly ICollection<Goal> Goals = new List<Goal>();

        /// <summary>
        /// Filter on Issues.
        /// </summary>
        public readonly ICollection<Issue> Issues = new List<Issue>();

        /// <summary>
        /// Filter on Blocked Issues.
        /// </summary>
        public readonly ICollection<Issue> BlockingIssues = new List<Issue>();

        /// <summary>
        /// Filter on Requests.
        /// </summary>
        public readonly ICollection<Request> Requests = new List<Request>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("Reference", Reference);
            
            builder.Comparison("Risk", Risk);
            builder.Comparison("Swag", Swag);
            builder.Comparison("Value", Value);

            builder.Relation("Parent", Theme);
            builder.Relation("Super", Parent);

            builder.MultiRelation("Goals", Goals);
            builder.MultiRelation("Issues", Issues);
            builder.MultiRelation("BlockingIssues", BlockingIssues);
            builder.MultiRelation("Owners", Owners);
            builder.MultiRelation("Requests", Requests);

            builder.ListRelation<EpicStatus>("Status", Status);
            builder.ListRelation<WorkitemSource>("Source", Source);
            builder.ListRelation<EpicType>("Category", Type);
            builder.ListRelation<EpicPriority>("Priority", Priority);
        }

        //TODO investigate if this method is redundant and filter can work using base class implementation
        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive ? new TokenTerm("AssetState='Active';AssetType='Epic'") : new TokenTerm("AssetState='Closed';AssetType='Epic'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Epic'"));
            }
        }
    }
}