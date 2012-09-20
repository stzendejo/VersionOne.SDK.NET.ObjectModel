using System;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents an Effort Record in the VersionOne system
    /// </summary>
    [MetaData("Actual")]
    public class Effort : Entity {
        internal Effort(V1Instance instance) : base(instance) {}
        internal Effort(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// Story, Defect, Task, Test that this effort record belongs to
        /// </summary>
        public Workitem Workitem {
            get { return GetRelation<Workitem>("Workitem"); }
            set { SetRelation("Workitem", value); }
        }

        /// <summary>
        /// Project this effort record belongs to
        /// </summary>
        [MetaRenamed("Scope")]
        public Project Project {
            get { return GetRelation<Project>("Scope"); }
            set { SetRelation("Scope", value); }
        }

        /// <summary>
        /// Member this effort record belongs to
        /// </summary>
        public Member Member {
            get { return GetRelation<Member>("Member"); }
            set { SetRelation("Member", value); }
        }

        /// <summary>
        /// Iteration this effort record belongs to
        /// </summary>
        [MetaRenamed("Timebox")]
        public Iteration Iteration {
            get { return GetRelation<Iteration>("Timebox"); }
            set { SetRelation("Timebox", value); }
        }

        /// <summary>
        /// Team this effort record belongs to
        /// </summary>
        public Team Team {
            get { return GetRelation<Team>("Team"); }
            set { SetRelation("Team", value); }
        }

        /// <summary>
        /// Date this effort record is logged against
        /// </summary>
        public DateTime Date {
            get { return Get<DateTime>("Date"); }
            set { Set("Date", value.Date); }
        }

        /// <summary>
        /// Value of this effort record
        /// </summary>
        public double Value {
            get { return Get<double>("Value"); }
            set { Set("Value", value); }
        }
    }
}