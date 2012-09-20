using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// A filter for Regression Suites
    /// </summary>
    public class RegressionSuiteFilter : BaseAssetFilter {
        internal override Type EntityType {
            get { return typeof (RegressionSuite); }
        }

        /// <summary>
        /// DisplayID of this item.
        /// </summary>
        public readonly ICollection<string> DisplayID = new List<string>();

        /// <summary>
        /// Item owner(s).
        /// </summary>
        public readonly ICollection<Member> Owners = new List<Member>();

        /// <summary>
        /// Filter on Reference.
        /// </summary>
        public readonly ICollection<string> Reference = new List<string>();

        /// <summary>
        /// Filter on Estimate.
        /// </summary>
        public readonly NullableDoubleSearcher Estimate = new NullableDoubleSearcher();

        /// <summary>
        /// RegressionPlan associate with suite.
        /// </summary>
        public readonly ICollection<RegressionPlan> RegressionPlan = new List<RegressionPlan>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("Number", DisplayID);
            builder.Simple("Reference", Reference);
            
            builder.Comparison("Estimate", Estimate);

            builder.Relation("Owner", Owners);
            builder.Relation("RegressionPlan", RegressionPlan);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive
                                     ? new TokenTerm("AssetState='Active';AssetType='RegressionSuite'")
                                     : new TokenTerm("AssetState='Closed';AssetType='RegressionSuite'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='RegressionSuite'"));
            }
        }
    }
}