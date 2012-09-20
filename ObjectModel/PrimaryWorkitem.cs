using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Base class for Stories and Defects.
    /// </summary>
    [MetaData("PrimaryWorkitem", null, "PrimaryWorkitem.Order")]
    public abstract class PrimaryWorkitem : Workitem {
        internal PrimaryWorkitem(V1Instance instance) : base(instance) { }

        internal PrimaryWorkitem(AssetID id, V1Instance instance) : base(id, instance) { }

        /// <summary>
        /// The Team this item is assigned to.
        /// </summary>
        public Team Team {
            get { return GetRelation<Team>("Team"); }
            set { SetRelation("Team", value); }
        }

        /// <summary>
        /// The Iteration this item is assigned to.
        /// </summary>
        [MetaRenamed("Timebox")]
        public Iteration Iteration {
            get { return GetRelation<Iteration>("Timebox"); }
            set { SetRelation("Timebox", value); }
        }

        /// <summary>
        /// The Theme this item is assigned to.
        /// </summary>
        [MetaRenamed("Parent")]
        public Theme Theme {
            get { return GetRelation<Theme>("Parent"); }
            set { SetRelation("Parent", value); }
        }

        /// <summary>
        /// This item's Status
        /// </summary>
        public IListValueProperty Status {
            get { return GetListValue<WorkitemStatus>("Status"); }
        }

        /// <summary>
        /// This Item's Priority
        /// </summary>
        public IListValueProperty Priority {
            get { return GetListValue<WorkitemPriority>("Priority"); }
        }

        /// <summary>
        /// This Item's Source
        /// </summary>
        public IListValueProperty Source {
            get { return GetListValue<WorkitemSource>("Source"); }
        }

        /// <summary>
        /// This item's order.
        /// </summary>
        [MetaRenamed("Order")]
        public Rank<PrimaryWorkitem> RankOrder {
            get { return GetRank<PrimaryWorkitem>("Order"); }
        }

        /// <summary>
        /// High-level estimate (in story points) of this item.
        /// </summary>
        public double? Estimate {
            get { return Get<double?>("Estimate"); }
            set { Set("Estimate", value); }
        }

        /// <summary>
        /// Goals this item is assigned to.
        /// </summary>
        public ICollection<Goal> Goals {
            get { return GetMultiRelation<Goal>("Goals"); }
        }

        /// <summary>
        /// Requests associated with this item.
        /// </summary>
        public ICollection<Request> Requests {
            get { return GetMultiRelation<Request>("Requests"); }
        }

        /// <summary>
        /// Issues associated with this item.
        /// </summary>
        public ICollection<Issue> Issues {
            get { return GetMultiRelation<Issue>("Issues"); }
        }

        /// <summary>
        /// Issues that are preventing the completion of this item.
        /// </summary>
        public ICollection<Issue> BlockingIssues {
            get { return GetMultiRelation<Issue>("BlockingIssues"); }
        }

        /// <summary>
        /// Defects affecting this item.
        /// </summary>
        public ICollection<Defect> AffectedByDefects {
            get { return GetMultiRelation<Defect>("AffectedByDefects"); }
        }

        /// <summary>
        /// Build Run's this Primary Workitem was completed in
        /// </summary>
        [MetaRenamed("CompletedInBuildRuns")]
        public ICollection<BuildRun> CompletedIn {
            get { return GetMultiRelation<BuildRun>("CompletedInBuildRuns"); }
        }

        /// <summary>
        /// Create a task that belongs to this item
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <returns></returns>
        public Task CreateTask(string name) {
            return Instance.Create.Task(name, this);
        }

        /// <summary>
        /// Create a task that belongs to this item
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Task CreateTask(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Task(name, this, attributes);
        }

        /// <summary>
        /// Create a test that belongs to this item
        /// </summary>
        /// <param name="name">The name of the test</param>
        /// <returns></returns>
        public Test CreateTest(string name) {
            return Instance.Create.Test(name, this);
        }

        /// <summary>
        /// Create a test that belongs to this item
        /// </summary>
        /// <param name="name">The name of the test</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Test CreateTest(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Test(name, this, attributes);
        }

        /// <summary>
        /// Gets the estimate of total effort required to implement this item.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public double? GetTotalDetailEstimate(WorkitemFilter filter) {
            return GetSum("ChildrenMeAndDown", filter ?? new WorkitemFilter(), "DetailEstimate");
        }

        /// <summary>
        /// Gets the total effort already expended to implement this item.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public double? GetTotalDone(WorkitemFilter filter) {
            return GetSum("ChildrenMeAndDown", filter ?? new WorkitemFilter(), "Actuals.Value");
        }

        /// <summary>
        /// Gets the total remaining effort required to complete implementation of this item.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public double? GetTotalToDo(WorkitemFilter filter) {
            return GetSum("ChildrenMeAndDown", filter ?? new WorkitemFilter(), "ToDo");
        }

        /// <summary>
        /// Collection of Tasks and Tests that belong to this primary workitem
        /// </summary>
        /// <param name="filter">How to filter the secondary workitems. To get only Tasks, pass a TaskFilter. To get only Tests, pass a TestFilter.</param>
        public ICollection<SecondaryWorkitem> GetSecondaryWorkitems(SecondaryWorkitemFilter filter) {
            filter = filter ?? new SecondaryWorkitemFilter();

            filter.Parent.Clear();
            filter.Parent.Add(this);

            return Instance.Get.SecondaryWorkitems(filter);
        }
    }
}