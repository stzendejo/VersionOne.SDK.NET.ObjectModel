using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VersionOne.SDK.ObjectModel.Tests {
    /// <summary>
    /// Entity creation factory that cleans up created objects when disposed. Intended to apply with 'using' statement.
    /// If you create BaseAsset manually, do not forget to handle them on your own.
    /// </summary>
    internal class EntityFactory {
        private readonly Stack<BaseAsset> baseAssets = new Stack<BaseAsset>();
        private readonly Stack<Entity> entities = new Stack<Entity>();
        private readonly V1Instance instance;

        public delegate TEntity FactoryFunction<out TEntity>() where TEntity : BaseAsset;

        internal EntityFactory(V1Instance instance) {
            this.instance = instance;
        }

        internal RegressionPlan CreateRegressionPlan(string name, Project project, IDictionary<string, object> attributes) {
            var plan = instance.Create.RegressionPlan(name, project, attributes);
            RegisterForDisposal(plan);

            return plan;
        }

        internal RegressionPlan CreateRegressionPlan(string name, Project project) {
            return CreateRegressionPlan(name, project, new Dictionary<string, object>());
        }

        internal RegressionSuite CreateRegressionSuite(string name, RegressionPlan regressionPlan) {
            var suite = instance.Create.RegressionSuite(name, regressionPlan, null);
            RegisterForDisposal(suite);

            return suite;
        }

        internal RegressionSuite CreateRegressionSuite(string name, RegressionPlan regressionPlan, IDictionary<string, object> attributes) {
            var suite = instance.Create.RegressionSuite(name, regressionPlan, attributes);
            RegisterForDisposal(suite);

            return suite;
        }

        internal Member CreateMember(string name) {
            var member = instance.Create.Member(name, Guid.NewGuid().ToString());
            RegisterForDisposal(member);

            member.Save();
            return member;
        }

        internal RegressionTest CreateRegressionTest(string name, Project project, IDictionary<string, object> attributes) {
            var regressionTest = instance.Create.RegressionTest(name, project, attributes);
            RegisterForDisposal(regressionTest);

            return regressionTest;
        }

        internal RegressionTest CreateRegressionTest(string name, Project project) {
            var regressionTest = instance.Create.RegressionTest(name, project);
            RegisterForDisposal(regressionTest);
            return regressionTest;
        }

        internal RegressionTest CreateRegressionTest(Test test) {
            var regressionTest = instance.Create.RegressionTest(test);
            RegisterForDisposal(regressionTest);
            return regressionTest;
        }

        internal TestSet CreateTestSet(string name, RegressionSuite suite, IDictionary<string, object> attributes = null) {
            var testSet = instance.Create.TestSet(name, suite, suite.RegressionPlan.Project, attributes);
            RegisterForDisposal(testSet);
            return testSet;
        }

        internal Test CreateTest(string name, Workitem workitem) {
            var test = instance.Create.Test(name, workitem);
            RegisterForDisposal(test);

            return test;
        }

        internal Task CreateTask(string name, PrimaryWorkitem primaryWorkitem, IDictionary<string, object> attributes = null) {
            var task = instance.Create.Task(name, primaryWorkitem, attributes);
            RegisterForDisposal(task);

            return task;
        }

        internal Story CreateStory(string name, Project scope) {
            var story = instance.Create.Story(name, scope);
            RegisterForDisposal(story);
            return story;
        }

        internal Defect CreateDefect(string name, Project scope) {
            var defect = instance.Create.Defect(name, scope);
            RegisterForDisposal(defect);
            return defect;
        }

        internal Epic CreateEpic(string name, Project scope, Epic parent) {
            var epic = instance.Create.Epic(name, scope);
            RegisterForDisposal(epic);
            
            if(parent != null) {
                epic.Super = parent;
            }

            epic.Save();
            return epic;
        }

        internal Project CreateProject(string sandboxName, Project rootProject, IDictionary<string, Object> mandatoryAttributes) {
            var project = instance.Create.Project(sandboxName, rootProject, DateTime.Now, null, mandatoryAttributes);
            RegisterForDisposal(project);

            return project;
        }

        internal Project CreateProjectWithSchedule(string sandboxName, Project rootProject) {
            var schedule = instance.Create.Schedule("Sandbox Schedule", new TimeSpan(7, 0, 0, 0), new TimeSpan(0L));
            RegisterForDisposal(schedule);
            var project = instance.Create.Project(sandboxName, rootProject, DateTime.Now, schedule);
            RegisterForDisposal(project);

            return project;
        }

        internal Project CreateSubProject(string sandboxName, DateTime now, Schedule schedule, Project sandboxProject) {
            var project = sandboxProject.CreateSubProject("Son of " + sandboxName, DateTime.Now, schedule);
            RegisterForDisposal(project);
            return project;
        }

        internal Epic CreateEpic(string name, Project sandboxProject) {
            var epic = sandboxProject.CreateEpic(name);
            RegisterForDisposal(epic);
            return epic;
        }

        internal Epic CreateChildEpic(Epic epic) {
            var childEpic = epic.GenerateChildEpic();
            RegisterForDisposal(childEpic);
            return childEpic;
        }

        internal Environment CreateEnvironment(String name, Project project, IDictionary<String, Object> attributes) {
            var environment = instance.Create.Environment(name, project, attributes);
            RegisterForDisposal(environment);
            return environment;
        }


        /// <summary>
        /// Register BaseAssets for dispose it at the end.
        /// </summary>
        /// <param name="asset">Entity to dispose.</param>
        internal void RegisterForDisposal(Entity asset) {
            if (!entities.Contains(asset)) {
                entities.Push(asset);
            }
        }

        /// <summary>
        /// Register BaseAssets for dispose it at the end.
        /// </summary>
        /// <param name="asset">BaseAsset to dispose.</param>
        internal void RegisterForDisposal(BaseAsset asset) {
            if (!baseAssets.Contains(asset)) {
                baseAssets.Push(asset);
            }
        }

        /// <summary>
        /// Create entity using custom local method and register it for later disposal
        /// </summary>
        /// <typeparam name="TEntity">Required type of entity</typeparam>
        /// <param name="creator">Delegate used to create entity instance</param>
        /// <returns>Created entity</returns>
        internal TEntity Create<TEntity>(FactoryFunction<TEntity> creator) where TEntity : BaseAsset {
            var entity = creator.Invoke();
            RegisterForDisposal(entity);
            return entity;
        }

        internal void Dispose() {
            new AssetsCleaner(baseAssets, entities, GetDefaultProject()).Delete();
        }


        private Project GetDefaultProject() {
            var projects = instance.Get.Projects(null);

            return BaseSDKTester.First(projects);
        }
    }
}