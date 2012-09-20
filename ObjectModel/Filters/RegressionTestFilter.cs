using System;
using System.Collections.Generic;

using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
    /// <summary>
    /// A filter for Regression Tests.
    /// </summary>
    public class RegressionTestFilter : ProjectAssetFilter
    {
        internal override Type EntityType 
        {
            get { return typeof(RegressionTest); }
        }

        /// <summary>
        /// Test which was used to generate this item.
        /// </summary>
        public readonly ICollection<Test> GeneratedFrom = new List<Test>();

        /// <summary>
        /// Tests generated from this item.
        /// </summary>
        public readonly ICollection<Test> GeneratedTests = new List<Test>();        
        
        /// <summary>
        /// Tags of this item.
        /// </summary>
        public readonly ICollection<string> Tags = new List<string>();

        /// <summary>
        /// Related Regression Suites.
        /// </summary>
        public readonly ICollection<RegressionSuite> RegressionSuites = new List<RegressionSuite>();

        /// <summary>
        /// Item owner(s).
        /// </summary>
        public readonly ICollection<Member> Owners = new List<Member>();

        /// <summary>
        /// Item status.
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// Item category.
        /// </summary>
        public readonly ICollection<string> Type = new List<string>();

        /// <summary>
        /// Reference of this item.
        /// </summary>
        public readonly ICollection<string> Reference = new List<string>();

        internal override void InternalModifyFilter(FilterBuilder builder)
        {
            base.InternalModifyFilter(builder);

            builder.Simple("Tags", Tags);
            builder.Simple("Reference", Reference);            

            builder.Relation("GeneratedFrom", GeneratedFrom);

            builder.MultiRelation("RegressionSuites", RegressionSuites);
            builder.MultiRelation("Owners", Owners);
            builder.MultiRelation("GeneratedTests", GeneratedTests);

            builder.ListRelation<RegressionTestStatus>("Status", Status);
            builder.ListRelation<TestType>("Category", Type);
        }


        internal override void InternalModifyState(FilterBuilder builder)
        {
            if (HasState) 
            {
                if (HasActive) 
                {
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='RegressionTest'"));
                } 
                else 
                {
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='RegressionTest'"));
                }
            } 
            else 
            {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='RegressionTest'"));
            }
        }
    }
}