using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for Tasks and Tests
	/// </summary>
	public class SecondaryWorkitemFilter : WorkitemFilter
	{
		internal override System.Type EntityType
		{
			get { return typeof(SecondaryWorkitem); }
		}

		/// <summary>
		/// Filter on which Story or Defect this workitem is attached to.
		/// </summary>
		public readonly ICollection<Workitem> Parent = new List<Workitem>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("Parent",Parent);
		}

		internal override void InternalModifyState(FilterBuilder builder)
		{
			if (HasState)
				if (HasActive)
					builder.Root.And(new TokenTerm("AssetState='Active';AssetType='Task','Test'"));
				else
					builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='Task','Test'"));
			else
				builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Task','Test'"));
		}
	}
}
