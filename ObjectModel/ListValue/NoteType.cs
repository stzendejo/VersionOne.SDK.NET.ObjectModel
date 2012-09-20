namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("NoteCategory")]
	internal class NoteType : ListValue
	{
		internal NoteType(AssetID id, V1Instance instance) : base(id, instance) { }
		internal NoteType(V1Instance instance) : base(instance) { }
	}
}