namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("TestCategory")]
	internal class TestType : ListValue
	{
		internal TestType(AssetID id, V1Instance instance) : base(id, instance) { }
		internal TestType(V1Instance instance) : base(instance) { }
	}
}