using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for Tests
	/// </summary>
	public class TestFilter : SecondaryWorkitemFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Test); }
        }

		/// <summary>
		/// The Tests' Type.  Must be complete match.
		/// </summary>
		public readonly ICollection<string> Type = new List<string>();

		/// <summary>
		/// The Status of the Test
		/// </summary>
		public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// The Epic that this Test belongs to.
        /// </summary>
        public readonly ICollection<Epic> Epic = new List<Epic>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.ListRelation<TestType>("Category", Type);
			builder.ListRelation<TestStatus>("Status", Status);
            builder.Relation("Parent", Epic);
		}

		internal override void InternalModifyState(FilterBuilder builder)
		{
			if (HasState)
				if (HasActive)
					builder.Root.And(new TokenTerm("AssetState='Active';AssetType='Test'"));
				else
					builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='Test'"));
			else
				builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Test'"));
		}
	}
}
