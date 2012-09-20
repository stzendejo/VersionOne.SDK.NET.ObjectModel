using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
    /// <summary>
    /// Represents a Build Run in the VersionOne System
    /// </summary>
    [MetaData("BuildRun")]
    public class BuildRun : BaseAsset
    {
        internal BuildRun(AssetID id, V1Instance instance) : base(id, instance) { }
        internal BuildRun(V1Instance instance) : base(instance) { }

        /// <summary>
        /// The Build Project this Build Run belongs to
        /// </summary>
        public BuildProject BuildProject { get { return GetRelation<BuildProject>("BuildProject"); } set { SetRelation("BuildProject", value); } }

        /// <summary>
        /// The Date this Build Run ran
        /// </summary>
        public DateTime Date { get { return Get<DateTime>("Date"); } set { Set("Date", value);} }

        /// <summary>
        /// The total elapsed time of the Build  Run
        /// </summary>
        public double? Elapsed { get { return Get<double?>("Elapsed"); } set { Set("Elapsed", value); } }

        /// <summary>
        /// A reference to an external system
        /// </summary>
        public string Reference { get { return Get<string>("Reference"); } set { Set("Reference",value); } }

        /// <summary>
        /// The source of this Build Run
        /// </summary>
        public IListValueProperty Source { get { return GetListValue<BuildSource>("Source"); } }

        /// <summary>
        /// The status of this Build Run
        /// </summary>
        public IListValueProperty Status { get { return GetListValue<BuildStatus>("Status"); } }

		/// <summary>
		/// ChangeSets in this BuildRun
		/// </summary>
		public ICollection<ChangeSet> ChangeSets { get { return GetMultiRelation<ChangeSet>("ChangeSets"); } }

		/// <summary>
		/// Stories and Defects completed in this Build Run
		/// </summary>
		public ICollection<PrimaryWorkitem> GetCompletedPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			filter.CompletedIn.Clear();
			filter.CompletedIn.Add(this);
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Stories and Defects with source changes in this Build Run
		/// </summary>
		public ICollection<PrimaryWorkitem> GetAffectedPrimaryWorkitems(PrimaryWorkitemFilter filter)
		{
			filter = filter ?? new PrimaryWorkitemFilter();
			// The BuildRun's connected to the ChangeSets.
			foreach (ChangeSet changeSet in ChangeSets)
			{
				// The ChangeSet's connected to the PWI's. Sing with me, now!
				foreach (PrimaryWorkitem pwi in changeSet.PrimaryWorkitems)
				{
					// Add the specific items by ID.
					filter.DisplayID.Add(pwi.DisplayID);
				}
			}
			return Instance.Get.PrimaryWorkitems(filter);
		}

		/// <summary>
		/// Defects found in this Build Run
		/// </summary>
		public ICollection<Defect> GetFoundDefects(DefectFilter filter)
		{
			filter = filter ?? new DefectFilter();
			filter.FoundIn.Clear();
			filter.FoundIn.Add(this);
			return Instance.Get.Defects(filter);
		}

        internal override bool CanCloseImpl { get { return false; } }
        internal override bool CanReactivateImpl { get { return false; } }

        internal override void CloseImpl()
        {
            throw new InvalidOperationException("Cannot close build runs");
        }

        internal override void ReactivateImpl()
        {
            throw new InvalidOperationException("Cannot reactivate build runs");
        }
    }
}
