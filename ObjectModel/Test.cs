using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a Test in the VersionOne System
	/// </summary>
	[MetaData("Test",null,"Test.Order")]
	public class Test : SecondaryWorkitem
	{
		internal Test(AssetID id, V1Instance instance) : base(id, instance) { }
		internal Test(V1Instance instance) : base(instance) { }

		/// <summary>
		/// The Type of this Test
		/// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type { get { return GetListValue<TestType>("Category"); } }

		/// <summary>
		/// The Status of this Test
		/// </summary>
		public IListValueProperty Status { get { return GetListValue<TestStatus>("Status"); } }

        /// <summary>
        /// Get collection of Regression Tests generated from Test.
        /// </summary>
        public ICollection<RegressionTest> GetGeneratedRegressionTests() 
        {
            return GetMultiRelation<RegressionTest>("GeneratedRegressionTests");
        }

	    /// <summary>
        /// Regression Test from with Test was generated.
	    /// </summary>
	    public RegressionTest GeneratedFrom 
        {
	        get { return GetRelation<RegressionTest>("GeneratedFrom"); }
            set { SetRelation("GeneratedFrom", value); }
	    }

		/// <summary>
		/// This item's order.
		/// </summary>
        [MetaRenamed("Order")]
        public Rank<Test> RankOrder { get { return GetRank<Test>("Order"); } }

		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Task>(this, "Inactivate");
		}

		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Task>(this, "Reactivate");
		}

        /// <summary>
        /// Generate Regression Test base on the current test.
        /// </summary>
        /// <returns>Generated Regression Test.</returns>
        public RegressionTest GenerateRegressionTest() 
        {
            return Instance.Create.RegressionTest(this);
        }
	}
}