using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Regression Plan representation.
    /// </summary>
    [MetaData("RegressionPlan")]
    public class RegressionPlan : ProjectAsset 
    {
        internal RegressionPlan(V1Instance instance) : base(instance) { }

        internal RegressionPlan(AssetID assetId, V1Instance instance) : base(assetId, instance) { }

        /// <summary>
        /// Members that own this item.
        /// </summary>
        public Member Owner 
        {
            get { return GetRelation<Member>("Owner"); }
            set { SetRelation("Owner", value); }
        }

        /// <summary>
        /// Cross-reference of this item with an external system.
        /// </summary>
        public string Reference 
        {
            get { return Get<string>("Reference"); } 
            set { Set("Reference", value); }
        }

        internal override bool CanCloseImpl { get { return false; } }
        internal override bool CanReactivateImpl { get { return false; } }

        internal override void CloseImpl() 
        {
            throw new InvalidOperationException("Cannot close regression plan.");
        }

        internal override void ReactivateImpl() 
        {
            throw new InvalidOperationException("Cannot reactivate regression plan.");
        }
        
        ///<summary>
        /// Create new Regression Suite with title assigned to this Regression Plan
        ///</summary>
        ///<param name="name">Title of the suite</param>
        ///<returns>Regression Suite</returns>
        public RegressionSuite CreateRegressionSuite(string name) 
        {
            return Instance.Create.RegressionSuite(name, this, null);
        }

        ///<summary>
        /// Create new Regression Suite with title assigned to this Regression Plan
        ///</summary>
        ///<param name="name">Title of the suite</param>
        ///<param name="attributes">Additional attributes for initialization of Regression Suite.</param>
        ///<returns>Regression Suite</returns>
        public RegressionSuite CreateRegressionSuite(string name, IDictionary<string, object> attributes) 
        {
            return Instance.Create.RegressionSuite(name, this, attributes);
        }

        /// <summary>
        /// Projects associated with this TestSuite.
        /// </summary>
        public ICollection<RegressionSuite> GetRegressionSuites(RegressionSuiteFilter filter) {
            filter = filter ?? new RegressionSuiteFilter();
            filter.RegressionPlan.Clear();
            filter.RegressionPlan.Add(this);
            return Instance.Get.RegressionSuites(filter);
        }
    }
}