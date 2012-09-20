namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// An Entity that only exists in the context of a Project.
    /// </summary>
    [MetaData(null, "Number")]
    public abstract class ProjectAsset : BaseAsset {
        internal ProjectAsset(V1Instance instance) : base(instance) {}

        internal ProjectAsset(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// The Project this ProjectAsset belongs in. This must be present.
        /// </summary>
        [MetaRenamed("Scope")]
        public Project Project {
            get { return GetRelation<Project>("Scope"); }
            set { SetRelation("Scope", value); }
        }

        /// <summary>
        /// ID (or Number) of this entity as displayed in the VersionOne system.
        /// </summary>
        [MetaRenamed("Number")]
        public string DisplayID {
            get { return Get<string>("Number"); }
        }
    }
}