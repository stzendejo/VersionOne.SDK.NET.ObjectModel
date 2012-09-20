using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a Goal in the VersionOne system.
	/// </summary>
	[MetaData("Goal")]
	public class Goal : ProjectAsset
	{
		internal Goal(V1Instance instance) : base(instance) { }
		internal Goal(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// This Goal's Type
		/// </summary>
		public IListValueProperty Type { get { return GetListValue<GoalType>("Category"); } }

		/// <summary>
		/// This Goal's Priority
		/// </summary>
		public IListValueProperty Priority { get { return GetListValue<GoalPriority>("Priority"); } }

		/// <summary>
		/// A collection of Projects Targeted by this Goal
		/// </summary>
		public ICollection<Project> GetTargetedBy(ProjectFilter filter)
		{
			filter = filter ?? new ProjectFilter();
			filter.Targets.Clear();
			filter.Targets.Add(this);
			return Instance.Get.Projects(filter);
		}

		/// <summary>
		/// Epics assigned to this Goal.
		/// </summary>
		public ICollection<Epic> GetEpics(EpicFilter filter)
		{
			filter = filter ?? new EpicFilter();
			filter.Goals.Clear();
			filter.Goals.Add(this);
			return Instance.Get.Epics(filter);
		}

		/// <summary>
		/// Themes assigned to this Goal.
		/// </summary>
		public ICollection<Theme> GetThemes(ThemeFilter filter)
		{
			filter = filter ?? new ThemeFilter();
			filter.Goals.Clear();
			filter.Goals.Add(this);
			return Instance.Get.Themes(filter);
		}

		/// <summary>
		/// Stories and Defects assigned to this Goal.
		/// </summary>
		/// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
		public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.Goals.Clear();
			filter.Goals.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Inactivates the Goal
		/// </summary>
		/// <exception cref="InvalidOperationException">The Goal is an invalid state for the Operation, e.g. it is already closed.</exception>
		internal override void CloseImpl()
		{
			Instance.ExecuteOperation<Goal>(this, "Inactivate");
		}

		/// <summary>
		/// Reactivates the Goal
		/// </summary>
		/// <exception cref="InvalidOperationException">The Goal is an invalid state for the Operation, e.g. it is already active.</exception>
		internal override void ReactivateImpl()
		{
			Instance.ExecuteOperation<Goal>(this, "Reactivate");
		}
	}
}
