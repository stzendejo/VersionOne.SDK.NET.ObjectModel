using System;

using NUnit.Framework;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class TotalTester : BaseSDKTester {
        protected override Project CreateSandboxProject(Project rootProject) {
            return EntityFactory.CreateProjectWithSchedule(SandboxName, rootProject);
        }

        [SetUp]
        public void Setup() {
            NewSandboxProject();
            NewSandboxIteration();
            NewSandboxTeam();
            NewSandboxMember();
        }

        [Test]
        public void ProjectTotalEstimateSliceByIteration() {
            var story1 = EntityFactory.CreateStory("Story 1", SandboxProject);
            var story2 = EntityFactory.CreateStory("Story 2", SandboxProject);
            var iteration = EntityFactory.Create(() => SandboxProject.CreateIteration());
            iteration.Name = "Test Iteration";
            iteration.Save();

            story1.Estimate = 1.0;
            story2.Estimate = 2.0;

            var inIteration1 = CreateStory("In 1", SandboxProject, iteration);
            var inIteration2 = CreateStory("In 2", SandboxProject, iteration);

            Assert.AreEqual(SandboxProject, inIteration1.Project);
            Assert.AreEqual(SandboxProject, inIteration2.Project);

            inIteration1.Estimate = 10.0;
            inIteration2.Estimate = 5.0;

            story1.Save();
            story2.Save();
            inIteration1.Save();
            inIteration2.Save();

            var filter = new PrimaryWorkitemFilter();
            filter.Iteration.Add(iteration);

            Assert.AreEqual(15.0, SandboxProject.GetTotalEstimate(filter));
        }

        #region Single Level Project Totals
        [Test]
        public void ProjectTotalDone() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            storyA.CreateTask("Task 1").CreateEffort(5);
            defectB.CreateEffort(10);

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(15, SandboxProject.GetTotalDone(null));
            Assert.AreEqual(15, SandboxProject.GetTotalDone(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalDone(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalDone(new StoryFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalDone(new DefectFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalDone(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalDone(new TaskFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalDone(new TestFilter()));
        }

        [Test]
        public void ProjectTotalToDo() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var task1 = storyA.CreateTask("Task 1");
            task1.ToDo = 5;
            defectB.ToDo = 10;

            task1.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxProject.GetTotalToDo(null));
            Assert.AreEqual(15, SandboxProject.GetTotalToDo(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalToDo(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalToDo(new StoryFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalToDo(new DefectFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalToDo(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalToDo(new TaskFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalToDo(new TestFilter()));
        }

        [Test]
        public void ProjectTotalDetailEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var task1 = storyA.CreateTask("Task 1");
            task1.DetailEstimate = 5;
            defectB.DetailEstimate = 10;

            task1.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxProject.GetTotalDetailEstimate(null));
            Assert.AreEqual(15, SandboxProject.GetTotalDetailEstimate(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalDetailEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalDetailEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalDetailEstimate(new DefectFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalDetailEstimate(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalDetailEstimate(new TaskFilter()));
            Assert.AreEqual(null, SandboxProject.GetTotalDetailEstimate(new TestFilter()));
        }

        [Test]
        public void ProjectTotalEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyA.Save();
            defectB.Save();

            //Epic rogue = SandboxProject.CreateEpic("Rogue");
            //rogue.Estimate = 13.37;
            //rogue.Save();

            Assert.AreEqual(15, SandboxProject.GetTotalEstimate(null));
            Assert.AreEqual(15, SandboxProject.GetTotalEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxProject.GetTotalEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxProject.GetTotalEstimate(new DefectFilter()));
        }
        #endregion

        #region MultiLevel Project Totals
        [Test]
        public void ProjectTotalDoneAndDown() {
            var childProject = EntityFactory.CreateSubProject("Son of " + SandboxName, DateTime.Now, null, SandboxProject);
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var storyC = EntityFactory.CreateStory("Story C", childProject);
            var defectD = EntityFactory.CreateDefect("Defect D", childProject);

            storyA.CreateTask("Task 1").CreateEffort(5);
            defectB.CreateEffort(10);
            storyC.CreateTest("Test 1").CreateEffort(2);
            defectD.CreateEffort(1);

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(18, SandboxProject.GetTotalDone(null, true));
            Assert.AreEqual(18, SandboxProject.GetTotalDone(new WorkitemFilter(), true));
            Assert.AreEqual(11, SandboxProject.GetTotalDone(new PrimaryWorkitemFilter(), true));
            Assert.AreEqual(null, SandboxProject.GetTotalDone(new StoryFilter(), true));
            Assert.AreEqual(11, SandboxProject.GetTotalDone(new DefectFilter(), true));
            Assert.AreEqual(7, SandboxProject.GetTotalDone(new SecondaryWorkitemFilter(), true));
            Assert.AreEqual(5, SandboxProject.GetTotalDone(new TaskFilter(), true));
            Assert.AreEqual(2, SandboxProject.GetTotalDone(new TestFilter(), true));
        }

        [Test]
        public void ProjectTotalToDoAndDown() {
            var childProject = EntityFactory.CreateSubProject("Son of " + SandboxName, DateTime.Now, null, SandboxProject);
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var storyC = EntityFactory.CreateStory("Story C", childProject);
            var defectD = EntityFactory.CreateDefect("Defect D", childProject);

            var task1 = storyA.CreateTask("Task 1");
            task1.ToDo = 5;
            defectB.ToDo = 10;

            var test1 = storyC.CreateTest("Test 1");
            test1.ToDo = 2;
            defectD.ToDo = 3;

            task1.Save();
            defectB.Save();
            test1.Save();
            defectD.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(20, SandboxProject.GetTotalToDo(null, true));
            Assert.AreEqual(20, SandboxProject.GetTotalToDo(new WorkitemFilter(), true));
            Assert.AreEqual(13, SandboxProject.GetTotalToDo(new PrimaryWorkitemFilter(), true));
            Assert.AreEqual(null, SandboxProject.GetTotalToDo(new StoryFilter(), true));
            Assert.AreEqual(13, SandboxProject.GetTotalToDo(new DefectFilter(), true));
            Assert.AreEqual(7, SandboxProject.GetTotalToDo(new SecondaryWorkitemFilter(), true));
            Assert.AreEqual(5, SandboxProject.GetTotalToDo(new TaskFilter(), true));
            Assert.AreEqual(2, SandboxProject.GetTotalToDo(new TestFilter(), true));
        }

        [Test]
        public void ProjectTotalDetailEstimateAndDown() {
            var childProject = EntityFactory.CreateSubProject("Son of " + SandboxName, DateTime.Now, null, SandboxProject);
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var storyC = EntityFactory.CreateStory("Story C", childProject);
            var defectD = EntityFactory.CreateDefect("Defect D", childProject);

            var task1 = storyA.CreateTask("Task 1");
            task1.DetailEstimate = 5;
            defectB.DetailEstimate = 10;

            var test1 = storyC.CreateTest("Test 1");
            test1.DetailEstimate = 2;
            defectD.DetailEstimate = 3;

            task1.Save();
            defectB.Save();
            test1.Save();
            defectD.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(20, SandboxProject.GetTotalDetailEstimate(null, true));
            Assert.AreEqual(20, SandboxProject.GetTotalDetailEstimate(new WorkitemFilter(), true));
            Assert.AreEqual(13, SandboxProject.GetTotalDetailEstimate(new PrimaryWorkitemFilter(), true));
            Assert.AreEqual(null, SandboxProject.GetTotalDetailEstimate(new StoryFilter(), true));
            Assert.AreEqual(13, SandboxProject.GetTotalDetailEstimate(new DefectFilter(), true));
            Assert.AreEqual(7, SandboxProject.GetTotalDetailEstimate(new SecondaryWorkitemFilter(), true));
            Assert.AreEqual(5, SandboxProject.GetTotalDetailEstimate(new TaskFilter(), true));
            Assert.AreEqual(2, SandboxProject.GetTotalDetailEstimate(new TestFilter(), true));
        }

        [Test]
        public void ProjectTotalEstimateAndDown() {
            var childProject = EntityFactory.CreateSubProject("Son of " + SandboxName, DateTime.Now, null, SandboxProject);
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var storyC = EntityFactory.CreateStory("Story C", childProject);
            var defectD = EntityFactory.CreateDefect("Defect D", childProject);

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyC.Estimate = 2;
            defectD.Estimate = 3;

            storyA.Save();
            defectB.Save();
            storyC.Save();
            defectD.Save();

            //Epic rogue = SandboxProject.CreateEpic("Rogue");
            //rogue.Estimate = 13.37;
            //rogue.Save();

            Assert.AreEqual(20, SandboxProject.GetTotalEstimate(null, true));
            Assert.AreEqual(20, SandboxProject.GetTotalEstimate(new PrimaryWorkitemFilter(), true));
            Assert.AreEqual(7, SandboxProject.GetTotalEstimate(new StoryFilter(), true));
            Assert.AreEqual(13, SandboxProject.GetTotalEstimate(new DefectFilter(), true));
        }

        [Test]
        public void ProjectTotalEstimateWithClosedChild() {
            var childProject = EntityFactory.CreateSubProject("Son of " + SandboxName, DateTime.Now, null, SandboxProject);
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            var storyC = EntityFactory.CreateStory("Story C", childProject);
            var defectD = EntityFactory.CreateDefect("Defect D", childProject);

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyC.Estimate = 2;
            defectD.Estimate = 3;

            storyA.Save();
            defectB.Save();
            storyC.Save();
            defectD.Save();

            // Close child project:
            childProject.Close();

            ResetInstance();

            // Expect to exclude Open workitems in closed child projects
            Assert.AreEqual(15, SandboxProject.GetTotalEstimate(null, true), "Expect to exclude Open workitems in closed child projects");
            Assert.AreEqual(15, SandboxProject.GetTotalEstimate(new PrimaryWorkitemFilter(), true), "Expect to exclude Open workitems in closed child projects");
            Assert.AreEqual(5, SandboxProject.GetTotalEstimate(new StoryFilter(), true), "Expect to exclude Open workitems in closed child projects");
            Assert.AreEqual(10, SandboxProject.GetTotalEstimate(new DefectFilter(), true), "Expect to exclude Open workitems in closed child projects");
        }
        #endregion

        #region Iteration Totals
        [Test]
        public void IterationTotalDone() {
            var storyA = CreateStory("Story A", SandboxProject, SandboxIteration);
            var defectB = CreateDefect("Defect B", SandboxProject, SandboxIteration);

            storyA.CreateTask("Task 1").CreateEffort(5);
            defectB.CreateEffort(10);

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(15, SandboxIteration.GetTotalDone(null));
            Assert.AreEqual(15, SandboxIteration.GetTotalDone(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalDone(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalDone(new StoryFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalDone(new DefectFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalDone(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalDone(new TaskFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalDone(new TestFilter()));
        }

        [Test]
        public void IterationTotalToDo() {
            var storyA = CreateStory("Story A", SandboxProject, SandboxIteration);
            var defectB = CreateDefect("Defect B", SandboxProject, SandboxIteration);

            var task1 = storyA.CreateTask("Task 1");
            task1.ToDo = 5;
            defectB.ToDo = 10;

            task1.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxIteration.GetTotalToDo(null));
            Assert.AreEqual(15, SandboxIteration.GetTotalToDo(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalToDo(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalToDo(new StoryFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalToDo(new DefectFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalToDo(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalToDo(new TaskFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalToDo(new TestFilter()));
        }

        [Test]
        public void IterationTotalDetailEstimate() {
            var storyA = CreateStory("Story A", SandboxProject, SandboxIteration);
            var defectB = CreateDefect("Defect B", SandboxProject, SandboxIteration);

            var task1 = storyA.CreateTask("Task 1");
            task1.DetailEstimate = 5;
            defectB.DetailEstimate = 10;

            task1.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxIteration.GetTotalDetailEstimate(null));
            Assert.AreEqual(15, SandboxIteration.GetTotalDetailEstimate(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalDetailEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalDetailEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalDetailEstimate(new DefectFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalDetailEstimate(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalDetailEstimate(new TaskFilter()));
            Assert.AreEqual(null, SandboxIteration.GetTotalDetailEstimate(new TestFilter()));
        }

        [Test]
        public void IterationTotalEstimate() {
            var storyA = CreateStory("Story A", SandboxProject, SandboxIteration);
            var defectB = CreateDefect("Defect B", SandboxProject, SandboxIteration);

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateStory("Rogue");
            rogue.Estimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxIteration.GetTotalEstimate(null));
            Assert.AreEqual(15, SandboxIteration.GetTotalEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxIteration.GetTotalEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxIteration.GetTotalEstimate(new DefectFilter()));
        }
        #endregion

        #region Team Totals
        [Test]
        public void TeamTotalDone() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Team = SandboxTeam;
            defectB.Team = SandboxTeam;

            storyA.Save();
            defectB.Save();

            task1.CreateEffort(5);
            defectB.CreateEffort(10);

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Team = SandboxTeam;
            rogue.Save();
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(15, SandboxTeam.GetTotalDone(null));
            Assert.AreEqual(15, SandboxTeam.GetTotalDone(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalDone(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalDone(new StoryFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalDone(new DefectFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalDone(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalDone(new TaskFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalDone(new TestFilter()));
        }

        [Test]
        public void TeamTotalToDo() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Team = SandboxTeam;
            defectB.Team = SandboxTeam;

            task1.ToDo = 5;
            defectB.ToDo = 10;

            task1.Save();
            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Team = SandboxTeam;
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxTeam.GetTotalToDo(null));
            Assert.AreEqual(15, SandboxTeam.GetTotalToDo(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalToDo(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalToDo(new StoryFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalToDo(new DefectFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalToDo(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalToDo(new TaskFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalToDo(new TestFilter()));
        }

        [Test]
        public void TeamTotalDetailEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Team = SandboxTeam;
            defectB.Team = SandboxTeam;

            task1.DetailEstimate = 5;
            defectB.DetailEstimate = 10;

            task1.Save();
            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Team = SandboxTeam;
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxTeam.GetTotalDetailEstimate(null));
            Assert.AreEqual(15, SandboxTeam.GetTotalDetailEstimate(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalDetailEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalDetailEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalDetailEstimate(new DefectFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalDetailEstimate(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalDetailEstimate(new TaskFilter()));
            Assert.AreEqual(null, SandboxTeam.GetTotalDetailEstimate(new TestFilter()));
        }

        [Test]
        public void TeamTotalEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            storyA.Team = SandboxTeam;
            defectB.Team = SandboxTeam;

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateStory("Rogue");
            rogue.Team = SandboxTeam;
            rogue.Estimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxTeam.GetTotalEstimate(null));
            Assert.AreEqual(15, SandboxTeam.GetTotalEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxTeam.GetTotalEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxTeam.GetTotalEstimate(new DefectFilter()));
        }
        #endregion

        #region Member Totals
        [Test]
        public void MemberTotalDone() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Owners.Add(SandboxMember);
            defectB.Owners.Add(SandboxMember);
            task1.Owners.Add(SandboxMember);

            storyA.Save();
            defectB.Save();

            task1.CreateEffort(5);
            defectB.CreateEffort(10);

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Owners.Add(SandboxMember);
            rogue.Save();
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(15, SandboxMember.GetTotalDone(null));
            Assert.AreEqual(15, SandboxMember.GetTotalDone(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalDone(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalDone(new StoryFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalDone(new DefectFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalDone(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalDone(new TaskFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalDone(new TestFilter()));
        }

        [Test]
        public void MemberTotalToDo() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Owners.Add(SandboxMember);
            defectB.Owners.Add(SandboxMember);
            task1.Owners.Add(SandboxMember);

            task1.ToDo = 5;
            defectB.ToDo = 10;

            task1.Save();
            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Owners.Add(SandboxMember);
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxMember.GetTotalToDo(null));
            Assert.AreEqual(15, SandboxMember.GetTotalToDo(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalToDo(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalToDo(new StoryFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalToDo(new DefectFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalToDo(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalToDo(new TaskFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalToDo(new TestFilter()));
        }

        [Test]
        public void MemberTotalDetailEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);
            var task1 = storyA.CreateTask("Task 1");

            storyA.Owners.Add(SandboxMember);
            defectB.Owners.Add(SandboxMember);
            task1.Owners.Add(SandboxMember);

            task1.DetailEstimate = 5;
            defectB.DetailEstimate = 10;

            task1.Save();
            storyA.Save();
            defectB.Save();

            var rogue = SandboxProject.CreateDefect("Rogue");
            rogue.Owners.Add(SandboxMember);
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, SandboxMember.GetTotalDetailEstimate(null));
            Assert.AreEqual(15, SandboxMember.GetTotalDetailEstimate(new WorkitemFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalDetailEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalDetailEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalDetailEstimate(new DefectFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalDetailEstimate(new SecondaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalDetailEstimate(new TaskFilter()));
            Assert.AreEqual(null, SandboxMember.GetTotalDetailEstimate(new TestFilter()));
        }

        [Test]
        public void MemberTotalEstimate() {
            var storyA = EntityFactory.CreateStory("Story A", SandboxProject);
            var defectB = EntityFactory.CreateDefect("Defect B", SandboxProject);

            storyA.Owners.Add(SandboxMember);
            defectB.Owners.Add(SandboxMember);

            storyA.Estimate = 5;
            defectB.Estimate = 10;

            storyA.Save();
            defectB.Save();

            //Epic rogue = SandboxProject.CreateEpic("Rogue");
            //rogue.Owners.Add(SandboxMember);
            //rogue.Estimate = 13.37;
            //rogue.Save();

            Assert.AreEqual(15, SandboxMember.GetTotalEstimate(null));
            Assert.AreEqual(15, SandboxMember.GetTotalEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(5, SandboxMember.GetTotalEstimate(new StoryFilter()));
            Assert.AreEqual(10, SandboxMember.GetTotalEstimate(new DefectFilter()));
        }
        #endregion

        #region PrimaryWorkitem Totals
        [Test]
        public void WorkitemTotalDetailEstimate() {
            var story = EntityFactory.CreateStory("Story 1", SandboxProject);
            var task1 = story.CreateTask("Task 1");
            var test1 = story.CreateTest("Test 1");
            var defect = EntityFactory.CreateDefect("Defect 1", SandboxProject);

            task1.DetailEstimate = 10;
            test1.DetailEstimate = 5;
            defect.DetailEstimate = 3;
            task1.Save();
            test1.Save();
            defect.Save();

            var rogue = story.CreateTask("Rogue");
            rogue.DetailEstimate = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, story.GetTotalDetailEstimate(null));
            Assert.AreEqual(15, story.GetTotalDetailEstimate(new WorkitemFilter()));
            Assert.AreEqual(10, story.GetTotalDetailEstimate(new TaskFilter()));
            Assert.AreEqual(5, story.GetTotalDetailEstimate(new TestFilter()));
            Assert.AreEqual(3, defect.GetTotalDetailEstimate(new PrimaryWorkitemFilter()));
            Assert.AreEqual(3, defect.GetTotalDetailEstimate(new DefectFilter()));
        }

        [Test]
        public void WorkitemTotalToDo() {
            var story = EntityFactory.CreateStory("Story 1", SandboxProject);
            var task1 = story.CreateTask("Task 1");
            var test1 = story.CreateTest("Test 1");
            var defect = EntityFactory.CreateDefect("Defect 1", SandboxProject);

            task1.ToDo = 10;
            test1.ToDo = 5;
            defect.ToDo = 3;
            task1.Save();
            test1.Save();
            defect.Save();

            var rogue = story.CreateTask("Rogue");
            rogue.ToDo = 13.37;
            rogue.Save();
            rogue.Delete();

            Assert.AreEqual(15, story.GetTotalToDo(null));
            Assert.AreEqual(15, story.GetTotalToDo(new WorkitemFilter()));
            Assert.AreEqual(10, story.GetTotalToDo(new TaskFilter()));
            Assert.AreEqual(5, story.GetTotalToDo(new TestFilter()));
            Assert.AreEqual(3, defect.GetTotalToDo(new PrimaryWorkitemFilter()));
            Assert.AreEqual(3, defect.GetTotalToDo(new DefectFilter()));
        }

        [Test]
        public void WorkitemTotalDone() {
            var story = EntityFactory.CreateStory("Story 1", SandboxProject);
            var task1 = story.CreateTask("Task 1");
            var test1 = story.CreateTest("Test 1");
            var defect = EntityFactory.CreateDefect("Defect 1", SandboxProject);

            task1.CreateEffort(10);
            test1.CreateEffort(5);
            defect.CreateEffort(3);

            var rogue = story.CreateTask("Rogue");
            rogue.CreateEffort(13.37);
            rogue.Delete();

            Assert.AreEqual(15, story.GetTotalDone(null));
            Assert.AreEqual(15, story.GetTotalDone(new WorkitemFilter()));
            Assert.AreEqual(10, story.GetTotalDone(new TaskFilter()));
            Assert.AreEqual(5, story.GetTotalDone(new TestFilter()));
            Assert.AreEqual(3, defect.GetTotalDone(new PrimaryWorkitemFilter()));
            Assert.AreEqual(3, defect.GetTotalDone(new DefectFilter()));
        }
        #endregion
    }
}