using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// The TestSuite asset.
	/// </summary>
	[MetaData("TestSuite")]
	public class TestSuite : BaseAsset
	{
		internal TestSuite(V1Instance instance) : base(instance) {}
		internal TestSuite(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// Reference is a free text field used for reference (perhaps to an external system).
		/// </summary>
		public string Reference { get { return Get<string>("Reference"); } set { Set("Reference", value); } }

		/// <summary>
		/// Projects associated with this TestSuite.
		/// </summary>
		public ICollection<Project> GetProjects(ProjectFilter filter)
		{
			filter = filter ?? new ProjectFilter();
			filter.TestSuite.Clear();
			filter.TestSuite.Add(this);
			return Instance.Get.Projects(filter);
		}

		/// <summary>
		/// Inactivates the TestSuite
		/// </summary>
		/// <exception cref="InvalidOperationException">The TestSuite is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<TestSuite>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the TestSuite
		/// </summary>
		/// <exception cref="InvalidOperationException">The Theme is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<TestSuite>(this, "Reactivate");
		}
	}
}
