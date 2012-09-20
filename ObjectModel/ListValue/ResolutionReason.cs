namespace VersionOne.SDK.ObjectModel.List
{
	[MetaData("DefectResolution")]
	internal class DefectResolutionReason : ListValue
	{
		internal DefectResolutionReason(AssetID id, V1Instance instance) : base(id, instance) { }

		internal DefectResolutionReason(V1Instance instance) : base(instance) { }
	}

	[MetaData("IssueResolution")]
	internal class IssueResolutionReason : ListValue
	{
		internal IssueResolutionReason(AssetID id, V1Instance instance) : base(id, instance) { }

		internal IssueResolutionReason(V1Instance instance) : base(instance) { }
	}

	[MetaData("RequestResolution")]
	internal class RequestResolution : ListValue
	{
		internal RequestResolution(AssetID id, V1Instance instance) : base(id, instance) { }

		internal RequestResolution(V1Instance instance) : base(instance) { }
	}
}
