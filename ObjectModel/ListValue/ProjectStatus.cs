namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("ScopeStatus")]
	internal class ProjectStatus : ListValue
	{
		internal ProjectStatus(AssetID id, V1Instance instance) : base(id, instance) {}

		internal ProjectStatus(V1Instance instance) : base(instance) {}
	}
}