namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("IssueCategory")]
	internal class IssueType : ListValue
	{
		internal IssueType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal IssueType(V1Instance instance) : base(instance) {}
	}
}