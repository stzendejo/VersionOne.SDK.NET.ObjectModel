namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("RequestStatus")]
	internal class RequestStatus : ListValue
	{
		internal RequestStatus(AssetID id, V1Instance instance) : base(id, instance) {}

		internal RequestStatus(V1Instance instance) : base(instance) {}
	}
}