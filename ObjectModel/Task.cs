using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a Task in the VersionOne system
    /// </summary>
    [MetaData("Task", null, "Task.Order")]
    public class Task : SecondaryWorkitem {
        internal Task(AssetID id, V1Instance instance) : base(id, instance) {}
        internal Task(V1Instance instance) : base(instance) {}

        /// <summary>
        /// Build number associated with this task.
        /// </summary>
        [MetaRenamed("LastVersion")]
        public string Build {
            get { return Get<string>("LastVersion"); }
            set { Set("LastVersion", value); }
        }

        /// <summary>
        /// The Source of this Task
        /// </summary>
        public IListValueProperty Source {
            get { return GetListValue<TaskSource>("Source"); }
        }

        /// <summary>
        /// The Type of this Task
        /// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type {
            get { return GetListValue<TaskType>("Category"); }
        }

        /// <summary>
        /// The Status of this Task
        /// </summary>
        public IListValueProperty Status {
            get { return GetListValue<TaskStatus>("Status"); }
        }

        /// <summary>
        /// This item's order.
        /// </summary>
        [MetaRenamed("Order")]
        public Rank<Task> RankOrder {
            get { return GetRank<Task>("Order"); }
        }

        internal override void CloseImpl() {
            Instance.ExecuteOperation<Task>(this, "Inactivate");
        }

        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Task>(this, "Reactivate");
        }
    }
}