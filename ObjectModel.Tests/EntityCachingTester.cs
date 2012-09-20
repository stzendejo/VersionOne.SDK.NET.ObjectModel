using NUnit.Framework;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class EntityCachingTester : BaseSDKTester
	{
		string _originalName = "Original Name";
		string _newName = "New Name";

		[Test] public void GetByID()
		{
			Story story = SandboxProject.CreateStory(_originalName);
			AssetID id = story.ID;

			// Make sure Name is in the cache.
			Assert.AreEqual(_originalName, story.Name);

			// Update story name to "New Name"
			UpdateStory(id, _originalName, _newName);

			// Assert 
			Story story2 = Instance.Get.StoryByID(id);
			Assert.AreEqual(_newName, story2.Name);
		}

		[Test] public void GetByID2()
		{
			Story story = SandboxProject.CreateStory(_originalName);
			AssetID id = story.ID;

			// Make sure Name is in the cache.
			Assert.AreEqual(_originalName, story.Name);

			// Update story name to "New Name"
			UpdateStory(id, _originalName, _newName);

			// This should not affect my cached value.
			Assert.AreEqual(_originalName, story.Name);

			// Assert 
			Story story2 = Instance.Get.StoryByID(id);
			Assert.AreEqual(_newName, story2.Name);

			// Both versions should now show the same value.
			Assert.AreEqual(_newName, story.Name);
		}

		private void UpdateStory(AssetID id, string originalName, string newName)
		{
			IMetaModel metaModel = Instance.ApiClient.MetaModel;
			IServices services = Instance.ApiClient.Services;

			Oid storyId = Oid.FromToken(id.Token, metaModel);
			Query query = new Query(storyId);
			IAssetType storyType = metaModel.GetAssetType("Story");
			IAttributeDefinition nameAttribute = storyType.GetAttributeDefinition("Name");
			query.Selection.Add(nameAttribute);
			QueryResult result = services.Retrieve(query);
			Asset storyAsset = result.Assets[0];
			string oldName = storyAsset.GetAttribute(nameAttribute).Value.ToString();
			Assert.AreEqual(originalName, oldName);
			storyAsset.SetAttributeValue(nameAttribute, newName);
			services.Save(storyAsset);
		}
	}
}
