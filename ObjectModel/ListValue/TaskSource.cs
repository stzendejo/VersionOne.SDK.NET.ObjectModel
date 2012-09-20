namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("TaskSource")]
	internal class TaskSource : ListValue
	{
		internal TaskSource(AssetID id, V1Instance instance) : base(id, instance) { }
		internal TaskSource(V1Instance instance) : base(instance) { }
	}
}