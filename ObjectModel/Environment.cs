using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// Environment representation.
    /// </summary>
    [MetaData("Environment")]
    public class Environment : Entity 
    {        
        internal Environment(AssetID assetId, V1Instance instance) : base(assetId, instance) { }

        internal Environment(V1Instance instance) : base(instance) { }

        /// <summary>
        /// Name of the Environment
        /// </summary>
        public string Name { get { return Get<string>("Name"); } set { Set("Name", value); } }

        /// <summary>
        /// The Project this ProjectAsset belongs in. This must be present.
        /// </summary>
        [MetaRenamed("Scope")]
        public Project Project { get { return GetRelation<Project>("Scope"); } set { SetRelation("Scope", value); } }

        /// <summary>
        /// ID (or Number) of this entity as displayed in the VersionOne system.
        /// </summary>
        [MetaRenamed("Number")]
        public string DisplayID { get { return Get<string>("Number"); } }

        /// <summary>
        /// TestSets associated with this Environment
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Criteria to filter TestSets on. </returns>
        public ICollection<TestSet> GetTestSets(TestSetFilter filter)
        {
            filter = filter ?? new TestSetFilter();
            filter.Environment.Clear();
            filter.Environment.Add(this);
            return Instance.Get.TestSets(filter);
        }

        #region Actions
        /// <summary>
        /// Indicates if the entity is open or active.
        /// </summary>
        public bool IsActive
        {
            get { return IsActiveImpl; }
        }

        /// <summary>
        /// Indicates if the entity is closed or inactive.
        /// </summary>
        public bool IsClosed
        {
            get { return IsClosedImpl; }
        }

        /// <summary>
        /// Internal worker function to determine active state.
        /// </summary>
        internal bool IsActiveImpl
        {
            get { return Get<byte>("AssetState", true) == 64; }
        }

        /// <summary>
        /// Internal worker function to determine closed state.
        /// </summary>
        internal bool IsClosedImpl
        {
            get { return Get<byte>("AssetState", true) == 128; }
        }

        /// <summary>
        /// True if the item can be closed.
        /// </summary>
        public bool CanClose { get { return CanCloseImpl; } }

        internal bool CanCloseImpl { get { return Instance.CanExecuteOperation(this, "Inactivate"); } }

        /// <summary>
        /// Closes or Inactivates the item
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already closed or inactive.</exception>
        public void Close()
        {
            Save();
            CloseImpl();
            ClearCache("AssetState");
        }

        internal void CloseImpl()
        {
            Instance.ExecuteOperation<Environment>(this, "Inactivate");
        }

        /// <summary>
        /// True if item can be Reactivated.
        /// </summary>
        public bool CanReactivate { get { return CanReactivateImpl; } }

        internal bool CanReactivateImpl { get { return Instance.CanExecuteOperation(this, "Reactivate"); } }

        /// <summary>
        /// Reactivates the item
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already open or active.</exception>
        public void Reactivate()
        {
            ReactivateImpl();
            Save();
            ClearCache("AssetState");
        }

        internal void ReactivateImpl()
        {
            Instance.ExecuteOperation<Environment>(this, "Reactivate");
        }

        #endregion

    }
}