namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// TestSet representation.
    /// </summary>
    [MetaData("TestSet")]
    public class TestSet : PrimaryWorkitem 
    {
        internal TestSet(AssetID assetId, V1Instance instance) : base(assetId, instance) { }
        
        internal TestSet(V1Instance instance) : base(instance) { }
        
        /// <summary>
        /// Corresponding Regression Suite
        /// </summary>
        public RegressionSuite RegressionSuite 
        {
            get { return GetRelation<RegressionSuite>("RegressionSuite"); }
            set { SetRelation("RegressionSuite", value);}
        } 

        /// <summary>
        /// Environment for this Test Set
        /// </summary>
        public Environment Environment 
        {
            get { return GetRelation<Environment>("Environment"); }
            set { SetRelation("Environment", value);}
        }

        /// <summary>
        /// Check whether acceptance tests can be copied from related Regression Suite
        /// </summary>
        public bool CanCopyAcceptanceTestsFromRegressionSuite 
        {
            get 
            {
                return Instance.CanExecuteOperation(this, "CopyAcceptanceTestsFromRegressionSuite");
            }
        }

        /// <summary>
        /// Copy tests from related Regression Suite.
        /// </summary>
        public void CopyAcceptanceTestsFromRegressionSuite() 
        {
            Instance.ExecuteOperation<TestSet>(this, "CopyAcceptanceTestsFromRegressionSuite");
        }
        
        internal override void CloseImpl() 
        {
            Instance.ExecuteOperation<TestSet>(this, "Inactivate");
        }

        internal override void ReactivateImpl() 
        {
            Instance.ExecuteOperation<TestSet>(this, "Reactivate");
        }
    }
}