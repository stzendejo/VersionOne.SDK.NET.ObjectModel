namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("IssuePriority")]
	internal class IssuePriority : ListValue
	{
		internal IssuePriority(AssetID id, V1Instance instance) : base(id, instance) {}

		internal IssuePriority(V1Instance instance) : base(instance) {}
	}
}
