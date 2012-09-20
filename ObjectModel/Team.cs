using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a team in the VersionOne system
	/// </summary>
	[MetaData("Team")]
	public class Team : BaseAsset
	{
		internal Team(V1Instance instance) : base(instance) { }
		internal Team(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// The Stories and Defects assigned to this Team
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Team.Clear();
			filter.Team.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// The Issues assigned to this Team
		/// </summary>
		public ICollection<Issue> GetIssues(IssueFilter filter)
		{
			filter = filter ?? new IssueFilter();
			filter.Team.Clear();
			filter.Team.Add(this);
			return Instance.Get.Issues(filter);
		}

		/// <summary>
		/// The Retrospectives assigned to this Team
		/// </summary>
		public ICollection<Retrospective> GetRetrospectives(RetrospectiveFilter filter)
		{
			filter = filter ?? new RetrospectiveFilter();
			filter.Team.Clear();
			filter.Team.Add(this);
			return Instance.Get.Retrospectives(filter);
		}

		/// <summary>
		/// Effort Records that belong to this Team
		/// </summary>
		public ICollection<Effort> GetEffortRecords(EffortFilter filter)
		{
			filter = filter ?? new EffortFilter();
			filter.Team.Clear();
			filter.Team.Add(this);
			return Instance.Get.EffortRecords(filter);
		}

		/// <summary>
		/// Inactivates the Team
		/// </summary>
		/// <exception cref="InvalidOperationException">The Team is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Team>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the Team
		/// </summary>
		/// <exception cref="InvalidOperationException">The Theme is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Team>(this, "Reactivate");
		}

		/// <summary>
		/// Return the total estimate for all stories and defects in this team optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on.</param>
		/// <returns></returns>
		public double? GetTotalEstimate(PrimaryWorkitemFilter filter)
		{
			return GetSum("Workitems:PrimaryWorkitem", filter ?? new PrimaryWorkitemFilter(), "Estimate");
		}

		/// <summary>
		/// Return the total done for all workitems in this team optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalDone(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "Actuals.Value");
		}

		/// <summary>
		/// Return the total detail estimate for all workitems in this team optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalDetailEstimate(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "DetailEstimate");
		}

		/// <summary>
		/// Return the total to do for all workitems in this team optionally filtered
		/// </summary>
		/// <param name="filter">Criteria to filter workitems on.</param>
		/// <returns></returns>
		public double? GetTotalToDo(WorkitemFilter filter)
		{
			return GetSum("Workitems", filter ?? new WorkitemFilter(), "ToDo");
		}
	}
}
