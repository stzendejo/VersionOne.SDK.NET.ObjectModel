namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("RegressionTestStatus")]
	internal class RegressionTestStatus : ListValue
	{
		internal RegressionTestStatus(AssetID id, V1Instance instance) : base(id, instance) { }
		internal RegressionTestStatus(V1Instance instance) : base(instance) { }
	}
}