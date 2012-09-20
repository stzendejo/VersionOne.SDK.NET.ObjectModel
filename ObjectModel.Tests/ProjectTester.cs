using System;
using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests
{
    [TestFixture]
    public class ProjectTester : BaseSDKTester
    {
        private const string RegressionPlanName = "Plan B";
        private const string RegressionPlanDescription = "Description for Test Regression Plan";
        private const string TestSetDescription = "Test set description";

        [Test]
        public void InstanceEnumerable()
        {
            var projects = Instance.Projects;
            CollectionAssert.AreEqual(new string[] { "System (All Projects)" }, DeriveListOfNamesFromAssets(projects));
            CollectionAssert.AreEqual(new string[] { "Scope:0" }, DeriveListOfIdsFromAssets(projects));
            //CollectionAssert.AreEqual(new[] { "System (All Projects)" }, new [] {new EntityToNameTransformer<Project>().Transform(projects[0])});
            //CollectionAssert.AreEqual(new[] { "Scope:0" }, new [] {new EntityToAssetIDTransformer<Project>().Transform(projects[0])});
        }

        [Test]
        public void SimpleProjectAttributes()
        {
            var project = Instance.Get.ProjectByID("Scope:0");
            Assert.AreEqual(new DateTime(2007, 9, 8), project.BeginDate);
            Assert.IsNull(project.EndDate);
            Assert.IsTrue(project.IsActive);
            Assert.IsFalse(project.IsClosed);
            Assert.IsNull(project.ParentProject);
            Assert.IsNotNull(project.Schedule);
            Assert.AreEqual(new Duration(14, Duration.Unit.Days), project.Schedule.IterationLength);
            Assert.AreEqual(new Duration(0, Duration.Unit.Days), project.Schedule.IterationGap);
        }

        [Test]
        public void CreateSubProjectShareSchedule()
        {
            var project = Instance.Get.ProjectByID("Scope:0");
            var targetBeginDate = new DateTime(2007, 1, 1);
            var subProject = project.CreateSubProject("Sub Project A", targetBeginDate);
            var subProjectID = subProject.ID.ToString();

            ResetInstance();

            var cleanRootProject = Instance.Get.ProjectByID("Scope:0");
            var cleanSubProject = Instance.Get.ProjectByID(subProjectID);

            Assert.AreEqual(targetBeginDate, cleanSubProject.BeginDate);
            Assert.IsNull(cleanSubProject.EndDate);
            Assert.AreEqual(cleanRootProject, cleanSubProject.ParentProject);
            Assert.IsNull(cleanSubProject.Schedule);
        }

        [Test]
        public void CreateProjectWithRequiredAttributes()
        {
            const string scheduleDescription = "Test for Schedule creation with required attributes";
            const string projectDescription = "Test for Project creation with required attributes";
            const string name = "CreateAndRetrieveProject";
            var attributes = new Dictionary<string, object> { { "Description", scheduleDescription } };

            var project = Instance.Get.ProjectByID("Scope:0");
            var targetBeginDate = new DateTime(2007, 1, 1);
            var ownSchedule = Instance.Create.Schedule("Test Schedule", new Duration(1, Duration.Unit.Weeks),
                                                            new Duration(2, Duration.Unit.Days), attributes);

            attributes = new Dictionary<string, object> { { "Description", projectDescription } };
            var subProject = project.CreateSubProject(name, targetBeginDate, ownSchedule, attributes);
            var subProjectID = subProject.ID.ToString();

            ResetInstance();

            Project cleanSubProject = Instance.Get.ProjectByID(subProjectID);

            Assert.AreEqual(name, cleanSubProject.Name);
            Assert.AreEqual(projectDescription, cleanSubProject.Description);
            Assert.AreEqual(scheduleDescription, cleanSubProject.Schedule.Description);
        }

        [Test]
        public void CreateSubProjectWithSpecialSymbols()
        {
            const string projectName = "Sub Project A !@#$%^&*()_+~'";
            var project = Instance.Get.ProjectByID("Scope:0");
            var targetBeginDate = new DateTime(2007, 1, 1);
            var subProject = project.CreateSubProject(projectName, targetBeginDate);
            string subProjectID = subProject.ID;

            ResetInstance();

            var cleanSubProject = Instance.Get.ProjectByID(subProjectID);

            Assert.AreEqual(projectName, cleanSubProject.Name);

            ResetInstance();

            var filter = new ProjectFilter();
            filter.Name.Add(projectName);
            var projectLists = Instance.Get.Projects(filter);

            CollectionAssert.Contains(DeriveListOfNamesFromAssets(projectLists), projectName);
        }

        [Test]
        public void CreateSubProjectOwnSchedule()
        {
            var project = Instance.Get.ProjectByID("Scope:0");
            var targetBeginDate = new DateTime(2007, 1, 1);
            var ownSchedule = Instance.Create.Schedule("Own Schedule", new Duration(1, Duration.Unit.Weeks), new Duration(2, Duration.Unit.Days));
            var subProject = project.CreateSubProject("Sub Project B", targetBeginDate, ownSchedule);
            var subProjectID = subProject.ID.ToString();

            ResetInstance();

            var cleanRootProject = Instance.Get.ProjectByID("Scope:0");
            var cleanSubProject = Instance.Get.ProjectByID(subProjectID);

            Assert.AreEqual(targetBeginDate, cleanSubProject.BeginDate);
            Assert.IsNull(cleanSubProject.EndDate);
            Assert.AreEqual(cleanRootProject, cleanSubProject.ParentProject);
            Assert.IsNotNull(cleanSubProject.Schedule);
            Assert.AreEqual(cleanSubProject.Schedule.IterationLength, new Duration(1, Duration.Unit.Weeks));
            Assert.AreEqual(cleanSubProject.Schedule.IterationGap, new Duration(2, Duration.Unit.Days));
        }

        [Test]
        public void AssignMember()
        {
            var project = Instance.Get.ProjectByID("Scope:0");
            var member = Instance.Create.Member("Duude", "dud", Role.TeamMember);

            Assert.IsFalse(project.AssignedMembers.Contains(member));

            project.AssignedMembers.Add(member);

            ResetInstance();
            project = Instance.Get.ProjectByID(project.ID);
            member = Instance.Get.MemberByID(member.ID);

            Assert.IsTrue(project.AssignedMembers.Contains(member));
        }

        #region Sandbox
        private AssetID relationSandboxProjectID;

        private AssetID RelationSandboxProjectID
        {
            get
            {
                if (relationSandboxProjectID == null)
                {
                    var rootProject = Instance.Get.ProjectByID("Scope:0");
                    var sandbox = Instance.Create.Project("SDK Project Relation Tests", rootProject, DateTime.Now, null);

                    relationSandboxProjectID = sandbox.ID;
                }
                return relationSandboxProjectID;
            }
        }

        private Project RelationSandbox
        {
            get { return Instance.Get.ProjectByID(RelationSandboxProjectID); }
        }
        #endregion

        [Test]
        public void AddAndGetDefects()
        {
            var roach = RelationSandbox.CreateDefect("Cockroach");

            ResetInstance();
            roach = Instance.Get.DefectByID(roach.ID);
            var found = FindRelated(roach, RelationSandbox.GetDefects(null));
            Assert.IsTrue(found, "Expected to find defect \"{0}\" in project \"{1}\".", roach.Name, RelationSandbox.Name);
        }

        [Test]
        public void AddAndGetStories()
        {
            var peterPan = RelationSandbox.CreateStory("Peter Pan");

            ResetInstance();
            peterPan = Instance.Get.StoryByID(peterPan.ID);
            bool found = FindRelated(peterPan, RelationSandbox.GetStories(null));
            Assert.IsTrue(found, "Expected to find Story \"{0}\" in Project \"{1}\".", peterPan.Name, RelationSandbox.Name);
        }

        [Test]
        public void AddAndGetIssues()
        {
            var myMother = RelationSandbox.CreateIssue("My Mother");

            ResetInstance();
            myMother = Instance.Get.IssueByID(myMother.ID);
            var found = FindRelated(myMother, RelationSandbox.GetIssues(null));
            Assert.IsTrue(found, "Expected to find Issue \"{0}\" in Project \"{1}\".", myMother.Name, RelationSandbox.Name);
        }

        [Test]
        public void AddAndGetRequests()
        {
            var more = RelationSandbox.CreateRequest("Please Sir, I want some more.");

            ResetInstance();
            more = Instance.Get.RequestByID(more.ID);
            var found = FindRelated(more, RelationSandbox.GetRequests(null));
            Assert.IsTrue(found, "Expected to find Request \"{0}\" in Project \"{1}\".", more.Name, RelationSandbox.Name);
        }

        [Test]
        public void AddAndGetGoals()
        {
            var win = RelationSandbox.CreateGoal("Go.  Fight.  Win!");

            ResetInstance();
            win = Instance.Get.GoalByID(win.ID);
            var found = FindRelated(win, RelationSandbox.GetGoals(null));
            Assert.IsTrue(found, "Expected to find Goal \"{0}\" in Project \"{1}\".", win.Name, RelationSandbox.Name);
        }

        #region Disabled Project Relations
        //[Test] public void GoalsInHierarchy()
        //{
        //    Project parent = Instance.Create.Project("Goal Parent", RelationSandbox, DateTime.Now, null);
        //    Project child = Instance.Create.Project("Goal child", parent, DateTime.Now, null);

        //    Goal inherited = parent.CreateGoal("Inherited");
        //    parent.GoalsTargetedDirectly.Add(inherited);

        //    Goal direct = parent.CreateGoal("Direct");
        //    child.GoalsTargetedDirectly.Add(direct);

        //    ResetInstance();
        //    parent = Instance.Get.ProjectByID(parent.ID);
        //    child = Instance.Get.ProjectByID(child.ID);
        //    inherited = Instance.Get.GoalByID(inherited.ID);
        //    direct = Instance.Get.GoalByID(direct.ID);

        //    Assert.IsTrue(parent.GoalsTargetedDirectly.Contains(inherited), "Expect directly targeted goal");
        //    Assert.IsTrue(child.GoalsTargetedDirectly.Contains(direct), "Expect directly targeted goal");

        //    Assert.IsTrue(FindRelated(inherited, child.GoalsTargetedViaParent), "Expect goal targeted by parent.");

        //    Assert.IsTrue(FindRelated(inherited, parent.GoalsAvailable), "Expect goal in parent to be available to both parent and child.");

        //    Assert.IsTrue(FindRelated(inherited, child.GoalsAvailable), "Expect goal in parent to be available to both parent and child.");
        //    Assert.IsTrue(FindRelated(direct, child.GoalsAvailable), "Expect goal in child to be available to child.");

        //    Assert.IsTrue(FindRelated(parent, inherited.TargetedBy), "Expect parent to target \"inherited \"goal.");
        //    Assert.IsTrue(FindRelated(child, direct.TargetedBy), "Expect parent to target \"direct\"goal.");
        //}

        //[Test] public void EnumerateAvailableThemes()
        //{
        //    IEnumerable<Theme> availableThemes = Instance.Get.ProjectByID("Scope:1013").ThemesAvailable;
        //    string[] expected = new string[] { "Theme:1055", "Theme:1056", "Theme:1057","Theme:1058", "Theme:1059", "Theme:1060","Theme:1061","Theme:1062" };
        //    ListAssert.AreEqualIgnoringOrder(expected, availableThemes, new EntityToAssetIDTransformer<Theme>());
        //}

        //[Test] public void EmptyEnumerateAvaiableThemes()
        //{
        //    IEnumerable<Theme> availableThemes = Instance.Get.ProjectByID("Scope:1011").ThemesAvailable;
        //    ListAssert.AreEqual(new string[] {},availableThemes,new EntityToAssetIDTransformer<Theme>());
        //} 
        #endregion

        [Test]
        public void GetIterations()
        {
            var iterations = Instance.Get.ProjectByID("Scope:1012").GetIterations(null);
            var expected = new[] { "Timebox:1021", "Timebox:1022", "Timebox:1023", "Timebox:1024", "Timebox:1025", "Timebox:1026", "Timebox:1027", "Timebox:1028" };
            CollectionAssert.AreEquivalent(expected, DeriveListOfIdsFromAssets(iterations));
        }

        [Test]
        public void GetIterationsEmpty()
        {
            var iterations = Instance.Get.ProjectByID("Scope:1011").GetIterations(null);
            CollectionAssert.AreEquivalent(new string[] { }, DeriveListOfIdsFromAssets(iterations));
        }

        [Test]
        public void GetIterationsDefinedInParentProject()
        {
            //Children projects no longer inherit their parent's schedule, so the child project no longer has iterations from above
            var parentProject = Instance.Get.ProjectByID("Scope:1012");
            var childProject = parentProject.CreateSubProject("Sub Project with No Schedule", DateTime.Now);
            var iterations = childProject.GetIterations(null);
            CollectionAssert.AreEquivalent(new string[] { }, DeriveListOfIdsFromAssets(iterations));
        }

        [Test]
        public void SubProjects()
        {
            Assert.IsTrue(Instance.Get.ProjectByID("Scope:0").GetThisAndAllChildProjects().Count > 1);
        }

        [Test]
        public void GetProjectByName()
        {
            var projectName = Guid.NewGuid().ToString();
            var mine = Instance.Create.Project(projectName, SandboxProject.ID, DateTime.Now, null);

            ResetInstance();
            mine = Instance.Get.ProjectByID(mine.ID);

            Assert.AreEqual(mine.ID, Instance.Get.ProjectByName(projectName).ID);
        }

        [Test]
        public void GetProjectByNameWithSpecialSybmols()
        {
            var projectName = "Sub Project A !@#$%^&*()_+~'" + Guid.NewGuid().ToString();
            var mine = Instance.Create.Project(projectName, SandboxProject.ID, DateTime.Now, null);

            ResetInstance();
            mine = Instance.Get.ProjectByID(mine.ID);

            Assert.AreEqual(mine.ID, Instance.Get.ProjectByName(projectName).ID);
        }

        [Test]
        public void GetRegressionPlanWithNullFilterTest()
        {
            var plans = SandboxProject.GetRegressionPlans(null);
            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var updatedPlans = SandboxProject.GetRegressionPlans(null);

            Assert.AreEqual(1, updatedPlans.Count - plans.Count);
            CollectionAssert.Contains(updatedPlans, plan);
        }

        [Test]
        public void GetRegressionPlanFromMismatchingProjectTest()
        {
            var project = EntityFactory.Create(() => Instance.Create.Project("other", SandboxProject.ParentProject,
                DateTime.Now, SandboxSchedule));

            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var plans = project.GetRegressionPlans(null);

            Assert.AreEqual(0, plans.Count);
            CollectionAssert.DoesNotContain(plans, plan);
        }

        [Test]
        public void GetRegressionPlanWithValuableFilterTest()
        {
            var owner = EntityFactory.CreateMember("Paul");
            var nobody = EntityFactory.CreateMember("Mike");

            var attributes = new Dictionary<string, object> {
                {"Owner", owner.ID.Token}
            };

            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject, attributes);

            var filter = new RegressionPlanFilter();
            filter.Owners.Add(owner);
            var plans = SandboxProject.GetRegressionPlans(filter);
            Assert.AreEqual(1, plans.Count);
            CollectionAssert.Contains(plans, plan);

            filter = new RegressionPlanFilter();
            filter.Project.Add(SandboxProject);
            filter.Owners.Add(nobody);
            plans = SandboxProject.GetRegressionPlans(filter);
            Assert.AreEqual(0, plans.Count);
        }

        [Test]
        public void GetRegressionPlanFromChildProjectTest()
        {
            var childProject = EntityFactory.Create(() => SandboxProject.CreateSubProject("child", DateTime.Now));

            var plan = EntityFactory.CreateRegressionPlan(RegressionPlanName, childProject);

            var plans = SandboxProject.GetRegressionPlans(null, false);
            CollectionAssert.DoesNotContain(plans, plan);

            plans = childProject.GetRegressionPlans(null);
            CollectionAssert.Contains(plans, plan);

            plans = SandboxProject.GetRegressionPlans(null, true);
            CollectionAssert.Contains(plans, plan);
        }

        [Test]
        public void CreateRegressionPlanTest()
        {
            var regressionPlan = EntityFactory.Create(() => SandboxProject.CreateRegressionPlan(RegressionPlanName));
            Assert.AreEqual(SandboxProject, regressionPlan.Project);
            Assert.AreEqual(RegressionPlanName, regressionPlan.Name);
        }

        [Test]
        public void CreateRegressionPlanWithAttributesTest()
        {
            var member = EntityFactory.CreateMember("test user");
            var attributes = new Dictionary<string, object> {
                {"Description", RegressionPlanDescription},
                {"Owner", Oid.FromToken(member.ID, Instance.ApiClient.MetaModel)}
            };
            var regressionPlan = EntityFactory.Create(() => SandboxProject.CreateRegressionPlan(RegressionPlanName, attributes));
            Assert.AreEqual(SandboxProject, regressionPlan.Project);
            Assert.AreEqual(RegressionPlanName, regressionPlan.Name);
            Assert.AreEqual(RegressionPlanDescription, regressionPlan.Description);
            Assert.AreEqual(member, regressionPlan.Owner);
        }

        [Test]
        public void CreateTestSetTest()
        {
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite("suite1", regressionPlan);

            var attributes = new Dictionary<string, object> {
                {"Description", TestSetDescription}
            };
            var testSet = SandboxProject.CreateTestSet("my test set", regressionSuite, attributes);

            ResetInstance();

            var testSets = new List<TestSet>(regressionSuite.GetTestSets(null));
            CollectionAssert.Contains(testSets, testSet);
            Assert.IsTrue(testSets[0].Project.Equals(SandboxProject));
        }

        [Test]
        public void RetrieveTestSetTest()
        {
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite("suite1", regressionPlan);

            var beforeCreation = SandboxProject.GetTestSets(null);
            var testSet = SandboxProject.CreateTestSet("my test set", regressionSuite);

            ResetInstance();

            var afterCreation = new List<TestSet>(SandboxProject.GetTestSets(null));
            CollectionAssert.Contains(afterCreation, testSet);
            CollectionAssert.DoesNotContain(beforeCreation, testSet);
            Assert.IsTrue(afterCreation.Count - beforeCreation.Count == 1);
        }

        [Test]
        public void RetrieveTestSetFromChildProjectTest()
        {
            var subProject = EntityFactory.Create(() => SandboxProject.CreateSubProject("subproject", DateTime.Now));
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, subProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite("suite1", regressionPlan);

            var testSet = EntityFactory.Create(() => subProject.CreateTestSet("my test set", regressionSuite));

            ResetInstance();

            var testSetsFromSandboxProject = SandboxProject.GetTestSets(null);
            var testSetsFromChildProject = subProject.GetTestSets(null);
            var testSetsFromSandboxProjectAndChildren = SandboxProject.GetTestSets(null, true);

            CollectionAssert.Contains(testSetsFromSandboxProjectAndChildren, testSet);
            CollectionAssert.Contains(testSetsFromChildProject, testSet);
            CollectionAssert.DoesNotContain(testSetsFromSandboxProject, testSet);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateTestSetWithInvalidParameters()
        {
            var subProject = EntityFactory.Create(() => SandboxProject.CreateSubProject("subproject", DateTime.Now));
            var regressionPlan = EntityFactory.CreateRegressionPlan(RegressionPlanName, SandboxProject);
            var regressionSuite = EntityFactory.CreateRegressionSuite("suite1", regressionPlan);

            EntityFactory.Create(() => subProject.CreateTestSet("my test set", regressionSuite));
        }

        [Test]
        public void CreateRegressionTestTest()
        {
            const string regressionTestName = "Regression Test from Project";

            var subProject = EntityFactory.Create(() => SandboxProject.CreateSubProject("subproject", DateTime.Now));
            var regressionTest = subProject.CreateRegressionTest(regressionTestName);
            EntityFactory.RegisterForDisposal(regressionTest);

            ResetInstance();

            var regressionTestNew = Instance.Get.RegressionTestByID(regressionTest.ID);

            Assert.AreEqual(regressionTestName, regressionTestNew.Name);
            Assert.AreEqual(subProject, regressionTestNew.Project);
        }

        [Test]
        public void CreateRegressionTestWithAttributesTest()
        {
            const string regressionTestName = "Regression Test from Project";
            const string regressionTestDescription = "Regression Test from Project regressionTestDescription";
            const string regressionTestTags = "test tags regression";

            var attributes = new Dictionary<string, object> {
                {"Description", regressionTestDescription}, 
                {"Tags", regressionTestTags}
            };

            var subProject = EntityFactory.Create(() => SandboxProject.CreateSubProject("subproject", DateTime.Now));
            var regressionTest = subProject.CreateRegressionTest(regressionTestName, attributes);
            EntityFactory.RegisterForDisposal(regressionTest);

            ResetInstance();

            var regressionTestNew = Instance.Get.RegressionTestByID(regressionTest.ID);

            Assert.AreEqual(regressionTestName, regressionTestNew.Name);
            Assert.AreEqual(0, regressionTestNew.Owners.Count);
            Assert.AreEqual(regressionTestTags, regressionTestNew.Tags);
            Assert.AreEqual(regressionTestDescription, regressionTestNew.Description);
        }
    }
}
