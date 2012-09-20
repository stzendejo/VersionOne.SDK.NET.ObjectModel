using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for Links
	/// </summary>
	public class LinkFilter : EntityFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Link); }
        }

		/// <summary>
		/// Item name. Must be complete match.
		/// </summary>
		public readonly ICollection<string> Name = new List<string>();

		/// <summary>
		/// Item URL. Must be complete match.
		/// </summary>
		public readonly ICollection<string> URL = new List<string>();

		/// <summary>
		/// The Link's Related Asset.
		/// </summary>
		public readonly ICollection<BaseAsset> Asset = new List<BaseAsset>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("Name", Name);
			builder.Simple("URL", URL);

			builder.Relation("Asset", Asset);
		}
	}
}
