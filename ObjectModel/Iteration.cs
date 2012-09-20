using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents an iteration or sprint in the VersionOne System
	/// </summary>
	[MetaData("Timebox")]
	public class Iteration : BaseAsset
	{
		internal Iteration(AssetID id, V1Instance instance) : base(id, instance) { }
		internal Iteration(V1Instance instance) : base(instance)
		{
			SetRelation("State", State.Future);
		}

		/// <summary>
        /// The Schedule this Iteration belongs in. This must be present.
		/// </summary>
        public Schedule Schedule { get { return GetRelation<Schedule>("Schedule"); } set { SetRelation("Schedule", value); } }

		/// <summary>
		/// The begin date or start date of this iteration
		/// </summary>
		public DateTime BeginDate { get { return Get<DateTime>("BeginDate"); } set { Set("BeginDate", value.Date); } }

		/// <summary>
		/// The end date of this iteration
		/// </summary>
		public DateTime EndDate { get { return Get<DateTime>("EndDate"); } set { Set("EndDate", value.Date); } }

		/// <summary>
		/// A collection of stories and defects that belong to this iteration
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Iteration.Clear();
			filter.Iteration.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// A read-only collection of Effort Records that belong to this iteration
		/// </summary>
		public ICollection<Effort> GetEffortRecords(EffortFilter filter)
		{
			filter = filter ?? new EffortFilter();
			filter.Iteration.Clear();
			filter.Iteration.Add(this);
			return Instance.Get.EffortRecords(filter);
		}

		/// <summary>
		/// True if this iteration is in the future state and can be opened or activated
		/// </summary>
		public bool CanActivate { get { return Instance.CanExecuteOperation(this, "Activate"); } }

		/// <summary>
		/// Open or activate this iteration.  Iteration must be in a future state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already open or active.</exception>
		public void Activate()
		{
			Save();
			Instance.ExecuteOperation(this, "Activate");
		}

		/// <summary>
		/// True if this iteration is marked future
		/// </summary>
		public bool IsFuture { get { return Get<byte>("AssetState") == 0; } }

		/// <summary>
		/// True if this iteration is open or active and can be marked future
		/// </summary>
		public bool CanMakeFuture { get { return Instance.CanExecuteOperation(this, "MakeFuture"); } }

		/// <summary>
		/// Mark this iteration as future.  Iteration must be in an open or active state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already in a future state.</exception>
		public void MakeFuture()
		{
			Save();
			Instance.ExecuteOperation(this, "MakeFuture");
		}

		internal override bool CanCloseImpl { get { return Instance.CanExecuteOperation(this, "Close"); } }

		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Iteration>(this, "Close");
		}

		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Iteration>(this, "Reactivate");
		}

		/// <summary>
		/// Return the total estimate for all stories and defects in this iteration optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		/// <returns></returns>
		public double? GetTotalEstimate(PrimaryWorkitemFilter filter)
		{
			return GetSum("Workitems:PrimaryWorkitem", filter ?? new PrimaryWorkitemFilter(), "Estimate");
		}

		/// <summary>
		/// Return the total estimate for all workitems in this iteration optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalDetailEstimate(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "DetailEstimate");
		}

		/// <summary>
		/// Return the total to do for all workitems in this iteration optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalToDo(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "ToDo");
		}

		/// <summary>
		/// Return the total done for all workitems in this iteration optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalDone(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "Actuals.Value");
		}
	}
}
