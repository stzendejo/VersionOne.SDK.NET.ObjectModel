namespace VersionOne.SDK.ObjectModel.List {

    [MetaData("EpicStatus")]
    internal class EpicStatus : ListValue {
		internal EpicStatus(AssetID id, V1Instance instance) : base(id, instance) {}

        internal EpicStatus(V1Instance instance) : base(instance) { }
    }
}
