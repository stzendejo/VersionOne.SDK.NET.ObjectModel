namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("GoalPriority")]
	internal class GoalPriority : ListValue
	{
		internal GoalPriority(AssetID id, V1Instance instance) : base(id, instance) {}

		internal GoalPriority(V1Instance instance) : base(instance) {}
	}
}
