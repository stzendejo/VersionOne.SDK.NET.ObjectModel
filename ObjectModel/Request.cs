using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a request in the VersionOne system.
	/// </summary>
	[MetaData("Request")]
	public class Request : ProjectAsset
	{
		internal Request(V1Instance instance) : base(instance) { }
		internal Request(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// The Member who pwns this Request.
		/// </summary>
		public Member Owner { get { return GetRelation<Member>("Owner"); } set { SetRelation("Owner", value); } }

		/// <summary>
		/// This Request's rank order among all Requests.
		/// </summary>
		[MetaRenamed("Order")]
		public Rank<Request> RankOrder { get { return GetRank<Request>("Order"); } }

		/// <summary>
		/// This Request's Source
		/// </summary>
		public IListValueProperty Source { get { return GetListValue<WorkitemSource>("Source"); } }

		/// <summary>
		/// This Request's Type
		/// </summary>
		public IListValueProperty Type { get { return GetListValue<RequestType>("Category"); } }

		/// <summary>
		/// This Request's Status
		/// </summary>
		public IListValueProperty Status { get { return GetListValue<RequestStatus>("Status"); } }

		/// <summary>
		/// This Request's Priority
		/// </summary>
		public IListValueProperty Priority { get { return GetListValue<RequestPriority>("Priority"); } }

		/// <summary>
		/// Name of person or organization originating this Request.
		/// </summary>
		public string RequestedBy { get { return Get<string>("RequestedBy"); } set { Set("RequestedBy", value); } }

		/// <summary>
		/// Cross-reference of this Request with an external system.
		/// </summary>
		public string Reference { get { return Get<string>("Reference"); } set { Set("Reference", value); } }

		/// <summary>
		/// Reason this Request was resolved.
		/// </summary>
		public IListValueProperty ResolutionReason { get { return GetListValue<RequestResolution>("ResolutionReason"); } }

		/// <summary>
		/// Text field for the description of how this Request was resolved.
		/// </summary>
		public string ResolutionDetails { get { return Get<string>("Resolution"); } set { Set("Resolution", value); } }

		/// <summary>
		/// Stories and Defects associated with this Request.
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Requests.Clear();
			filter.Requests.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Issues associated with this Request.
		/// </summary>
		public ICollection<Issue> Issues { get { return GetMultiRelation<Issue>("Issues"); } }

		/// <summary>
		/// Epics associated with this Request.
		/// </summary>
		public ICollection<Epic> GetEpics(EpicFilter filter)
		{
			filter = filter ?? new EpicFilter();
			filter.Requests.Clear();
			filter.Requests.Add(this);
			return Instance.Get.Epics(filter);
		}

		/// <summary>
		/// Inactivates the Request
		/// </summary>
		/// <exception cref="InvalidOperationException">The Request is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Request>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the Request
		/// </summary>
		/// <exception cref="InvalidOperationException">The Request is an invalid state for the Operation, e.g. it is already active.</exception>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Request>(this, "Reactivate");
		}

		/// <summary>
		/// Creates a Story from this Request.
		/// </summary>
        /// <returns>A Story in the VersionOne system related to this Request.</returns>
		public Story GenerateStory()
		{
            return GenerateStory(null);
		}

        /// <summary>
        /// Creates a Story from this Request.
        /// </summary>
        /// <param name="attributes">required attributes</param>
        /// <returns>A Story in the VersionOne system related to this Request.</returns>
        public Story GenerateStory(IDictionary<string, object> attributes)
        {
            Story story = Instance.New<Story>(this);

            Instance.Create.AddAttributes(story, attributes);

            story.Save();
            return story;
        }

		/// <summary>
		/// Creates a Defect from this Request.
		/// </summary>
        /// <returns>A Defect in the VersionOne system related to this Request.</returns>
		public Defect GenerateDefect()
		{
            return GenerateDefect(null);
		}

        /// <summary>
        /// Creates a Defect from this Request.
        /// </summary>
        /// <param name="attributes">required attributes</param>
        /// <returns>A Defect in the VersionOne system related to this Request.</returns>
        public Defect GenerateDefect(IDictionary<string, object> attributes)
        {
            Defect defect = Instance.New<Defect>(this);

            Instance.Create.AddAttributes(defect, attributes);

            defect.Save();
            return defect;
        }
	}
}
