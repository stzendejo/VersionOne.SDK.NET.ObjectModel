namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// The level at which DetailEstimate, ToDo and Effort are tracked
	/// </summary>
	public enum TrackingLevel
	{
		/// <summary>
		/// Track Detail Estimate and ToDo at PrimaryWorkitem (Story/Defect) level only
		/// </summary>
		PrimaryWorkitem = APIClient.TrackingLevel.On,
		/// <summary>
		/// Track Detail Estimate and ToDo at SecondaryWorkitem (Task/Test) level only
		/// </summary>
		SecondaryWorkitem = APIClient.TrackingLevel.Off,
		/// <summary>
		/// Track Detail Estimate and ToDo at both the PrimaryWorkitem and SecondaryWorkitem levels
		/// </summary>
		Both = APIClient.TrackingLevel.Mix
	}

	public partial class V1Instance
	{
		/// <summary>
		/// Represents configuration data of a VersionOne Instance
		/// </summary>
		public class InstanceConfiguration
		{
			/// <summary>
			/// If Effort Tracking is enabled (Effort Records can be created and Saved)
			/// </summary>
			public readonly bool EffortTrackingEnabled;
			/// <summary>
			/// The level at which DetailEstimate, ToDo and Effort are tracked for Stories
			/// </summary>
			public readonly TrackingLevel StoryTrackingLevel;
			/// <summary>
			/// The level at which DetailEstimate, ToDo and Effort are tracked for Defects
			/// </summary>
			public readonly TrackingLevel DefectTrackingLevel;

			/// <summary>
			/// The maximum size of an attachment the instance will accept before returning an error
			/// </summary>
			public readonly int MaximumAttachmentSize;

			internal InstanceConfiguration(bool effortTrackingEnabled, TrackingLevel storyTrackingLevel, TrackingLevel defectTrackingLevel, int maxAttachmentSize)
			{
				EffortTrackingEnabled = effortTrackingEnabled;
				DefectTrackingLevel = defectTrackingLevel;
				StoryTrackingLevel = storyTrackingLevel;
				MaximumAttachmentSize = maxAttachmentSize;
			}
		}
	}
}