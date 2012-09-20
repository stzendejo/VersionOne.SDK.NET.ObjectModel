namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("DefectType")]
	internal class DefectType : ListValue
	{
		internal DefectType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal DefectType(V1Instance instance) : base(instance) {}
	}
}