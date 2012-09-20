using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a Theme in the VersionOne system.
    /// </summary>
    [MetaData("Theme")]
    public class Theme : ProjectAsset {
        internal Theme(V1Instance instance) : base(instance) {
        }

        internal Theme(AssetID id, V1Instance instance) : base(id, instance) {
        }

        /// <summary>
        /// Members that pwn this item.
        /// </summary>
        public ICollection<Member> Owners {
            get { return GetMultiRelation<Member>("Owners"); }
        }

        /// <summary>
        /// This Theme's Risk
        /// </summary>
        public IListValueProperty Risk {
            get { return GetListValue<WorkitemRisk>("Risk"); }
        }

        /// <summary>
        /// This Theme's Priority
        /// </summary>
        public IListValueProperty Priority {
            get { return GetListValue<WorkitemPriority>("Priority"); }
        }

        /// <summary>
        /// This Theme's Source
        /// </summary>
        public IListValueProperty Source {
            get { return GetListValue<ThemeSource>("Source"); }
        }

        /// <summary>
        /// This Theme's Type
        /// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type {
            get { return GetListValue<ThemeType>("Category"); }
        }

        /// <summary>
        /// This Theme's Status
        /// </summary>
        public IListValueProperty Status {
            get { return GetListValue<ThemeStatus>("Status"); }
        }

        /// <summary>
        /// The Theme this Theme is assigned to.
        /// </summary>
        [MetaRenamed("Parent")]
        public Theme ParentTheme {
            get { return GetRelation<Theme>("Parent"); }
            set { SetRelation("Parent", value); }
        }

        /// <summary>
        /// Member assigned as a customer for this Theme.
        /// </summary>
        public Member Customer {
            get { return GetRelation<Member>("Customer"); }
            set { SetRelation("Customer", value); }
        }

        /// <summary>
        /// Build number associated with this Theme.
        /// </summary>
        [MetaRenamed("LastVersion")]
        public string Build {
            get { return Get<string>("LastVersion"); }
            set { Set("LastVersion", value); }
        }

        /// <summary>
        /// Gets the estimate of total effort required to implement this Feature Group.
        /// </summary>
        /// <param name="filter">filter for WorkitemFilter (If null, then all items returned).</param>
        /// <returns>estimate of total effort required to implement this Feature Group.</returns>
        public double? GetTotalDetailEstimate(WorkitemFilter filter) {
            return GetSum("ChildrenMeAndDown", filter ?? new WorkitemFilter(), "DetailEstimate");
        }


        /// <summary>
        /// Gets the total remaining effort required to complete implementation of this Feature Group.
        /// </summary>
        /// <param name="filter">restriction for the work items which have to be counted. 
        /// If null, then all items counted.</param>
        /// <returns>total remaining effort required to complete implementation of this Feature Group.</returns>
        public double? GetTotalToDo(WorkitemFilter filter) {
            return GetSum("ChildrenMeAndDown", filter ?? new WorkitemFilter(), "ToDo");
        }

        /// <summary>
        /// High-level estimate of this Theme.
        /// </summary>
        public double? Estimate {
            get { return Get<double?>("Estimate"); }
            set { Set("Estimate", value); }
        }

        /// <summary>
        /// Stories and Defects assigned to this Theme.
        /// </summary>
        /// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
        public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter) {
            filter = filter ?? new PrimaryWorkitemFilter();
            filter.Theme.Clear();
            filter.Theme.Add(this);
            return Instance.Get.PrimaryWorkitems(filter);
        }

        /// <summary>
        /// Child Themes of this Theme.
        /// </summary>
        public ICollection<Theme> GetChildThemes(ThemeFilter filter) {
            filter = filter ?? new ThemeFilter();
            filter.Parent.Clear();
            filter.Parent.Add(this);
            return Instance.Get.Themes(filter);
        }

        /// <summary>
        /// Goals this Theme is assigned to.
        /// </summary>
        public ICollection<Goal> Goals {
            get { return GetMultiRelation<Goal>("Goals"); }
        }

        /// <summary>
        /// Inactivates the Theme
        /// </summary>
        /// <exception cref="InvalidOperationException">The Theme is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl() {
            Instance.ExecuteOperation<Theme>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Theme
        /// </summary>
        /// <exception cref="InvalidOperationException">The Theme is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Theme>(this, "Reactivate");
        }

        /// <summary>
        /// Create a theme that is a child of this theme.
        /// </summary>
        /// <param name="name">Name of the new theme.</param>
        /// <returns>The new theme.</returns>
        public Theme CreateChildTheme(string name) {
            return CreateChildTheme(name, null);
        }

        /// <summary>
        /// Create a theme that is a child of this theme.
        /// </summary>
        /// <param name="name">Name of the new theme.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>The new theme.</returns>
        public Theme CreateChildTheme(string name, IDictionary<string, object> attributes) {
            var theme = Instance.Create.Theme(name, Project, attributes);

            Instance.Create.AddAttributes(theme, attributes);

            theme.ParentTheme = this;
            theme.Save();
            return theme;
        }
    }
}