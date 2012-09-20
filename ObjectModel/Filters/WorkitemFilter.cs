using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for Workitems
    /// </summary>
    public class WorkitemFilter : ProjectAssetFilter {
        internal override System.Type EntityType {
            get { return typeof (Workitem); }
        }

        /// <summary>
        /// Item owner(s).
        /// </summary>
        public readonly ICollection<Member> Owners = new List<Member>();

        /// <summary>
        /// Filter on Reference.
        /// </summary>
        public readonly ICollection<string> Reference = new List<string>();

        /// <summary>
        /// Filter on DetailEstimate. Must be an exact match. Add a null to the list to include items that have not been estimated.
        /// </summary>
        public readonly NullableDoubleSearcher DetailEstimate = new NullableDoubleSearcher();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.MultiRelation("Owners", Owners);
            builder.Simple("Reference", Reference);

            builder.Comparison("DetailEstimate", DetailEstimate);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive
                                     ? new TokenTerm("AssetState='Active';AssetType!='Theme'")
                                     : new TokenTerm("AssetState='Closed';AssetType!='Theme'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType!='Theme'"));
            }
        }
    }
}