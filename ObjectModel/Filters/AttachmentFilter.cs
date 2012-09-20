using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for Attachments
	/// </summary>
	public class AttachmentFilter : EntityFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Attachment); }
        }

		/// <summary>
		/// Item name. Must be complete match.
		/// </summary>
		public readonly ICollection<string> Name = new List<string>();

		/// <summary>
		/// Item description. Must be complete match.
		/// </summary>
		public readonly ICollection<string> Description = new List<string>();

		/// <summary>
		/// The Attachment's Type.  Must be complete match.
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();

		/// <summary>
		/// The Attachment's Related Asset.
		/// </summary>
		public readonly ICollection<BaseAsset> Asset = new List<BaseAsset>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("Name", Name);
			builder.Simple("Description", Description);

			builder.Relation("Asset", Asset);

			builder.ListRelation<AttachmentType>("Category", Type);
		}
	}
}
