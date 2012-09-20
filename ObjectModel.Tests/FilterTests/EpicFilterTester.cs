using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
	[TestFixture]
    public class EpicFilterTester : BaseSDKTester {
        private Project sandboxProject;
        private Member andre;
        private Epic epic1;
        private Epic epic2;

        private EpicFilter GetFilter() {
            var filter = new EpicFilter();
            filter.Project.Add(sandboxProject);
			return filter;
		}

        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

		[SetUp]
        public void CreateAssets() {
            NewSandboxProject();
            sandboxProject = SandboxProject;

            andre = EntityFactory.CreateMember("Member 1");

            epic1 = EntityFactory.CreateEpic("Epic 1", sandboxProject);
            epic2 = EntityFactory.CreateEpic("Epic 2", sandboxProject);

            var childEpic = EntityFactory.CreateChildEpic(epic2);
                childEpic.Name = "Son of an Epic";
                childEpic.Save();

            epic1.Owners.Add(andre);
				epic1.Source.CurrentValue = "Customer";
            epic1.Risk = 5;
            epic1.Value = 20;
            epic1.Swag = 20;
            epic1.Type.CurrentValue = "New Feature";
				epic1.Save();

            epic2.Priority.CurrentValue = "High";
            epic2.Source.CurrentValue = "Sales";
            epic2.Reference = "Find Me";
            epic2.Risk = 10;
            epic2.Value = 80;
            epic2.Swag = 30;
            epic2.Save();

            var story1 = EntityFactory.Create(epic1.GenerateChildStory);
				story1.Reference = "Find Me";
            story1.Customer = andre;
				story1.Save();
        }

        [Test]
        public void Source() {
            var filter = GetFilter();
            filter.Source.Add("Sales");
            Assert.AreEqual(1, Instance.Get.Epics(filter).Count);
		}

        [Test]
        public void Reference() {
            var filter = GetFilter();
            filter.Reference.Add("Find Me");
			Assert.AreEqual(1, Instance.Get.Epics(filter).Count);
		}

        [Test]
        public void Type() {
            var filter = GetFilter();
            filter.Type.Add("New Feature");
			Assert.AreEqual(1, Instance.Get.Epics(filter).Count);
		}

        [Test]
        public void Risk() {
            var filter = GetFilter();
            filter.Risk.AddTerm(FilterTerm.Operator.Equal, 5.0);
            var epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Risk.Range(1, 4);
            Assert.AreEqual(0, Instance.Get.Epics(filter).Count);

            filter = GetFilter();
            filter.Risk.Range(6, 9);
            Assert.AreEqual(0, Instance.Get.Epics(filter).Count);

            filter = GetFilter();
            filter.Risk.Range(1, 5.5);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Risk.Range(1, 10);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(2, epics.Count);
            CollectionAssert.Contains(epics, epic1);
            CollectionAssert.Contains(epics, epic2);
		}

        [Test]
        public void Value() {
            var filter = GetFilter();
            filter.Value.AddTerm(FilterTerm.Operator.Equal, 20);
            var epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Value.Range(1, 19);
            Assert.AreEqual(0, Instance.Get.Epics(filter).Count);

            filter = GetFilter();
            filter.Value.AddTerm(FilterTerm.Operator.GreaterThan, 20.05);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic2);

            filter = GetFilter();
            filter.Value.Range(15, 25);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Value.Range(1, 100);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(2, epics.Count);
            CollectionAssert.Contains(epics, epic1);
            CollectionAssert.Contains(epics, epic2);
		}

        [Test]
        public void Swag() {
            var filter = GetFilter();
            filter.Swag.AddTerm(FilterTerm.Operator.Equal, 20);
            var epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Swag.Range(1, 19);
            Assert.AreEqual(0, Instance.Get.Epics(filter).Count);

            filter = GetFilter();
            filter.Swag.AddTerm(FilterTerm.Operator.GreaterThan, 20.05);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic2);

            filter = GetFilter();
            filter.Swag.Range(15, 25);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(1, epics.Count);
            CollectionAssert.Contains(epics, epic1);

            filter = GetFilter();
            filter.Swag.Range(1, 100);
            epics = Instance.Get.Epics(filter);
            Assert.AreEqual(2, epics.Count);
            CollectionAssert.Contains(epics, epic1);
            CollectionAssert.Contains(epics, epic2);
		}

        [Test]
        public void Priority() {
            var filter = GetFilter();
			filter.Priority.Add("High");
			Assert.AreEqual(1, Instance.Get.Epics(filter).Count);
		}

        [Test]
        public void GetChildEpicsWithNullFilter() {
            var epic = Instance.Get.EpicByID(epic2.ID);
            Assert.AreEqual(1, epic.GetChildEpics(null).Count);
        }

        [Test]
        public void NoStoryAmongEpics() {
            var decoy = EntityFactory.CreateStory("Decoy", SandboxProject);
            ResetInstance();
        	CollectionAssert.DoesNotContain(
        		Instance.Get.Epics(null),
        		DeriveListOfNamesFromAssets(Instance.Get.Epics(null)));
        }

        [Test]
        public void Owners() {
            var filter = GetFilter();
            filter.Owners.Add(andre);
            Assert.AreEqual(1, Instance.Get.Epics(filter).Count);
        }

        [Test]
        public void ActiveState() {
            var epic = EntityFactory.CreateEpic("Some new Epic", sandboxProject);
            
            var filter = GetFilter();
            filter.State.Add(State.Active);
            var epics = Instance.Get.Epics(filter);

            CollectionAssert.Contains(epics, epic);
        }

        [Test]
        public void ClosedState() {
            var epic = EntityFactory.CreateEpic("Some new Epic", sandboxProject);
            var anotherEpic = EntityFactory.CreateEpic("Another Epic", sandboxProject);
            anotherEpic.Close();

            var filter = GetFilter();
            filter.State.Add(State.Closed);
            var epics = Instance.Get.Epics(filter);

            CollectionAssert.DoesNotContain(epics, epic);
            CollectionAssert.Contains(epics, anotherEpic);
        }

        [Test]
        public void BothStates() {
            var epic = EntityFactory.CreateEpic("Some new Epic", sandboxProject);
            var anotherEpic = EntityFactory.CreateEpic("Another Epic", sandboxProject);
            anotherEpic.Close();

            var filter = GetFilter();
            var epics = Instance.Get.Epics(filter);

            CollectionAssert.Contains(epics, epic);
            CollectionAssert.Contains(epics, anotherEpic);
        }
        
        [Test]
        public void DeletedState() {
            var epic = EntityFactory.CreateEpic("Some new Epic", sandboxProject);
            epic.Delete();
            var filter = GetFilter();

            var epics = Instance.Get.Epics(filter);

            CollectionAssert.DoesNotContain(epics, epic);
		}
	}
}
