using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A Filter for Issues
	/// </summary>
	public class IssueFilter : ProjectAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Issue); }
        }

		/// <summary>
		/// The Issue's Type
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();
		/// <summary>
		/// The Issue's Priority
		/// </summary>
		public readonly ICollection<string> Priority = new List<string>();

		/// <summary>
		/// Reason this Issue was resolved.
		/// </summary>
		public readonly ICollection<string> ResolutionReason = new List<string>();

		/// <summary>
		/// Members that own this issue
		/// </summary>
		public readonly ICollection<Member> Owner = new List<Member>();

		/// <summary>
		/// Teams that own this issue
		/// </summary>
		public readonly ICollection<Team> Team = new List<Team>();

		/// <summary>
		/// Retrospectives related to this issue
		/// </summary>
		public readonly ICollection<Retrospective> Retrospective = new List<Retrospective>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("Owner", Owner);
			builder.Relation("Team", Team);

			builder.MultiRelation("Retrospectives", Retrospective);

			builder.ListRelation<IssueType>("Category", Type);
			builder.ListRelation<IssuePriority>("Priority", Priority);
			builder.ListRelation<IssueResolutionReason>("ResolutionReason", ResolutionReason);
		}
	}
}
