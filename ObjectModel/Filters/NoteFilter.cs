using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for Notes
	/// </summary>
	public class NoteFilter : EntityFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Note); }
        }

		/// <summary>
		/// Item name. Must be complete match.
		/// </summary>
		public readonly ICollection<string> Name = new List<string>();

		/// <summary>
		/// Item Content. Must be complete match.
		/// </summary>
		public readonly ICollection<string> Content = new List<string>();

		/// <summary>
		/// The Note's Type.  Must be complete match.
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();

		/// <summary>
		/// The Note's Related Asset.
		/// </summary>
		public readonly ICollection<BaseAsset> Asset = new List<BaseAsset>();

		/// <summary>
		/// Note this Note is a response to.
		/// </summary>
		public readonly ICollection<Note> InResponseTo = new List<Note>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("PersonalTo", new Member[] { null, builder.Instance.LoggedInMember });

			builder.Simple("Name", Name);
			builder.Simple("Content", Content);

			builder.Relation("Asset", Asset);
			builder.Relation("InResponseTo", InResponseTo);

			builder.ListRelation<NoteType>("Category", Type);
		}
	}
}
