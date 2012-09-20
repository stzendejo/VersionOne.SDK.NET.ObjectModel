using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for getting defects.
	/// </summary>
	public class DefectFilter : PrimaryWorkitemFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Defect); }
        }

		/// <summary>
		/// Filter on the Type property.
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();

		/// <summary>
		/// Filter on the FoundBy property.
		/// </summary>
		public readonly ICollection<string> FoundBy = new List<string>();

		/// <summary>
		/// Filter on the FoundInBuild property.
		/// </summary>
		public readonly ICollection<string> FoundInBuild = new List<string>();

		/// <summary>
		/// Filter on the FoundInVersion property.
		/// </summary>
		public readonly ICollection<string> FoundInVersion = new List<string>();

		/// <summary>
		/// Filter on the Environment property.
		/// </summary>
		public readonly ICollection<string> Environment = new List<string>();

		/// <summary>
		/// Filter on the ResolvedInBuild property.
		/// </summary>
		public readonly ICollection<string> ResolvedInBuild = new List<string>();

		/// <summary>
		/// Filter on the ResolutionReason property.
		/// </summary>
		public readonly ICollection<string> ResolutionReason = new List<string>();

		/// <summary>
		/// Filter on the VerifiedBy property.
		/// </summary>
		public readonly ICollection<Member> VerifiedBy = new List<Member>();

		/// <summary>
		/// Filter on stories or defects affected.
		/// </summary>
		public readonly ICollection<PrimaryWorkitem> AffectedPrimaryWorkitems = new List<PrimaryWorkitem>();

		/// <summary>
		/// Filter on build runs.
		/// </summary>
		public readonly ICollection<BuildRun> FoundIn = new List<BuildRun>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("FoundBy",FoundBy);
			builder.Simple("FoundInBuild", FoundInBuild);
			builder.Simple("FoundInVersion", FoundInVersion);
			builder.Simple("Environment", Environment);
			builder.Simple("FixedInBuild", ResolvedInBuild);

			builder.Relation("VerifiedBy", VerifiedBy);

			builder.MultiRelation("AffectedPrimaryWorkitems", AffectedPrimaryWorkitems);
			builder.MultiRelation("FoundInBuildRuns", FoundIn);

			builder.ListRelation<DefectType>("Type", Type);
			builder.ListRelation<DefectResolutionReason>("ResolutionReason", ResolutionReason);
		}

		internal override void InternalModifyState(FilterBuilder builder)
		{
			if (HasState)
				if (HasActive)
					builder.Root.And(new TokenTerm("AssetState='Active';AssetType='Defect'"));
				else
					builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='Defect'"));
			else
				builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Defect'"));
		}
	}
}