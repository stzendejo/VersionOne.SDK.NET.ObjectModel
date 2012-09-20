using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A Filter for Requests
	/// </summary>
	public class RequestFilter : ProjectAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Request); }
        }

		/// <summary>
		/// The Member who pwns the Request.
		/// </summary>
		public readonly ICollection<Member> Owner = new List<Member>();

		/// <summary>
		/// The Request's Source
		/// </summary>
		public readonly ICollection<string> Source = new List<string>();
		/// <summary>
		/// The Request's Type
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();
		/// <summary>
		/// The Request's Status
		/// </summary>
		public readonly ICollection<string> Status = new List<string>();
		/// <summary>
		/// The Request's Priority
		/// </summary>
		public readonly ICollection<string> Priority = new List<string>();
		/// <summary>
		/// Name of person or organization originating the Request.
		/// </summary>
		public readonly ICollection<string> RequestedBy = new List<string>();
		/// <summary>
		/// Cross-reference of the Request with an external system.
		/// </summary>
		public readonly ICollection<string> Reference = new List<string>();
		/// <summary>
		/// Reason the Request was resolved.
		/// </summary>
		public readonly ICollection<string> ResolutionReason = new List<string>();
		/// <summary>
		/// Issues related to this Request.
		/// </summary>
		public readonly ICollection<Issue> Issues = new List<Issue>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("RequestedBy", RequestedBy);
			builder.Simple("Reference", Reference);

			builder.Relation("Owner", Owner);

			builder.MultiRelation("Issues", Issues);

			builder.ListRelation<WorkitemSource>("Source", Source);
			builder.ListRelation<RequestType>("Category", Type);
			builder.ListRelation<RequestStatus>("Status", Status);
			builder.ListRelation<RequestPriority>("Priority", Priority);
			builder.ListRelation<RequestResolution>("ResolutionReason", ResolutionReason);
		}
	}
}
