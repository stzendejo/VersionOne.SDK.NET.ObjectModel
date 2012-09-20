namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("WorkitemPriority")]
	internal class WorkitemPriority : ListValue
	{
		internal WorkitemPriority(AssetID id, V1Instance instance) : base(id, instance) {}

		internal WorkitemPriority(V1Instance instance) : base(instance) {}
	}
}
