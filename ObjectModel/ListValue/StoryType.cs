namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("StoryCategory")]
	internal class StoryType : ListValue
	{
		internal StoryType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal StoryType(V1Instance instance) : base(instance) {}
	}
}