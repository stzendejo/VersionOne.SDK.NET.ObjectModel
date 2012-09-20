using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// Regression Test representation.
    /// </summary>
    [MetaData("RegressionTest")]
    public class RegressionTest : ProjectAsset
    {
        internal RegressionTest(V1Instance instance) : base(instance) { }

        internal RegressionTest(AssetID assetID, V1Instance instance) : base(assetID, instance) { }

        /// <summary>
        /// Source Test used to generate current Regression Test.
        /// </summary>
        public Test GeneratedFrom 
        {
            get { return GetRelation<Test>("GeneratedFrom"); }
            set { SetRelation("GeneratedFrom", value);}
        }

        /// <summary>
        /// Get Tests generated from current Regression Test.
        /// </summary>
        public ICollection<Test> GetGeneratedTests() 
        {
            return GetMultiRelation<Test>("GeneratedTests");
        }

        /// <summary>
        /// Tags defined for current Regression Test.
        /// </summary>
        public string Tags 
        {
            get { return Get<string>("Tags"); }
            set { Set("Tags", value);}
        }

        /// <summary>
        /// Related Regression Suites.
        /// </summary>
        public ICollection<RegressionSuite> RegressionSuites 
        {
            get { return GetMultiRelation<RegressionSuite>("RegressionSuites"); }
        }

        /// <summary>
        /// Regression Test owners.
        /// </summary>
        public ICollection<Member> Owners 
        {
            get { return GetMultiRelation<Member>("Owners"); }
        }

        /// <summary>
        /// Regression Test status.
        /// </summary>
        public IListValueProperty Status 
        {
            get { return GetListValue<RegressionTestStatus>("Status"); }
        }

        /// <summary>
        /// Test category.
        /// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type 
        {
            get { return GetListValue<TestType>("Category"); }
        }

        /// <summary>
        /// Reference value.
        /// </summary>
        public string Reference 
        {
            get { return Get<string>("Reference"); }
            set { Set("Reference", value);}
        }

        /// <summary>
        /// Results that we expect when running the test.
        /// </summary>
        public string ExpectedResults 
        {
            get { return Get<string>("ExpectedResults"); }
            set { Set("ExpectedResults", value); }
        }

        /// <summary>
        /// Regression Test steps description.
        /// </summary>
        public string Steps 
        {
            get { return Get<string>("Steps"); }
            set { Set("Steps", value); }
        }

        /// <summary>
        /// Test Inputs description.
        /// </summary>
        public string Inputs 
        {
            get { return Get<string>("Inputs"); }
            set { Set("Inputs", value); }
        }

        /// <summary>
        /// Test Setup description.
        /// </summary>
        public string Setup 
        {
            get { return Get<string>("Setup"); }
            set { Set("Setup", value); }
        }

        #region Operations

        internal override void CloseImpl() 
        {
            Instance.ExecuteOperation<RegressionTest>(this, "Inactivate");
        }

        internal override void ReactivateImpl() 
        {
            Instance.ExecuteOperation<RegressionTest>(this, "Reactivate");
        }

        #endregion
    }
}