using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel
{
    /// <summary>
    /// Represents a Build Project in the VersionOne System
    /// </summary>
    [MetaData("BuildProject")]
    public class BuildProject : BaseAsset
    {
        internal BuildProject(AssetID id, V1Instance instance) : base(id, instance) { }
        internal BuildProject(V1Instance instance) : base(instance) { }

        /// <summary>
        /// Reference of this Build Project
        /// </summary>
        public string Reference { get { return Get<string>("Reference"); } set { Set("Reference", value); } }

        /// <summary>
        /// A collection of Build Runs associated with this Build Project
        /// </summary>
        public ICollection<BuildRun> GetBuildRuns(BuildRunFilter filter)
        {
            filter = filter ?? new BuildRunFilter();
            filter.BuildProjects.Clear();
            filter.BuildProjects.Add(this);
            return Instance.Get.BuildRuns(filter);
        }

        /// <summary>
        /// Create a Build Run with the given name and date in this Build Project
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <returns>A new Build Run in this Build Project</returns>
        public BuildRun CreateBuildRun(string name, DateTime date)
        {
            return Instance.Create.BuildRun(this, name, date);
        }

        /// <summary>
        /// Create a Build Run with the given name and date in this Build Project
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Build Run in this Build Project</returns>
        public BuildRun CreateBuildRun(string name, DateTime date, IDictionary<string, object> attributes)
        {
            return Instance.Create.BuildRun(this, name, date, attributes);
        }

        internal override void CloseImpl()
        {
            Instance.ExecuteOperation<BuildProject>(this, "Inactivate");
        }

        internal override void ReactivateImpl()
        {
            Instance.ExecuteOperation<BuildProject>(this, "Reactivate");
        }
    }
}
