using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a schedule in the VersionOne system
	/// </summary>
    [MetaData("Schedule")]
	public class Schedule : BaseAsset
	{
	    internal Schedule(V1Instance instance) : base(instance) { }
	    internal Schedule(AssetID id, V1Instance instance) : base(id, instance) { }

        /// <summary>
        /// Projects associated with this Schedule
        /// </summary>
        public ICollection<Project> ScheduledProjects { get { return GetMultiRelation<Project>("ScheduledScopes"); } }

	    /// <summary>
		/// The duration an iteration will last in this schedule.
		/// </summary>
		public Duration IterationLength
		{
			get { return Get<Duration>("TimeboxLength"); }
			set { Set("TimeboxLength",value); }
		}

		/// <summary>
		/// The duration between iterations in this schedule.
		/// </summary>
		public Duration IterationGap
		{
            get { return Get<Duration>("TimeboxGap"); } 
			set { Set("TimeboxGap", value); }
		}

        /// <summary>
        /// Inactivates the Schedule
        /// </summary>
        /// <exception cref="InvalidOperationException">The Schedule is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl()
        {
            Instance.ExecuteOperation<Schedule>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Schedule
        /// </summary>
        /// <exception cref="InvalidOperationException">The Schedule is an invalid state for the Operation, e.g. it is already active.</exception>
        internal override void ReactivateImpl()
        {
            Instance.ExecuteOperation<Schedule>(this, "Reactivate");
        }
	}
}