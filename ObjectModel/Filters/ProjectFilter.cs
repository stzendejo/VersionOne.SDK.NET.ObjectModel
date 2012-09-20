using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for getting projects.
	/// </summary>
	public class ProjectFilter : BaseAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Project); }
        }

		/// <summary>
		/// Filter on Targets
		/// </summary>
		public ICollection<Goal> Targets = new List<Goal>();

		/// <summary>
		/// Filter on Parent Project
		/// </summary>
		public ICollection<Project> Parent = new List<Project>();

		/// <summary>
		/// Filter on Project TestSuite
		/// </summary>
		public ICollection<TestSuite> TestSuite = new List<TestSuite>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("Parent", Parent);
			builder.Relation("TestSuite", TestSuite);

			builder.MultiRelation("Targets", Targets);
		}
	}
}
