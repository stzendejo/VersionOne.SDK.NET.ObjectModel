namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("ThemeSource")]
	internal class ThemeSource : ListValue
	{
		internal ThemeSource(AssetID id, V1Instance instance) : base(id, instance) {}

		internal ThemeSource(V1Instance instance) : base(instance) {}
	}
}