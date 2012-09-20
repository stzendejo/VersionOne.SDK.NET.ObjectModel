namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("TaskCategory")]
	internal class TaskType : ListValue
	{
		internal TaskType(AssetID id, V1Instance instance) : base(id, instance) { }
		internal TaskType(V1Instance instance) : base(instance) { }
	}
}