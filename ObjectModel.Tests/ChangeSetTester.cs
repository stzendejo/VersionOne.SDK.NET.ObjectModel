using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class ChangeSetTester : BaseSDKTester
	{
		private string _name = "Test ChangeSet";
		private string _reference = "123456";

		[Test] public void Create()
		{
			ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference);

			Assert.AreEqual(_name, changeSet.Name);
			Assert.AreEqual(_reference, changeSet.Reference);
		}

        [Test]
        public void CreateWithAttributes()
        {
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Description", "Test for ChangeSet creation with required attributes");

            ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference, attributes);

            Assert.AreEqual(_name, changeSet.Name);
            Assert.AreEqual(_reference, changeSet.Reference);
            Assert.AreEqual("Test for ChangeSet creation with required attributes", changeSet.Description);
        }

		[Test] public void Delete()
		{
			ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference);

			AssetID id = changeSet.ID;

			changeSet.Delete();

			ResetInstance();

			Assert.IsNull(Instance.Get.ChangeSetByID(id));
		}

		[ExpectedException(typeof(NotSupportedException))]
		[Test] public void Close()
		{
			ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference);
			Assert.IsFalse(changeSet.CanClose);
			changeSet.Close();
		}

		[Test] public void GetBuildRuns()
		{
			ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference);
			ChangeSet notMyChangeSet = Instance.Create.ChangeSet("Other ChangeSet", "abcd");
			BuildProject buildProject = Instance.Create.BuildProject("BP", "1234");
			BuildRun buildRun = buildProject.CreateBuildRun("BR", DateTime.Now);
			BuildRun notMyBuildRun = buildProject.CreateBuildRun("Not My BR", DateTime.Now);
			buildRun.ChangeSets.Add(changeSet);
			notMyBuildRun.ChangeSets.Add(notMyChangeSet);
			AssetID changeSetId = changeSet.ID;
			AssetID buildRunId = buildRun.ID;

			ResetInstance();

			changeSet = Instance.Get.ChangeSetByID(changeSetId);
			ICollection<BuildRun> buildRuns = changeSet.GetBuildRuns(null);
			Assert.AreEqual(1,buildRuns.Count);

			IEnumerator<BuildRun> enumerator = buildRuns.GetEnumerator();
			enumerator.MoveNext();
			Assert.AreEqual(buildRunId, enumerator.Current.ID);
		}

		[Test] public void PrimaryWorkitems()
		{
			ChangeSet changeSet = Instance.Create.ChangeSet(_name, _reference);
			Story story = SandboxProject.CreateStory("Test Story");
			changeSet.PrimaryWorkitems.Add(story);
			AssetID changeSetId = changeSet.ID;

			ResetInstance();

			changeSet = Instance.Get.ChangeSetByID(changeSetId);
			Assert.AreEqual(1, changeSet.PrimaryWorkitems.Count);
		}
	}
}