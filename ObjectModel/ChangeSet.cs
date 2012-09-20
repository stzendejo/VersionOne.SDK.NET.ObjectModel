using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a ChangeSet in the VersionOne system
	/// </summary>
	[MetaData("ChangeSet")]
	public class ChangeSet : BaseAsset
	{
		internal ChangeSet(AssetID id, V1Instance instance) : base(id, instance) { }
		internal ChangeSet(V1Instance instance) : base(instance) { }

		/// <summary>
		/// Reference of this ChangeSet
		/// </summary>
		public string Reference { get { return Get<string>("Reference"); } set { Set("Reference", value); } }

		/// <summary>
		/// Primary workitems affected by this ChangeSet.
		/// </summary>
		public ICollection<PrimaryWorkitem> PrimaryWorkitems { get { return GetMultiRelation<PrimaryWorkitem>("PrimaryWorkitems"); } }

		/// <summary>
		/// A collection of Build Runs associated with this ChangeSet
		/// </summary>
		public ICollection<BuildRun> GetBuildRuns(BuildRunFilter filter)
		{
			filter = filter ?? new BuildRunFilter();
			filter.ChangeSets.Clear();
			filter.ChangeSets.Add(this);
			return Instance.Get.BuildRuns(filter);
		}

		/// <summary>
		/// Closes the ChangeSet
		/// </summary>
		/// <exception cref="InvalidOperationException">The ChangeSet is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			throw new NotSupportedException("ChangeSets cannot be closed in VersionOne.");
		}

		/// <summary>
		/// Reopens the ChangeSet
		/// </summary>
		/// <exception cref="InvalidOperationException">The ChangeSet is an invalid state for the Operation, e.g. it is already active.</exception>
		internal override void ReactivateImpl()
		{
			throw new NotSupportedException("ChangeSets cannot be closed in VersionOne.");
		}

		internal override bool CanCloseImpl
		{
			get
			{
				return false;
			}
		}

	}
}
