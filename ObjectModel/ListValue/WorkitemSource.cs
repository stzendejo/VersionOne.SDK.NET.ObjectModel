namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("StorySource")]
	internal class WorkitemSource : ListValue
	{
		internal WorkitemSource(AssetID id, V1Instance instance) : base(id, instance) {}

		internal WorkitemSource(V1Instance instance) : base(instance) {}
	}
}