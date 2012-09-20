namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("ThemeCategory")]
	internal class ThemeType : ListValue
	{
		internal ThemeType(AssetID id, V1Instance instance) : base(id, instance) {}

		internal ThemeType(V1Instance instance) : base(instance) {}
	}
}