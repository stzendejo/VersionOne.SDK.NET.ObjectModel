using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters 
{
    /// <summary>
    /// A filter for Regression Plans
    /// </summary>
    public class RegressionPlanFilter : ProjectAssetFilter 
    {
        internal override Type EntityType 
        {
            get { return typeof(RegressionPlan); }
        }

        /// <summary>
        /// Item owner(s).
        /// </summary>
        public readonly ICollection<Member> Owners = new List<Member>();

        /// <summary>
        /// Filter on Reference.
        /// </summary>
        public readonly ICollection<string> Reference = new List<string>();

        internal override void InternalModifyFilter(FilterBuilder builder) 
        {
            base.InternalModifyFilter(builder);

            builder.Relation("Owner", Owners);
            builder.Simple("Reference", Reference);
        }

        internal override void InternalModifyState(FilterBuilder builder) 
        {
            if (HasState) 
            {
                if (HasActive) 
                {
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='RegressionPlan'"));
                } 
                else 
                {
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='RegressionPlan'"));
                }
            } 
            else 
            {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='RegressionPlan'"));
            }
        }
    }
}