namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("AttachmentCategory")]
	internal class AttachmentType : ListValue
	{
		internal AttachmentType(AssetID id, V1Instance instance) : base(id, instance) { }
		internal AttachmentType(V1Instance instance) : base(instance) { }
	}
}
