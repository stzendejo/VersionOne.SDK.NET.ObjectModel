namespace VersionOne.SDK.ObjectModel.List {

    [MetaData("EpicPriority")]
    internal class EpicPriority : ListValue {
		internal EpicPriority(AssetID id, V1Instance instance) : base(id, instance) {}

        internal EpicPriority(V1Instance instance) : base(instance) { }         
    }
}