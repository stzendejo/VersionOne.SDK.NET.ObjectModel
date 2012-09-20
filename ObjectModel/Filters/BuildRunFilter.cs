using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    ///<summary>
    /// A filter for Build Runs
    ///</summary>
    public class BuildRunFilter : BaseAssetFilter {
        internal override Type EntityType {
            get { return typeof (BuildRun); }
        }

        /// <summary>
        /// Build Project of the Build Run
        /// </summary>
        public readonly ICollection<BuildProject> BuildProjects = new List<BuildProject>();

        /// <summary>
        /// Reference of the Build Run
        /// </summary>
        public readonly ICollection<string> References = new List<string>();

        /// <summary>
        /// Source of the Build Run
        /// </summary>
        public readonly ICollection<string> Source = new List<string>();

        /// <summary>
        /// Status of the Build Run
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// ChangeSets in the Build Run
        /// </summary>
        public readonly ICollection<ChangeSet> ChangeSets = new List<ChangeSet>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("Reference", References);

            builder.Relation("BuildProject", BuildProjects);
            builder.Relation("ChangeSets", ChangeSets);

            builder.ListRelation<BuildSource>("Source", Source);
            builder.ListRelation<BuildStatus>("Status", Status);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive
                                     ? new TokenTerm("AssetState='Active';AssetType='BuildRun'")
                                     : new TokenTerm("AssetState='Closed';AssetType='BuildRun'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='BuildRun'"));
            }
        }

    }
}