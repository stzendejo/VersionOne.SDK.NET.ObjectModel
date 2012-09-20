using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for ChangeSets
	/// </summary>
	public class ChangeSetFilter : BaseAssetFilter
	{
		internal override Type EntityType
		{
			get { return typeof(ChangeSet); }
		}

		/// <summary>
		/// Cross-reference of the ChangeSet with an external system.
		/// </summary>
		public readonly ICollection<string> Reference = new List<string>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("Reference", Reference);
		}
	}
}
