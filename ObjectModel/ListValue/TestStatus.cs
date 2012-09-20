namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("TestStatus")]
	internal class TestStatus : ListValue
	{
		internal TestStatus(AssetID id, V1Instance instance) : base(id, instance) { }
		internal TestStatus(V1Instance instance) : base(instance) { }
	}
}