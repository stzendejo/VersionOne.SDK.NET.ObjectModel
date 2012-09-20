using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a Retrospective in the VersionOne System
	/// </summary>
	[MetaData("Retrospective")]
	public class Retrospective : BaseAsset
	{
		internal Retrospective(AssetID id, V1Instance instance) : base(id, instance) { }
		internal Retrospective(V1Instance instance) : base(instance) { }

		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Retrospective>(this, "Inactivate");
		}

		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Retrospective>(this, "Reactivate");
		}

		/// <summary>
		/// The Project this ProjectAsset belongs in. This must be present.
		/// </summary>
		public Project Project { get { return GetRelation<Project>("Scope"); } set { SetRelation("Scope", value); } }

		/// <summary>
		/// A read-only collection of Stories Identified in the Retrospective
		/// </summary>
		public ICollection<Story> GetIdentifiedStories(StoryFilter filter)
		{
			filter = filter ?? new StoryFilter();
			filter.IdentifiedIn.Clear();
			filter.IdentifiedIn.Add(this);
			return Instance.Get.Stories(filter);
		}

		/// <summary>
		/// A collection of Issues Identified in the Retrospective
		/// </summary>
		public ICollection<Issue> GetIssues(IssueFilter filter)
		{
			filter = filter ?? new IssueFilter();
			filter.Retrospective.Clear();
			filter.Retrospective.Add(this);
			return Instance.Get.Issues(filter);
		}

		/// <summary>
		/// The Retrospective Facilitator
		/// </summary>
		public Member FacilitatedBy { get { return GetRelation<Member>("FacilitatedBy"); } set { SetRelation("FacilitatedBy", value); } }

		/// <summary>
		/// The Iteration this Retrospective was conducted for
		/// </summary>
		public Iteration Iteration { get { return GetRelation<Iteration>("Timebox"); } set { SetRelation("Timebox", value); } }

		/// <summary>
		/// The date this Retrospective was conducted.
		/// </summary>
		public DateTime? Date
		{
			get { return Get<DateTime?>("Date"); } 
			set
			{
				if (value.HasValue)
					Set("Date", value.Value.Date);
				else
					Set("Date", value);
			}
		}

		/// <summary>
		/// The Team this Retrospecive belongs to
		/// </summary>
		public Team Team { get { return GetRelation<Team>("Team"); } set { SetRelation("Team", value); } }

		/// <summary>
		/// The Retrospective Summary
		/// </summary>
		public string Summary { get { return Get<string>("Summary"); } set { Set("Summary", value); } }

		/// <summary>
		/// Creates a Story related to this Retrospective
		/// </summary>
		/// <param name="name">The name of the Story</param>
		/// <returns>The newly saved Story</returns>
		public Story CreateStory(string name)
		{
			return Instance.Create.Story(name, this);
		}

        /// <summary>
        /// Creates a Story related to this Retrospective
        /// </summary>
        /// <param name="name">The name of the Story</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>The newly saved Story</returns>
        public Story CreateStory(string name, IDictionary<string, object> attributes)
        {
            return Instance.Create.Story(name, this, attributes);
        }

		/// <summary>
		/// Creates an Issue related to this Retrospective
		/// </summary>
		/// <param name="name">The name of the Issue</param>
		/// <returns>The newly saved Issue</returns>
		public Issue CreateIssue(string name)
		{
			return Instance.Create.Issue(name, this);
		}

        /// <summary>
        /// Creates an Issue related to this Retrospective
        /// </summary>
        /// <param name="name">The name of the Issue</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>The newly saved Issue</returns>
        public Issue CreateIssue(string name, IDictionary<string, object> attributes)
        {
            return Instance.Create.Issue(name, this, attributes);
        }
	}
}
