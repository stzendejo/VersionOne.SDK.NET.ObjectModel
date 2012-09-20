using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class StoryTester : BaseSDKTester {
        [Test]
        public void StoryOwner() {
            var story = SandboxProject.CreateStory("StoryOwner");

            var owner = Instance.Create.Member("Dude", "dud", Role.ProjectLead);
            SandboxProject.AssignedMembers.Add(owner);
            SandboxProject.Save();

            story.Owners.Add(owner);
            story.Save();

            ResetInstance();
            story = Instance.Get.StoryByID(story.ID);
            owner = Instance.Get.MemberByID(owner.ID);

            Assert.IsTrue(FindRelated(owner, story.Owners));
            Assert.IsTrue(FindRelated(story, owner.GetOwnedPrimaryWorkitems(null)));

            story.Delete();
            owner.Delete();
        }

        [Test]
        public void CreateAndRetrieveStory() {
            const string name = "CreateAndRetrieveStory";
            var id = Instance.Create.Story(name, SandboxProject).ID;
            ResetInstance();

            var story = Instance.Get.StoryByID(id);
            Assert.AreEqual(story.Name, name);

            story.Delete();
        }

        [Test]
        public void CreateStroyWithCustomFields() {
            Instance.ValidationEnabled = true;

            const string description = "Test for Story creation with required attributes";
            const string name = "CreateAndRetrieveStory";
            var attributes = new Dictionary<string, object> {
                {"Description", description}, 
                {"Custom_ShoeSize", 11.5}
            };

            var story = Instance.Create.Story(name, SandboxProject, attributes);

            story.Save();

            story = Instance.Get.StoryByID(story.ID);

            var shoeSize = story.CustomField.GetNumeric("ShoeSize");
            Assert.AreEqual(name, story.Name);
            Assert.AreEqual(description, story.Description);
            Assert.AreEqual(11.5, shoeSize);

            story.Delete();

            Instance.ValidationEnabled = false;
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void StoryCreateValidation() {
            Instance.Create.Story(null, SandboxProject);
            Assert.IsTrue(false, "Save Method should throw an EntityValidationException");
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void UpdateCreateValidation() {
            var story = Instance.Create.Story("NotEmptyName", SandboxProject);
            story.Name = null;
        }

        [Test]
        public void CacheUpdateValidation() {
            Instance.ValidationEnabled = true;
            const string name = "NotEmptyStoryName";
            const string description = "Test for Story creation with required attributes";

            var story = Instance.Create.Story(name, SandboxProject);

            ResetInstance();
            Instance.ValidationEnabled = true;

            story = Instance.Get.StoryByID(story.ID);
            story.Description = description;
            story.Save();

            ResetInstance();

            story = Instance.Get.StoryByID(story.ID);
            Assert.AreEqual(description, story.Description);
            story.Delete();
            Instance.ValidationEnabled = false;
        }

        [Test]
        public void StoryBenefits() {
            const string name = "StoryBenefits";
            const string benefits = "It's good for you!";

            var story = Instance.Create.Story(name, SandboxProject);
            story.Benefits = benefits;
            var id = story.ID;

            story.Save();
            ResetInstance();

            story = Instance.Get.StoryByID(id);
            Assert.AreEqual(benefits, story.Benefits);

            story.Delete();
        }

        [Test]
        public void StoryOrder() {
            var story1 = SandboxProject.CreateStory("StoryOrder 1");
            var story2 = SandboxProject.CreateStory("StoryOrder 2");

            var id1 = story1.ID;
            var id2 = story2.ID;

            story1.RankOrder.SetBelow(story2);

            Assert.IsTrue(story1.RankOrder.IsBelow(story2));
            Assert.IsTrue(story2.RankOrder.IsAbove(story1));

            ResetInstance();

            story1 = Instance.Get.StoryByID(id1);
            story2 = Instance.Get.StoryByID(id2);

            story1.RankOrder.SetAbove(story2);

            Assert.IsTrue(story1.RankOrder.IsAbove(story2));
            Assert.IsTrue(story2.RankOrder.IsBelow(story1));

            story1.Delete();
            story2.Delete();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void Actuals() {
            Story story = null;

            try {
                story = SandboxProject.CreateStory("Actuals 1");
                var actual = story.CreateEffort(5);

                string storyID = story.ID;
                string actualID = actual.ID;

                ResetInstance();

                var newStory = Instance.Get.StoryByID(storyID);
                var newActual = Instance.Get.EffortByID(actualID);

                Assert.AreEqual(5, newActual.Value);
                Assert.AreEqual(newStory, newActual.Workitem);
            } finally {
                if(story != null) {
                    story.Delete();
                }
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ActualsWithDateAndMember() {
            Story story = null;

            try {
                var member = Instance.Get.MemberByID("Member:20");
                story = SandboxProject.CreateStory("ActualsWithDateAndMember 2");
                var actual = story.CreateEffort(member, new DateTime(2007, 1, 1), 10);

                string storyID = story.ID;
                string actualID = actual.ID;

                ResetInstance();

                var newStory = Instance.Get.StoryByID(storyID);
                var newActual = Instance.Get.EffortByID(actualID);
                var newMember = Instance.Get.MemberByID("Member:20");

                Assert.AreEqual(10, newActual.Value);
                Assert.AreEqual(newStory, newActual.Workitem);
                Assert.AreEqual(new DateTime(2007, 1, 1), newActual.Date);
                Assert.AreEqual(newMember, newActual.Member);
            } finally {
                if(story != null) {
                    story.Delete();
                }
            }
        }

        [Test]
        public void GetByDisplayID() {
            const string defectName = "GetByDisplayIDTest";
            var displayID = SandboxProject.CreateStory(defectName).DisplayID;

            ResetInstance();

            Assert.AreEqual(defectName, Instance.Get.StoryByDisplayID(displayID).Name);
            Instance.Get.StoryByDisplayID(displayID).Delete();
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelDetailEstimate() {
            Story story = null;

            try {
                // The V1SDKTests system is assumed to be configured for "Story:Off"
                story = SandboxProject.CreateStory("HonorTrackingLevelDetailEstimate");
                story.DetailEstimate = 10.0; //Should throw
            } finally {
                if(story != null) {
                    story.Delete();
                }
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelToDo() {
            Story story = null;

            try {
                // The V1SDKTests system is assumed to be configured for "Story:Off"
                story = SandboxProject.CreateStory("HonorTrackingLevelToDo");
                story.ToDo = 10.0; //Should throw
            } finally {
                if(story != null) {
                    story.Delete();
                }
            }
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void HonorTrackingLevelEffort() {
            Story story = null;

            try {
                // The V1SDKTests system is assumed to be configured for "Defect:On"
                story = SandboxProject.CreateStory("HonorTrackingLevelEffort");
                story.CreateEffort(10.0); // should throw
            } finally {
                if(story != null) {
                    story.Delete();
                }
            }
        }

        [Test]
        public void EmptyAttributes() {
            const string name = "EmptyAttributesTest";

            var story = Instance.Create.Story(name, SandboxProject);
            Assert.AreEqual(null, story.Reference);
            Assert.AreEqual(null, story.Estimate);
            Assert.AreEqual(null, story.Description);

            story.Reference = "ref";
            story.Estimate = 5;
            story.Description = "test";
            story.Save();

            var id = story.ID;
            ResetInstance();

            story = Instance.Get.StoryByID(id);
            story.Description = "";
            story.Estimate = null;
            story.Reference = null;
            story.Save();

            ResetInstance();

            story = Instance.Get.StoryByID(id);

            Assert.AreEqual(null, story.Reference);
            Assert.AreEqual(null, story.Estimate);
            Assert.AreEqual(null, story.Description); // in story.Description still the "test" value

            story.Delete();
        }

        [Test]
        public void CloseAndReactivateWorkitem() {
            const string name = "CloseAndReactivateWorkitemTest";
            SandboxProject.CreateStory(name);

            var filter = new StoryFilter();
            filter.Name.Add(name);
            var workitems = SandboxProject.GetPrimaryWorkitems(filter);
            Assert.AreEqual(1, workitems.Count);
            var story = First(workitems);

            Assert.IsFalse(story.CanReactivate);
            Assert.IsTrue(story.CanClose);
            Assert.IsFalse(story.IsClosed);
            Assert.IsTrue(story.IsActive);

            story.Close();
            Assert.IsTrue(story.CanReactivate);
            Assert.IsFalse(story.CanClose);
            Assert.IsTrue(story.IsClosed);
            Assert.IsFalse(story.IsActive);

            story.Reactivate();
            Assert.IsFalse(story.CanReactivate);
            Assert.IsTrue(story.CanClose);
            Assert.IsFalse(story.IsClosed);
            Assert.IsTrue(story.IsActive);

            story.Delete();
        }

        /// <summary>
        /// Test for D-01078 (CanDelete returns True on closed assets)
        /// </summary>
        [Test]
        public void CanDelete() {
            const string name = "CanDeleteTest";
            SandboxProject.CreateStory(name);
            var filter = new StoryFilter();
            filter.Name.Add(name);
            var workitems = SandboxProject.GetPrimaryWorkitems(filter);
            Assert.AreEqual(1, workitems.Count);
            var story = First(workitems);

            Assert.IsTrue(story.CanDelete);

            story.Close();
            Assert.IsFalse(story.CanDelete);

            story.Reactivate();
            Assert.IsTrue(story.CanDelete);

            story.Delete();
        }

        /// <summary>
        /// Test for D-01143 (Effort Double Entry)
        /// </summary>
        [Test]
        public void DoubleEffortEntry() {
            const string storyName = "DoubleEffortEntryStoryTest";
            const string taskName = "DoubleEffortEntryTaskTest";

            if(!Instance.ApiClient.V1Config.EffortTracking) {
                Assert.Fail("Effort tracking is not enabled.");
            }

            if(Instance.ApiClient.V1Config.StoryTrackingLevel == APIClient.TrackingLevel.On) {
                Assert.Fail("Task effort tracking is not enabled.");
            }

            var member = Instance.Get.MemberByID("Member:20");
            var story = SandboxProject.CreateStory(storyName);
            var task = story.CreateTask(taskName);
            task.CreateEffort(member, DateTime.Now, 1.0);

            var effortRecords = task.GetEffortRecords(null);

            var actual = effortRecords.Aggregate(0.0, (current, effortRecord) => current + effortRecord.Value);

            Assert.AreEqual(1.0, actual);

            story.Delete();
        }

        [Test] 
        public void CanBreakdown() {
            var story = EntityFactory.CreateStory("Story", SandboxProject);
            Assert.IsTrue(story.CanBreakdown());
        }

        [Test]
        public void CannotBreakdown() {
            var story = EntityFactory.CreateStory("Story", SandboxProject);
            story.Close();           
            Assert.IsFalse(story.CanBreakdown());
        }
    }
}