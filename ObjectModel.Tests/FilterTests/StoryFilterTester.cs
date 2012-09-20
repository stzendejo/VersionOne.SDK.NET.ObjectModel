using System;
using System.Linq;
using NUnit.Framework;

using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
	[TestFixture]
    public class StoryFilterTester : PrimaryWorkitemFilterTesterBase {
        private StoryFilter GetFilter() {
            var filter = new StoryFilter();
            filter.Project.Add(sandboxProject);
			return filter;
		}

        [Test]
        public void Project() {
            var stories = Instance.Get.Stories(GetFilter());

            foreach(var result in stories) {
				Assert.AreEqual(SandboxProject, result.Project);
		}
        }

        [Test]
        public void NoOwner() {
            var filter = GetFilter();
			filter.Owners.Add(null);

            var stories = Instance.Get.Stories(filter);

            if(stories.Any(x => x.Owners.Count > 0)) {
					Assert.Fail("Filtered Query should only return stories owned by no one.");
			}
		}

        [Test]
        public void FilterExist() {
            var assetType = Instance.ApiClient.MetaModel.GetAssetType("Story");

            var customFilter = new FilterTerm(assetType.GetAttributeDefinition("Timebox"));
            customFilter.Exists();

            var query = new Query(assetType) {Filter = customFilter};
            Instance.ApiClient.Services.Retrieve(query);
        }

        [Test]
        public void NoOrAndreOwner() {
            var filter = GetFilter();
			filter.Owners.Add(null);
            filter.Owners.Add(andre);

            var stories = Instance.Get.Stories(filter);

            if(stories.Any(x => !FindRelated(andre, x.Owners) && x.Owners.Count > 0)) {
                Assert.Fail("Filtered Query should only return stories owned by {0} or no one.", andre.Name);
			}
		}

        [Test]
        public void Names() {
            var filter = GetFilter();
			filter.Name.Add("Defect 2");
			filter.Name.Add("Story 2");

            var stories = Instance.Get.Stories(filter);

			Assert.AreEqual(1, stories.Count);
		}

        [Test]
        public void DisplayIDs() {
            var filter = GetFilter();
            filter.DisplayID.Add(story1.DisplayID);
            filter.DisplayID.Add(defect1.DisplayID);

            var stories = Instance.Get.Stories(filter);

			Assert.AreEqual(1, stories.Count);
		}

        [Test]
        public void State() {
            var stories = Instance.Get.Stories(GetFilter());

            var allStoriesCount = stories.Count;

            var closedStory = sandboxProject.CreateStory("Close Me");
			closedStory.Close();

			Assert.AreEqual(++allStoriesCount, Instance.Get.Stories(GetFilter()).Count);

            var openFilter = GetFilter();
			openFilter.State.Add(Filters.State.Active);
            var activeStories = Instance.Get.Stories(openFilter);
			Assert.AreEqual(allStoriesCount-1, activeStories.Count );

            foreach(var story in activeStories) {
				Assert.IsTrue(story.IsActive);
            }

            var closedFilter = GetFilter();
			closedFilter.State.Add(Filters.State.Closed);
            var closedStories = Instance.Get.Stories(closedFilter);
			Assert.AreEqual(1, closedStories.Count);

            foreach(var story in closedStories) {
				Assert.IsTrue(story.IsClosed);
		}
        }

        [Test]
        public void RequestedBy() {
            var story = SandboxProject.CreateStory("RequestdBy Filter");
			story.RequestedBy = "ME";
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

            var filter = new StoryFilter();
			filter.RequestedBy.Add("ME");

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual("ME", result.RequestedBy);
		}
        }

        [Test]
        public void Build() {
            var story = SandboxProject.CreateStory("Build Filter");
			story.Build = "10.2.24.1";
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

            var filter = GetFilter();
			filter.Build.Add("10.2.24.1");

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual("10.2.24.1", result.Build);
		}
        }

        [Test]
        public void Epic() {
            var epic = Instance.Create.Epic("Filter by me", SandboxProject);
            var story = epic.GenerateChildStory();
			story.Name = "Find Me";
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);
			epic = Instance.Get.EpicByID(epic.ID);

            var filter = GetFilter();
			filter.Epic.Add(epic);
            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual(epic, result.Epic);
		}
        }

        [Test]
        public void Risk() {
            var story = SandboxProject.CreateStory("Risk Filter");
            var riskValue = story.Risk.AllValues[0];
			story.Risk.CurrentValue = riskValue;
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

            var filter = GetFilter();
			filter.Risk.Add(riskValue);

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual(riskValue, result.Risk.CurrentValue);
		}
        }

        [Test]
        public void Type() {
            var story = SandboxProject.CreateStory("Type Filter");
            var typeValue = story.Type.AllValues[0];
			story.Type.CurrentValue = typeValue;
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);

            var filter = GetFilter();
			filter.Type.Add(typeValue);

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual(typeValue, result.Type.CurrentValue);
		}
        }

        [Test]
        public void Customer() {
            var customer = Instance.Members.FirstOrDefault();

            var story = SandboxProject.CreateStory("Customer filter");
			story.Customer = customer;
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);
			customer = Instance.Get.MemberByID(customer.ID);

            var filter = GetFilter();
			filter.Customer.Add(customer);

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual(customer, result.Customer);
		}
        }

        [Test]
        public void DependsOnStories() {
            var benefactor = SandboxProject.CreateStory("Benefactor");
            var dependant = SandboxProject.CreateStory("Dependant");
			dependant.DependsOnStories.Add(benefactor);
			dependant.Save();

			ResetInstance();
			dependant = Instance.Get.StoryByID(dependant.ID);
			benefactor = Instance.Get.StoryByID(benefactor.ID);

            var filter = GetFilter();
			filter.DependsOnStories.Add(benefactor);
            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(dependant, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.IsTrue(FindRelated(benefactor, result.DependsOnStories), "Expected story to depend on value used in filter");
		}
        }

        [Test]
        public void DependantStories() {
            var benefactor = SandboxProject.CreateStory("Benefactor");
            var dependant = SandboxProject.CreateStory("Dependant");
			dependant.DependsOnStories.Add(benefactor);
			dependant.Save();

			ResetInstance();
			dependant = Instance.Get.StoryByID(dependant.ID);
			benefactor = Instance.Get.StoryByID(benefactor.ID);

            var filter = GetFilter();
			filter.DependentStories.Add(dependant);
            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(benefactor, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.IsTrue(FindRelated(dependant, result.DependentStories), "Expected story to includ value used in filter in dependant stories");
		}
        }

        [Test]
        public void IdentifiedIn() {
            var retro = SandboxProject.CreateRetrospective("Has a story");
            var story = retro.CreateStory("Retrospective filter");
			story.Save();

			ResetInstance();
			story = Instance.Get.StoryByID(story.ID);
			retro = Instance.Get.RetrospectiveByID(retro.ID);

            var filter = GetFilter();
			filter.IdentifiedIn.Add(retro);

            var results = SandboxProject.GetStories(filter);

			Assert.IsTrue(FindRelated(story, results), "Expected to find story that matched filter.");

            foreach(var result in results) {
				Assert.AreEqual(retro, result.IdentifiedIn);
		}
        }
		

		[Ignore("Find with new data doesn't work in SDK Tests with 9.2 - Full-Text indexes don't update fast enough")]
        [Test]
        public void FindInDescriptionFound() {
            var nameString = Guid.NewGuid().ToString();
            var a = SandboxProject.CreateStory("Has a wierd description");
			a.Description = nameString;
            var b = SandboxProject.CreateStory("Also with funky data");
			b.Description = nameString;

			a.Save();
			b.Save();

            System.Threading.Thread.Sleep(5000);

            var filter = GetFilter();
			filter.Find.SearchString = nameString;
			filter.Find.Fields.Add("Description");

			Assert.AreEqual(2, Instance.Get.Stories(filter).Count);
		}

		[Ignore("Find with new data doesn't work in SDK Tests with 9.2 - Full-Text indexes don't update fast enough")]
        [Test]
        public void FindInDescriptionNotFound() {
            var filter = GetFilter();
			filter.Find.SearchString = Guid.NewGuid().ToString();
			filter.Find.Fields.Add("Description");
			Assert.AreEqual(0, Instance.Get.Stories(filter).Count);
		}

        [Test]
        [Ignore("Server throws NotSupportedException: SimpleLongTextAttributeDefinition.BuildPredicate (Story.Description)")]
        public void FilterByDescription() {
            var weirdString = Guid.NewGuid().ToString();

            var story = EntityFactory.CreateStory("my test story for description", SandboxProject);
            story.Description = weirdString;

			ResetInstance();

            var filter = new StoryFilter();
            filter.Description.Add(weirdString);

            Assert.AreEqual(1, Instance.Get.Stories(filter).Count);
		}

        [Test]
        public void NoProjectAmongStories() {
            var sandboxName = SandboxProject.Name;
			ResetInstance();
            CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.Stories(null)), sandboxName);
		}
	

		[Test]
        public void NoEpicAmongStories() {
            var epic = SandboxProject.CreateEpic("War And Piece");
            ResetInstance();
			CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.Stories(null)), epic.Name);
		}
	}
}
