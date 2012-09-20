namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("ThemeStatus")]
	internal class ThemeStatus : ListValue
	{
		internal ThemeStatus(AssetID id, V1Instance instance) : base(id, instance) {}

		internal ThemeStatus(V1Instance instance) : base(instance) {}
	}
}