namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("StoryStatus")]
	internal class WorkitemStatus : ListValue
	{
		internal WorkitemStatus(AssetID id, V1Instance instance) : base(id, instance) {}

		internal WorkitemStatus(V1Instance instance) : base(instance) {}
	}
}