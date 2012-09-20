using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Regression Suite representation.
    /// </summary>
    [MetaData("RegressionSuite")]
    public class RegressionSuite : BaseAsset {
        internal RegressionSuite(V1Instance instance) : base(instance) {}

        internal RegressionSuite(AssetID assetId, V1Instance instance) : base(assetId, instance) {}

        /// <summary>
        /// ID (or Number) of this entity as displayed in the VersionOne system.
        /// </summary>
        [MetaRenamed("Number")]
        public string DisplayID {
            get { return Get<string>("Number"); }
        }

        /// <summary>
        /// Members that own this item.
        /// </summary>
        public Member Owner {
            get { return GetRelation<Member>("Owner"); }
            set { SetRelation("Owner", value); }
        }

        /// <summary>
        /// Reference is a free text field used for reference (perhaps to an external system).
        /// </summary>
        public string Reference {
            get { return Get<string>("Reference"); }
            set { Set("Reference", value); }
        }

        /// <summary>
        /// Estimate required to implement this item.
        /// </summary>
        public double? Estimate {
            get { return Get<double?>("Estimate"); }
            set { Set("Estimate", value); }
        }

        /// <summary>
        /// Regression Plan associated with this suite.
        /// </summary>
        public RegressionPlan RegressionPlan {
            get { return GetRelation<RegressionPlan>("RegressionPlan"); }
            set { SetRelation("RegressionPlan", value); }
        }

        /// <summary>
        /// Regression Tests associated with this suite.
        /// </summary>
        public ICollection<RegressionTest> RegressionTests {
            get { return GetMultiRelation<RegressionTest>("RegressionTests"); }
        }

        /// <summary>
        /// Assign Regression Test to this suite.
        /// </summary>
        /// <param name="regressionTest">Regression Test to assign.</param>
        public void AssignRegressionTest(RegressionTest regressionTest) {
            RegressionTests.Add(regressionTest);
        }

        /// <summary>
        /// Un-assign Regression Test from this suite.
        /// </summary>
        /// <param name="regressionTest">Regression Test to un-assign.</param>
        public void UnassignRegressionTest(RegressionTest regressionTest) {
            if (!RegressionTests.Contains(regressionTest)) {
                throw new InvalidOperationException("Suite doesn't have this regression test.");
            }

            RegressionTests.Remove(regressionTest);
        }

        internal override bool CanCloseImpl {
            get { return false; }
        }

        internal override bool CanReactivateImpl {
            get { return false; }
        }

        internal override void CloseImpl() {
            throw new InvalidOperationException("Cannot close regression suite.");
        }

        internal override void ReactivateImpl() {
            throw new InvalidOperationException("Cannot reactivate regression suite.");
        }

        /// <summary>
        /// Create new TestSet
        /// </summary>
        /// <param name="name">Test Set name</param>
        /// <returns>Created entity</returns>
        public TestSet CreateTestSet(string name) {
            return Instance.Create.TestSet(name, this, RegressionPlan.Project);
        }

        /// <summary>
        /// Create new TestSet
        /// </summary>
        /// <param name="name">Test Set name</param>
        /// <param name="attributes">Additional attributes for the brand new Test Set</param>
        /// <returns>Created entity</returns>
        public TestSet CreateTestSet(string name, IDictionary<string, object> attributes) {
            return Instance.Create.TestSet(name, this, RegressionPlan.Project, attributes);
        }

        /// <summary>
        /// Request related Test Sets.
        /// </summary>
        /// <param name="filter">Test Set filter</param>
        /// <returns>Related Test Sets</returns>
        public ICollection<TestSet> GetTestSets(TestSetFilter filter) {
            filter = filter ?? new TestSetFilter();
            filter.RegressionSuite.Clear();
            filter.RegressionSuite.Add(this);

            return Instance.Get.TestSets(filter);
        }
    }
}