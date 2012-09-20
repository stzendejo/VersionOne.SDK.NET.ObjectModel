using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents an Epic in the VersionOne system.
    /// </summary>
    [MetaData("Epic")]
    public class Epic : Workitem {
        internal Epic(V1Instance instance) : base(instance) {}
        internal Epic(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// The Parent Epic that this Epic belongs to.
        /// </summary>
        public Epic Super {
            get { return GetRelation<Epic>("Super"); }
            set { SetRelation("Super", value); }
        }

        /// <summary>
        /// The Workitem this Epic is assigned to.
        /// </summary>
        public Workitem Parent {
            get { return GetRelation<Workitem>("Parent"); }
        }

        /// <summary>
        /// Swag for the Epic.
        /// </summary>
        public double? Swag {
            get { return Get<double?>("Swag"); }
            set { Set("Swag", value); }
        }

        /// <summary>
        /// Value for the Epic.
        /// </summary>
        public double? Value {
            get { return Get<double?>("Value"); }
            set { Set("Value", value); }
        }

        /// <summary>
        /// This Epic's Status
        /// </summary>
        public IListValueProperty Status {
            get { return GetListValue<EpicStatus>("Status"); }
        }

        /// <summary>
        /// This Epic's Priority
        /// </summary>
        public IListValueProperty Priority {
            get { return GetListValue<EpicPriority>("Priority"); }
        }

        /// <summary>
        /// This Epic's Source
        /// </summary>
        public IListValueProperty Source {
            get { return GetListValue<WorkitemSource>("Source"); }
        }

        /// <summary>
        /// This Epic's Risk
        /// </summary>
        public double? Risk {
            get { return Get<double?>("Risk"); }
            set { Set("Risk", value); }
        }

        /// <summary>
        /// This Epic's Type.
        /// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type {
            get { return GetListValue<EpicType>("Category"); }
        }

        /// <summary>
        /// This item's order.
        /// </summary>
        [MetaRenamed("Order")]
        public Rank<Epic> RankOrder {
            get { return GetRank<Epic>("Order"); }
        }

        /// <summary>
        /// Epics that are immediate children of this Epic
        /// </summary>
        public ICollection<Epic> GetChildEpics(EpicFilter filter) {
            filter = filter ?? new EpicFilter();
            filter.Parent.Clear();
            filter.Parent.Add(this);
            return Instance.Get.Epics(filter);
        }

        /// <summary>
        /// Stories that are immediate children of this Epic
        /// </summary>
        public ICollection<Story> GetChildStories(StoryFilter filter) {
            filter = filter ?? new StoryFilter();
            filter.Epic.Clear();
            filter.Epic.Add(this);
            return Instance.Get.Stories(filter);
        }

        /// <summary>
        /// Tests that are immediate children of this Epic
        /// </summary>
        public ICollection<Test> GetChildTests(TestFilter filter) {
            filter = filter ?? new TestFilter();
            filter.Epic.Clear();
            filter.Epic.Add(this);
            return Instance.Get.Tests(filter);
        }

		/// <summary>
		/// PrimaryWorkiems that are immediate children of this Epic
		/// </summary>
		public ICollection<PrimaryWorkitem> GetChildPrimaryWorkiems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Epic.Clear();
			filter.Epic.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Defects that are immediate children of this Epic
		/// </summary>
		public ICollection<Defect> GetChildDefects(DefectFilter filter)
		{
			filter = filter ?? new DefectFilter();
			filter.Epic.Clear();
			filter.Epic.Add(this);
			return Instance.Get.Defects(filter);
		}

		/// <summary>
		/// TestSets that are immediate children of this Epic
		/// </summary>
		public ICollection<TestSet> GetTestSets(TestSetFilter filter)
		{
			filter = filter ?? new TestSetFilter();
			filter.Epic.Clear();
			filter.Epic.Add(this);
			return Instance.Get.TestSets(filter);
		}

        /// <summary>
        /// Goals this Epic is assigned to.
        /// </summary>
        public ICollection<Goal> Goals {
            get { return GetMultiRelation<Goal>("Goals"); }
        }

        /// <summary>
        /// Requests associated with this Epic.
        /// </summary>
        public ICollection<Request> Requests {
            get { return GetMultiRelation<Request>("Requests"); }
        }

        /// <summary>
        /// Issues associated with this Epic.
        /// </summary>
        public ICollection<Issue> Issues {
            get { return GetMultiRelation<Issue>("Issues"); }
        }

        /// <summary>
        /// Blocking Issues associated with this Epic.
        /// </summary>
        public ICollection<Issue> BlockingIssues {
            get { return GetMultiRelation<Issue>("BlockingIssues"); }
        }

        /// <summary>
        /// Name of person or organization requesting this Epic.
        /// </summary>
        public string RequestedBy {
            get { return Get<string>("RequestedBy"); }
            set { Set("RequestedBy", value); }
        }

        /// <summary>
        /// Inactivates the Story
        /// </summary>
        /// <exception cref="InvalidOperationException">The Epic is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl() {
            Instance.ExecuteOperation<Story>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Story
        /// </summary>
        /// <exception cref="InvalidOperationException">The Epic is an invalid state for the Operation, e.g. it is already active.</exception>
        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Story>(this, "Reactivate");
        }

        /// <summary>
        /// True if a Story can be generated from this Epic
        /// </summary>
        public bool CanGenerateChildStory {
            get { return Get<bool>("CheckGenerateSubStory"); }
        }

        /// <summary>
        /// Generates a Story that is the child of this Epic
        /// </summary>
        /// <returns>A Story that is a child of this Epic</returns>
        /// <exception cref="InvalidOperationException">If the Epic is not able to Generate a child Story</exception>
        public Story GenerateChildStory() {
            Save();
            return Instance.ExecuteOperation<Story>(this, "GenerateSubStory");
        }


        /// <summary>
        /// True if an Epic can be generated from this Epic.
        /// </summary>
        public bool CanGenerateChildEpic {
            get { return Get<bool>("CheckGenerateSubEpic"); }
        }

        /// <summary>
        /// Generates an Epic that is the child of this Epic
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Epic is not able to Generate a child Epic</exception>
        public Epic GenerateChildEpic() {
            Save();
            return Instance.ExecuteOperation<Epic>(this, "GenerateSubEpic");
        }
    }
}