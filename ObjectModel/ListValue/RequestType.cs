namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("RequestCategory")]
	internal class RequestType : ListValue
	{
		internal RequestType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal RequestType(V1Instance instance) : base(instance) {}
	}
}