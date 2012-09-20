namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("TaskStatus")]
	internal class TaskStatus : ListValue
	{
		internal TaskStatus(AssetID id, V1Instance instance) : base(id, instance) { }
		internal TaskStatus(V1Instance instance) : base(instance) { }
	}
}