using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for PrimaryWorkitems
    /// </summary>
    public class PrimaryWorkitemFilter : WorkitemFilter {
        internal override Type EntityType {
            get { return typeof (PrimaryWorkitem); }
        }

        /// <summary>
        /// Filter on Story status.
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// Filter on Team assigned.
        /// </summary>
        public readonly ICollection<Team> Team = new List<Team>();

        /// <summary>
        /// Filter on Iteration this item is in.
        /// </summary>
        public readonly ICollection<Iteration> Iteration = new List<Iteration>();

        /// <summary>
        /// Filter on Theme assigned.
        /// </summary>
        public readonly ICollection<Theme> Theme = new List<Theme>();

        /// <summary>
        /// Filter on Goals.
        /// </summary>
        public readonly ICollection<Goal> Goals = new List<Goal>();

        /// <summary>
        /// Filter on Requests.
        /// </summary>
        public readonly ICollection<Request> Requests = new List<Request>();

        /// <summary>
        /// Filter on Issues.
        /// </summary>
        public readonly ICollection<Issue> Issues = new List<Issue>();

        /// <summary>
        /// Filter on BlockingIssues.
        /// </summary>
        public readonly ICollection<Issue> BlockingIssues = new List<Issue>();

        /// <summary>
        /// Filter on AffectedByDefects.
        /// </summary>
        public readonly ICollection<Defect> AffectedByDefects = new List<Defect>();

        /// <summary>
        /// Filter on Estimate. Must be an exact match. Add a null to the list to include items that have not been estimated.
        /// </summary>
        public readonly NullableDoubleSearcher Estimate = new NullableDoubleSearcher();

        /// <summary>
        /// Filter on Source.
        /// </summary>
        public readonly ICollection<string> Source = new List<string>();

        /// <summary>
        /// Filter on Priority.
        /// </summary>
        public readonly ICollection<string> Priority = new List<string>();

        /// <summary>
        /// Filter on Build Runs
        /// </summary>
		public readonly ICollection<BuildRun> CompletedIn = new List<BuildRun>();

		/// <summary>
		/// The Epic that this Story belongs to.
		/// </summary>
		public readonly ICollection<Epic> Epic = new List<Epic>();


        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Comparison("Estimate", Estimate);

            builder.Relation("Team", Team);
            builder.Relation("Parent", Theme);
			builder.Relation("Timebox", Iteration);

			builder.Relation("Super", Epic);

            builder.ListRelation<WorkitemStatus>("Status", Status);
            builder.ListRelation<WorkitemSource>("Source", Source);
            builder.ListRelation<WorkitemPriority>("Priority", Priority);

            builder.MultiRelation("Goals", Goals);
            builder.MultiRelation("Requests", Requests);
            builder.MultiRelation("Issues", Issues);
            builder.MultiRelation("BlockingIssues", BlockingIssues);
            builder.MultiRelation("AffectedByDefects", AffectedByDefects);
            builder.MultiRelation("CompletedInBuildRuns", CompletedIn);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive
                                     ? new TokenTerm("AssetState='Active';AssetType='Story','Defect','TestSet'")
                                     : new TokenTerm("AssetState='Closed';AssetType='Story','Defect','TestSet'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Story','Defect','TestSet'"));
            }
        }
    }
}