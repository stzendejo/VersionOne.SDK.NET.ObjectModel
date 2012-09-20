namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("RequestPriority")]
	internal class RequestPriority : ListValue
	{
		internal RequestPriority(AssetID id, V1Instance instance) : base(id, instance) {}

		internal RequestPriority(V1Instance instance) : base(instance) {}
	}
}
