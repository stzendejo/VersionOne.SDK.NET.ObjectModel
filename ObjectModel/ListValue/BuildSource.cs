namespace VersionOne.SDK.ObjectModel.List
{
    [MetaData("BuildSource")]
    internal class BuildSource : ListValue
    {
        internal BuildSource(AssetID id, V1Instance instance) : base(id, instance) { }

        internal BuildSource(V1Instance instance) : base(instance) { }
    }
}
