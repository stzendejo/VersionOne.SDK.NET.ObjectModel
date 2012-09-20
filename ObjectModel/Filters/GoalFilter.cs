using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for Goals
	/// </summary>
	public class GoalFilter : ProjectAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Goal); }
        }

		/// <summary>
		/// The Goal's Type
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();
		/// <summary>
		/// The Goal's Priority
		/// </summary>
		public readonly ICollection<string> Priority = new List<string>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.ListRelation<GoalType>("Category",Type);
			builder.ListRelation<GoalPriority>("Priority", Priority);
		}
	}
}
