namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("WorkitemRisk")]
	internal class WorkitemRisk : ListValue
	{
		internal WorkitemRisk(AssetID id, V1Instance instance) : base(id, instance) {}

		internal WorkitemRisk(V1Instance instance) : base(instance) {}
	}
}