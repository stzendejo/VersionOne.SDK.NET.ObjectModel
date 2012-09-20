using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters 
{
    /// <summary>
    /// Filter for Test Set retrieval.
    /// </summary>
    public class TestSetFilter : PrimaryWorkitemFilter 
    {
        internal override Type EntityType
        {
            get { return typeof(TestSet); }
        }

        /// <summary>
        /// Filter on related Regression suites
        /// </summary>
        public readonly ICollection<RegressionSuite> RegressionSuite = new List<RegressionSuite>();

        /// <summary>
        /// Filter by Environment
        /// </summary>
        public readonly ICollection<Environment> Environment = new List<Environment>();

        internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("RegressionSuite", RegressionSuite);
			builder.Relation("Environment", Environment);
		}

		internal override void InternalModifyState(FilterBuilder builder)
		{
            if (HasState) 
            {
                if (HasActive) 
                {
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='TestSet'"));
                } 
                else 
                {
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='TestSet'"));
                }
            } 
            else 
            {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='TestSet'"));
            }
		}
    }
}