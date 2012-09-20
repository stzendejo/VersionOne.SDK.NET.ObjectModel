using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// A Defect.
	/// </summary>
	[MetaData("Defect",null,"Defect.Order")]
	public class Defect : PrimaryWorkitem
	{
		internal Defect(V1Instance instance) : base(instance) { }
		internal Defect(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// Free text field indicating environment this Defect was identified in.
		/// </summary>
		public string Environment { get { return Get<string>("Environment"); } set { Set("Environment", value); } }

		/// <summary>
		/// Free text field indicating who found the defect.
		/// </summary>
		public string FoundBy { get { return Get<string>("FoundBy"); } set { Set("FoundBy", value); } }

		/// <summary>
		/// Build number where this Defect was found.
		/// </summary>
		public string FoundInBuild { get { return Get<string>("FoundInBuild"); } set { Set("FoundInBuild", value); } }

		/// <summary>
		/// Version where this Defect was found.
		/// </summary>
        [MetaRenamed("VersionAffected")]
		public string FoundInVersion { get { return Get<string>("VersionAffected"); } set { Set("VersionAffected", value); } }

		/// <summary>
		/// Build number where this Defect was resolved.
		/// </summary>
        [MetaRenamed("FixedInBuild")]
		public string ResolvedInBuild { get { return Get<string>("FixedInBuild"); } set { Set("FixedInBuild", value); } }

		/// <summary>
		/// Reason this Defect was resolved.
		/// </summary>
		public IListValueProperty ResolutionReason { get { return GetListValue<DefectResolutionReason>("ResolutionReason"); } }

		/// <summary>
		/// Text field for the description of how this Defect was resolved.
		/// </summary>
        [MetaRenamed("Resolution")]
		public string ResolutionDetails { get { return Get<string>("Resolution"); } set { Set("Resolution", value); } }

		/// <summary>
		/// The Member this defect was verified by.
		/// </summary>
		public Member VerifiedBy { get { return GetRelation<Member>("VerifiedBy"); } set { SetRelation("VerifiedBy", value); } }

		/// <summary>
		/// This Defect's Type
		/// </summary>
		public IListValueProperty Type { get { return GetListValue<DefectType>("Type"); } }

		/// <summary>
		/// Build Run's this Defect was found in
		/// </summary>
		[MetaRenamed("FoundInBuildRuns")]
		public ICollection<BuildRun> FoundIn { get { return GetMultiRelation<BuildRun>("FoundInBuildRuns"); } }

		/// <summary>
		/// Primary workitems that are affected by this defect
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetAffectedPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.AffectedByDefects.Clear();
			filter.AffectedByDefects.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Inactivates the Defect
		/// </summary>
		/// <exception cref="InvalidOperationException">The Defect is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Defect>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the Defect
		/// </summary>
		/// <exception cref="InvalidOperationException">The Defect is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Story>(this, "Reactivate");
		}
	}
}
