using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter on Project Assets
    /// </summary>
    public abstract class ProjectAssetFilter : BaseAssetFilter {
        internal override Type EntityType {
            get { return typeof (ProjectAsset); }
        }

        /// <summary>
        /// Project this item belongs to.
        /// </summary>
        public readonly ICollection<Project> Project = new List<Project>();

        /// <summary>
        /// DisplayID of this item.
        /// </summary>
        public readonly ICollection<string> DisplayID = new List<string>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Relation("Scope", Project);
            builder.Simple("Number", DisplayID);
        }
    }
}