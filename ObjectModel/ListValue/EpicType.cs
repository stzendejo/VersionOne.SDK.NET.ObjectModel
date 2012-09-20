namespace VersionOne.SDK.ObjectModel.List {
    [MetaData("EpicCategory")]
	internal class EpicType : ListValue {
		internal EpicType(AssetID id, V1Instance instance) : base(id, instance) {}

        internal EpicType(V1Instance instance) : base(instance) { }
    }
}
