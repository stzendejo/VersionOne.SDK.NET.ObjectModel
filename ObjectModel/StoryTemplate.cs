using System;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// A Story template
	/// </summary>
	[MetaData("Story", 200)]
	public class StoryTemplate : ProjectAsset
	{
		internal StoryTemplate(V1Instance instance) : base(instance) { }
		internal StoryTemplate(AssetID id, V1Instance instance) : base(id, instance) {}

		/// <summary>
		/// Creates a Story from this Template, copying Attributes and Relationships
		/// </summary>
		/// <returns>A Story just like this Template</returns>
		public Story GenerateStory()
		{
			Save();
			return Instance.ExecuteOperation<Story>(this, "Copy");
		}

		internal override void CloseImpl()
		{
			throw new NotSupportedException();
		}

		internal override void ReactivateImpl()
		{
			throw new NotSupportedException();
		}
	}
}
