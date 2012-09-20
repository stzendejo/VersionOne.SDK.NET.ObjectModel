namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("GoalCategory")]
	internal class GoalType : ListValue
	{
		internal GoalType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal GoalType(V1Instance instance) : base(instance) {}
	}
}