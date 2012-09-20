namespace VersionOne.SDK.ObjectModel.List
{
    [MetaData("BuildStatus")]
    internal class BuildStatus : ListValue
    {
        internal BuildStatus(AssetID id, V1Instance instance) : base(id, instance) { }

        internal BuildStatus(V1Instance instance) : base(instance) { }
    }
}
