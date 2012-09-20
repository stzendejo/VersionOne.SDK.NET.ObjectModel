namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents the base of a Task or Test in the VersionOne System
    /// </summary>
    [MetaData("Workitem")]
    public abstract class SecondaryWorkitem : Workitem {
        internal SecondaryWorkitem(AssetID id, V1Instance instance) : base(id, instance) {
        }

        internal SecondaryWorkitem(V1Instance instance) : base(instance) {
        }

        /// <summary>
        /// The story, defect or epic this item belongs to.
        /// </summary>
        public Workitem Parent {
            get { return GetRelation<Workitem>("Parent"); }
            set { SetRelation("Parent", value); }
        }
    }
}