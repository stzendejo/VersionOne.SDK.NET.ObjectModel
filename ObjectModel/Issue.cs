using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents an issue in the VersionOne system.
	/// </summary>
	[MetaData("Issue")]
	public class Issue : ProjectAsset
	{
		internal Issue(V1Instance instance) : base(instance) { }
		internal Issue(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// This Issue rank order among all Issues.
		/// </summary>
		[MetaRenamed("Order")]
		public Rank<Issue> RankOrder { get { return GetRank<Issue>("Order"); } }

		/// <summary>
		/// Stories and Defects associated with this Issue.
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Issues.Clear();
			filter.Issues.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Requests associated with this Issue.
		/// </summary>
		public ICollection<Request> GetRequests(RequestFilter filter)
		{
			filter = filter ?? new RequestFilter();
			filter.Issues.Clear();
			filter.Issues.Add(this);
			return Instance.Get.Requests(filter);
		}

		/// <summary>
		/// Stories and Defects that cannot be completed because of this Issue.
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetBlockedPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Issues.Clear();
			filter.Issues.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Epics  associated with this Issue.
		/// </summary>
        /// <param name="filter">Criteria to filter epics on.</param>
        /// <returns> A collection epics that belong to this issue filtered by the 
        /// passed in filter.</returns>
		public ICollection<Epic> GetEpics(EpicFilter filter)
		{
			filter = filter ?? new EpicFilter();
			filter.Issues.Clear();
			filter.Issues.Add(this);
			return Instance.Get.Epics(filter);
		}

        /// <summary>
        /// Epics that cannot be completed because of this Issue.
        /// </summary>
        /// <param name="filter">Criteria to filter epics on.</param>
        /// <returns> A collection epics cannot be completed because of 
        /// this Issue filtered by the passed in filter.</returns>
        public ICollection<Epic> GetBlockedEpics(EpicFilter filter) {
            filter = filter ?? new EpicFilter();

            filter.BlockingIssues.Clear();
            filter.BlockingIssues.Add(this);
            return Instance.Get.Epics(filter);
        }

		/// <summary>
		/// This Issue's Source
		/// </summary>
		public IListValueProperty Source { get { return GetListValue<WorkitemSource>("Source"); } }

		/// <summary>
		/// This Issue's Type
		/// </summary>
		public IListValueProperty Type { get { return GetListValue<IssueType>("Category"); } }

		/// <summary>
		/// This Issue's Priority
		/// </summary>
		public IListValueProperty Priority { get { return GetListValue<IssuePriority>("Priority"); } }

		/// <summary>
		/// Reason this Issue was resolved.
		/// </summary>
		public IListValueProperty ResolutionReason { get { return GetListValue<IssueResolutionReason>("ResolutionReason"); } }

		/// <summary>
		/// Text field for the description of how this Request was resolved.
		/// </summary>
		public string ResolutionDetails { get { return Get<string>("Resolution"); } set { Set("Resolution", value); } }

		/// <summary>
		/// Name of person or organization originating this Issue.
		/// </summary>
		public string IdentifiedBy { get { return Get<string>("IdentifiedBy"); } set { Set("IdentifiedBy", value); } }

		/// <summary>
		/// Cross-reference of this Issue with an external system.
		/// </summary>
		public string Reference { get { return Get<string>("Reference"); } set { Set("Reference", value); } }

		/// <summary>
		/// Date this Issue brings the system down to a screeching halt
		/// </summary>
		public DateTime? TargetDate
		{
			get { return Get<DateTime?>("EndDate"); }
			set
			{
				if (value.HasValue)
					Set("EndDate", value.Value.Date);
				else
					Set("EndDate", value);
			}
		}

		/// <summary>
		/// The Team this Issue is assigned to.
		/// </summary>
		public Team Team { get { return GetRelation<Team>("Team"); } set { SetRelation("Team", value); } }

		/// <summary>
		/// The Member who pwns this Issue.
		/// </summary>
		public Member Owner { get { return GetRelation<Member>("Owner"); } set { SetRelation("Owner", value); } }

		/// <summary>
		/// The Retrospectives related to this Issue
		/// </summary>
		public ICollection<Retrospective> Retrospectives { get { return GetMultiRelation<Retrospective>("Retrospectives"); } }

		/// <summary>
		/// Inactivates the Issue
		/// </summary>
		/// <exception cref="InvalidOperationException">The Issue is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Issue>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the Issue
		/// </summary>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Issue>(this, "Reactivate");
		}

		/// <summary>
		/// Creates a Story from this Issue.
		/// </summary>
		/// <returns>A Story in the VersionOne system related to this Issue.</returns>
		public Story GenerateStory()
		{
			return GenerateStory(null);
		}

        /// <summary>
        /// Creates a Story from this Issue.
        /// </summary>
        /// <param name="attributes">required attributes</param>
        /// <returns>A Story in the VersionOne system related to this Issue.</returns>
        public Story GenerateStory(IDictionary<string, object> attributes)
        {
            Story story = Instance.New<Story>(this);

            Instance.Create.AddAttributes(story, attributes);

            story.Save();
            return story;
        }

		/// <summary>
		/// Creates a Defect from this Issue.
		/// </summary>
		/// <returns>A Defect in the VersionOne system related to this Issue.</returns>
		public Defect GenerateDefect()
		{
            return GenerateDefect(null);
		}

        /// <summary>
        /// Creates a Defect from this Issue.
        /// </summary>
        /// <param name="attributes">required attributes</param>
        /// <returns>A Defect in the VersionOne system related to this Issue.</returns>
        public Defect GenerateDefect(IDictionary<string, object> attributes)
        {
            Defect defect = Instance.New<Defect>(this);

            Instance.Create.AddAttributes(defect, attributes);

            defect.Save();
            return defect;
        }
	}
}
