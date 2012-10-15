using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a project or release in the VersionOne system
    /// </summary>
    [MetaData("Scope")]
    public class Project : BaseAsset {
        internal Project(AssetID id, V1Instance instance) : base(id, instance) {
        }

        internal Project(V1Instance instance) : base(instance) {
        }

        /// <summary>
        /// Parent project this project belongs to
        /// </summary>
        [MetaRenamed("Parent")]
        public Project ParentProject {
            get { return GetRelation<Project>("Parent"); }
            set { SetRelation("Parent", value); }
        }

        /// <summary>
        /// Date this project begins
        /// </summary>
        public DateTime BeginDate {
            get { return Get<DateTime>("BeginDate"); }
            set { Set("BeginDate", value.Date); }
        }

        /// <summary>
        /// Date this project ends
        /// </summary>
        public DateTime? EndDate {
            get { return Get<DateTime?>("EndDate"); }
            set {
                if (value.HasValue) {
                    Set("EndDate", value.Value.Date);
                } else {
                    Set("EndDate", value);
                }
            }
        }

        /// <summary>
        /// Schedule that defines how this project's iterations are spaced
        /// </summary>
        public Schedule Schedule {
            get { return GetRelation<Schedule>("Schedule"); }
            set { SetRelation("Schedule", value); }
        }

        /// <summary>
        /// The Member who pwns this Project.
        /// </summary>
        public Member Owner {
            get { return GetRelation<Member>("Owner"); }
            set { SetRelation("Owner", value); }
        }

        /// <summary>
        /// TestSuite assigned to this Project.
        /// </summary>
        public TestSuite TestSuite {
            get { return GetRelation<TestSuite>("TestSuite"); }
            set { SetRelation("TestSuite", value); }
        }

        /// <summary>
        /// This Project's Status
        /// </summary>
        public IListValueProperty Status {
            get { return GetListValue<ProjectStatus>("Status"); }
        }

        /// <summary>
        /// Build Projects associated with this Project
        /// </summary>
        public ICollection<BuildProject> BuildProjects {
            get { return GetMultiRelation<BuildProject>("BuildProjects"); }
        }

        /// <summary>
        /// Create a sub project under this project with a name, begin date, and optional schedule
        /// </summary>
        /// <param name="name">Name of the new project.</param>
        /// <param name="beginDate">Date the schedule will begin.</param>
        /// <param name="schedule">The new schedule. If null, the project will inherit the parent project's schedule.</param>
        /// <returns>The newly created project.</returns>
        public Project CreateSubProject(string name, DateTime beginDate, Schedule schedule) {
            return Instance.Create.Project(name, this, beginDate, schedule);
        }

        /// <summary>
        /// Create a sub project under this project with a name, begin date, and optional schedule
        /// </summary>
        /// <param name="name">Name of the new project.</param>
        /// <param name="beginDate">Date the schedule will begin.</param>
        /// <param name="attributes">required attributes</param>
        /// <param name="schedule">The new schedule. If null, the project will inherit the parent project's schedule.</param>
        /// <returns>The newly created project.</returns>
        public Project CreateSubProject(string name, DateTime beginDate, Schedule schedule, IDictionary<string, object> attributes) {
            return Instance.Create.Project(name, this, beginDate, schedule, attributes);
        }

        /// <summary>
        /// Create a sub project under this project with a name and begin date
        /// </summary>
        /// <param name="name">Name of the new project.</param>
        /// <param name="beginDate">Date the schedule will begin.</param>
        /// <returns>The newly created project.</returns>
        public Project CreateSubProject(string name, DateTime beginDate) {
            return CreateSubProject(name, beginDate, null);
        }

        /// <summary>
        /// Create a new Epic in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Epic.</param>
        /// <returns>A new Epic.</returns>
        public Epic CreateEpic(string name) {
            return Instance.Create.Epic(name, this);
        }

        /// <summary>
        /// Create a new Epic in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Epic.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Epic.</returns>
        public Epic CreateEpic(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Epic(name, this, attributes);
        }

        /// <summary>
        /// Create a new Story in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Story.</param>
        /// <returns>A new Story.</returns>
        public Story CreateStory(string name) {
            return Instance.Create.Story(name, this);
        }

        /// <summary>
        /// Create a new Story in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Story.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Story.</returns>
        public Story CreateStory(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Story(name, this, attributes);
        }

        /// <summary>
        /// Create a new Defect in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Defect.</param>
        /// <returns>A new Defect.</returns>
        public Defect CreateDefect(string name) {
            return Instance.Create.Defect(name, this);
        }

        /// <summary>
        /// Create a new Defect with required attributes in this Project.
        /// </summary>
        /// <param name="name">name of the Defect</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>new Defect</returns>
        public Defect CreateDefect(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Defect(name, this, attributes);
        }

        /// <summary>
        /// Create a new Theme in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Theme.</param>
        /// <returns>A new Theme.</returns>
        public Theme CreateTheme(string name) {
            return Instance.Create.Theme(name, this);
        }

        /// <summary>
        /// Create a new Theme in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Theme.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Theme.</returns>
        public Theme CreateTheme(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Theme(name, this, attributes);
        }

        /// <summary>
        /// Create a new Goal in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Goal.</param>
        /// <returns>A new Goal.</returns>
        public Goal CreateGoal(string name) {
            return Instance.Create.Goal(name, this);
        }

        /// <summary>
        /// Create a new Goal in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Goal.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Goal.</returns>
        public Goal CreateGoal(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Goal(name, this, attributes);
        }

        /// <summary>
        /// Create a new Request in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Request.</param>
        /// <returns>A new Request.</returns>
        public Request CreateRequest(string name) {
            return Instance.Create.Request(name, this);
        }

        /// <summary>
        /// Create a new Request in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Request.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Request.</returns>
        public Request CreateRequest(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Request(name, this, attributes);
        }

        /// <summary>
        /// Create a new Issue in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Issue.</param>
        /// <returns>A new Issue.</returns>
        public Issue CreateIssue(string name) {
            return Instance.Create.Issue(name, this);
        }

        /// <summary>
        /// Create a new Issue in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Issue.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Issue.</returns>
        public Issue CreateIssue(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Issue(name, this, attributes);
        }

        /// <summary>
        /// Create a new Retrospective in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Retrospective.</param>
        /// <returns>A new Retrospective.</returns>
        public Retrospective CreateRetrospective(string name) {
            return Instance.Create.Retrospective(name, this);
        }

        /// <summary>
        /// Create a new Retrospective in this Project.
        /// </summary>
        /// <param name="name">The initial name of the Retrospective.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Retrospective.</returns>
        public Retrospective CreateRetrospective(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Retrospective(name, this, attributes);
        }

        /// <summary>
        /// Create a new Iteration in the Project where the schedule is defined.  Use the suggested system values for the new iteration.
        /// </summary>
        /// <returns>A new Iteration.</returns>
        public Iteration CreateIteration() {
            return Instance.Create.Iteration(this);
        }

        /// <summary>
        /// Create a new Iteration in the Project where the schedule is defined.  Use the suggested system values for the new iteration.
        /// </summary>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Iteration.</returns>
        public Iteration CreateIteration(IDictionary<string, object> attributes) {
            return Instance.Create.Iteration(this, attributes);
        }

        /// <summary>
        /// Create a new Iteration in the Project where the schedule is defined.
        /// </summary>
        /// <param name="name">The initial name of the Iteration.</param>
        /// <param name="beginDate">The begin date of the Iteration.</param>
        /// <param name="endDate">The end date of the Iteration.</param>
        /// <returns>A new Iteration.</returns>
        public Iteration CreateIteration(string name, DateTime beginDate, DateTime endDate) {
            return Instance.Create.Iteration(name, Schedule, beginDate, endDate);
        }

        /// <summary>
        /// Create a new Iteration in the Project where the schedule is defined.
        /// </summary>
        /// <param name="name">The initial name of the Iteration.</param>
        /// <param name="beginDate">The begin date of the Iteration.</param>
        /// <param name="endDate">The end date of the Iteration.</param>
        /// <param name="attributes">required attributes</param>
        /// <returns>A new Iteration.</returns>
        public Iteration CreateIteration(string name, DateTime beginDate, DateTime endDate,
            IDictionary<string, object> attributes) {
            return Instance.Create.Iteration(name, Schedule, beginDate, endDate, attributes);
        }

        /// <summary>
        /// Create a new Regression Plan in the Project.
        /// </summary>
        /// <param name="name">Regression Plan title.</param>
        /// <returns>A new Regression Plan.</returns>
        public RegressionPlan CreateRegressionPlan(string name) {
            return Instance.Create.RegressionPlan(name, this);
        }

        /// <summary>
        /// Create a new Regression Plan in the Project with additional attributes.
        /// </summary>
        /// <param name="name">Regression Plan title.</param>
        /// <param name="attributes">Additional attributes for inicialization Regression Plan.</param>
        /// <returns>A new Regression Plan.</returns>
        public RegressionPlan CreateRegressionPlan(string name, IDictionary<string, object> attributes) {
            return Instance.Create.RegressionPlan(name, this, attributes);
        }

        /// <summary>
        /// Create a new Test Set in the project.
        /// </summary>
        /// <param name="name">Test Set name</param>
        /// <param name="suite">Parent Regression suite</param>
        /// <param name="attributes">Additional attributes</param>
        /// <returns>Newly created test set</returns>
        public TestSet CreateTestSet(string name, RegressionSuite suite, IDictionary<string, object> attributes) {
            return Instance.Create.TestSet(name, suite, this, attributes);
        }

        /// <summary>
        /// Create a new Test Set in the project.
        /// </summary>
        /// <param name="name">Test Set name</param>
        /// <param name="suite">Parent Regression suite</param>
        /// <returns>Newly created test set</returns>
        public TestSet CreateTestSet(string name, RegressionSuite suite) {
            return Instance.Create.TestSet(name, suite, this, null);
        }

        /// <summary>
        /// Create a new Environment in the Project.
        /// </summary>
        /// <param name="name">Environment title.</param>
        /// <returns>A new Environment.</returns>
        public Environment CreateEnvironment(string name) {
            return Instance.Create.Environment(name, this);
        }

        /// <summary>
        /// Create a new Environment in the Project with additional attributes.
        /// </summary>
        /// <param name="name">Environment title.</param>
        /// <param name="attributes">Additional attributes for inicialization Environment.</param>
        /// <returns>A new Environment.</returns>
        public Environment CreateEnvironment(string name, IDictionary<string, object> attributes) {
            return Instance.Create.Environment(name, this, attributes);
        }

        /// <summary>
        /// Create a new Regression Test in the Project.
        /// </summary>
        /// <param name="name">Name of Regression Test.</param>
        /// <returns>A new Regression Test.</returns>
        public RegressionTest CreateRegressionTest(string name) {
            return Instance.Create.RegressionTest(name, this);
        }

        /// <summary>
        /// Create a new Regression Test in the Project with additional attributes.
        /// </summary>
        /// <param name="name">Name of Regression Test.</param>
        /// <param name="attributes">Additional attributes for inicialization Regression Test.</param>
        /// <returns>A new Regression Test.</returns>
        public RegressionTest CreateRegressionTest(string name, IDictionary<string, object> attributes) {
            return Instance.Create.RegressionTest(name, this, attributes);
        }

        /// <summary>
        /// Members assigned to this project
        /// </summary>
        public ICollection<Member> AssignedMembers {
            get { return GetMultiRelation<Member>("Members"); }
        }

        #region Disabled Helper Relations to Theme
        ///// <summary>
        ///// Themes in this Project and Projects above this one
        ///// </summary>
        //public IEnumerable<Theme> ThemesAvailable { get { return GetMultiRelation<Theme>("ParentMeAndUp.Workitems:Theme"); } } // This wants to be Workitems:Theme, if it weren't for MS KB 932552 
        #endregion

        /// <summary>
        /// A read-only collection of StoryTemplates in this Project
        /// </summary>
        public ICollection<StoryTemplate> StoryTemplates {
            get { return GetMultiRelation<StoryTemplate>("Workitems[AssetType='Story';AssetState='200']").AsReadOnly(); }
        }

        // This wants to be Workitems:Story, if it weren't for MS KB 932552

        /// <summary>
        /// A read-only collection of DefectTemplates in the Project
        /// </summary>
        public ICollection<DefectTemplate> DefectTemplates {
            get { return GetMultiRelation<DefectTemplate>("Workitems[AssetType='Defect';AssetState='200']").AsReadOnly(); }
        }

        // This wants to be Workitems:Defect, if it weren't for MS KB 932552

        /// <summary>
        /// Returns a flattened collection of this project and all Projects that descend from this project. (readonly)
        /// </summary>
        /// <returns>a flattened collection of this project and all Projects that descend from this project</returns>
        public ICollection<Project> GetThisAndAllChildProjects() {
            return GetMultiRelation<Project>("ChildrenMeAndDown[AssetState!='Closed']").AsReadOnly();
        }

        private T WithThisProjectIncludedIn<T>(T filter, bool includeSubprojects) where T : ProjectAssetFilter, new() {
            filter = filter ?? new T();

            filter.Project.Clear();
            if (includeSubprojects) {
				filter.ArbitraryWhereTerms.Add(new KeyValuePair<string, string>("Scope[AssetState!='Closed'].ParentMeAndUp", this.ID));
            } else {
                filter.Project.Add(this);
            }

            return filter;
        }

        /// <summary>
        /// A collection of sub-projects that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all child projects in the project are returned.</param>
        public ICollection<Project> GetChildProjects(ProjectFilter filter) {
            return GetChildProjects(filter, false);
        }

        /// <summary>
        /// A collection of sub-projects that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all child projects in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub-project or not. This only adds open subprojects.</param>
        public ICollection<Project> GetChildProjects(ProjectFilter filter, bool includeSubprojects) {
            filter = filter ?? new ProjectFilter();
            filter.Parent.Clear();
            if (includeSubprojects) {
                foreach (var p in GetThisAndAllChildProjects()) {
                    filter.Parent.Add(p);
                }
            } else {
                filter.Parent.Add(this);
            }
            return Instance.Get.Projects(filter);
        }

        /// <summary>
        /// A collection of regression plans that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project(s) will be set automatically. If null, all related regression items in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub-project or not. This only adds open subprojects.</param>
        public ICollection<RegressionPlan> GetRegressionPlans(RegressionPlanFilter filter, bool includeSubprojects) {
            filter = filter ?? new RegressionPlanFilter();

            filter.Project.Clear();

            if (includeSubprojects) {
                foreach (var project in GetThisAndAllChildProjects()) {
                    filter.Project.Add(project);
                }
            } else {
                filter.Project.Add(this);
            }

            return Instance.Get.RegressionPlans(filter);
        }

        /// <summary>
        /// A collection of regression plans that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all related regression items in the project are returned.</param>
        public ICollection<RegressionPlan> GetRegressionPlans(RegressionPlanFilter filter) {
            return GetRegressionPlans(filter, false);
        }

        /// <summary>
        /// A collection of Effort records that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all effort records in the project are returned.</param>
        public ICollection<Effort> GetEffortRecords(EffortFilter filter) {
            return GetEffortRecords(filter, false);
        }

        /// <summary>
        /// A collection of Effort records that belong to this project
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all effort records in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        public ICollection<Effort> GetEffortRecords(EffortFilter filter, bool includeSubprojects) {
            filter = filter ?? new EffortFilter();
            filter.Project.Clear();
            if (includeSubprojects) {
                foreach (var p in GetThisAndAllChildProjects()) {
                    filter.Project.Add(p);
                }
            } else {
                filter.Project.Add(this);
            }
            return Instance.Get.EffortRecords(filter);
        }

        /// <summary>
        /// Get Epics in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all epics in the project are returned.</param>
        /// <returns>A readonly ICollection of Epic</returns>
        public ICollection<Epic> GetEpics(EpicFilter filter) {
            return GetEpics(filter, false);
        }

        /// <summary>
        /// Get Epics in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all epics in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Epic</returns>
        public ICollection<Epic> GetEpics(EpicFilter filter, bool includeSubprojects) {
            return Instance.Get.Epics(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get tracked Epics that belong to current Project.
        /// </summary>
        /// <returns></returns>
        public ICollection<Epic> GetTrackedEpics() {
            return Instance.Get.TrackedEpics(new[] {this});
        }

        /// <summary>
        /// Get stories in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all stories in the project are returned.</param>
        /// <returns>An ICollection of Story</returns>
        public ICollection<Story> GetStories(StoryFilter filter) {
            return GetStories(filter, false);
        }

        /// <summary>
        /// Get stories in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all stories in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>An ICollection of Story</returns>
        public ICollection<Story> GetStories(StoryFilter filter, bool includeSubprojects) {
            return Instance.Get.Stories(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Defects in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all defects in the project are returned.</param>
        /// <returns>An ICollection of Defect</returns>
        public ICollection<Defect> GetDefects(DefectFilter filter) {
            return GetDefects(filter, false);
        }

        /// <summary>
        /// Get Defects in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all defects in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>An ICollection of Defect</returns>
        public ICollection<Defect> GetDefects(DefectFilter filter, bool includeSubprojects) {
            return Instance.Get.Defects(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get test sets in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all test sets in the project are returned.</param>
        /// <returns>Collection of Test Sets</returns>
        public ICollection<TestSet> GetTestSets(TestSetFilter filter) {
            return GetTestSets(filter, false);
        }

        /// <summary>
        /// Get test sets in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all test sets in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>Collection of Test Sets</returns>
        public ICollection<TestSet> GetTestSets(TestSetFilter filter, bool includeSubprojects) {
            return Instance.Get.TestSets(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get PrimaryWorkitems in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all stories and defects in the project are returned.</param>
        /// <returns>An ICollection of PrimaryWorkitem</returns>
        public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter) {
            return GetPrimaryWorkitems(filter, false);
        }

        /// <summary>
        /// Get PrimaryWorkitems in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all stories and defects in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>An ICollection of PrimaryWorkitem</returns>
        public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(PrimaryWorkitemFilter filter, bool includeSubprojects) {
            return Instance.Get.PrimaryWorkitems(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Iterations in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all iterations in the project are returned.</param>
        /// <returns>A readonly ICollection of Iteration</returns>
        public ICollection<Iteration> GetIterations(IterationFilter filter) {
            return GetIterations(filter, false);
        }

        /// <summary>
        /// Get Iterations in this Project's Schedule filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Schedule will be set automatically. If null, all iterations in the project's schedule are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Iteration</returns>
        public ICollection<Iteration> GetIterations(IterationFilter filter, bool includeSubprojects) {
            filter = filter ?? new IterationFilter();

            filter.Schedule.Clear();
            if (includeSubprojects) {
                foreach (var p in GetThisAndAllChildProjects()) {
                    filter.Schedule.Add(p.Schedule);
                }
            } else {
                filter.Schedule.Add(Schedule);
            }

            return Instance.Get.Iterations(filter);
        }

        /// <summary>
        /// Get Themes in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all themes in the project are returned.</param>
        /// <returns>A readonly ICollection of Theme</returns>
        public ICollection<Theme> GetThemes(ThemeFilter filter) {
            return GetThemes(filter, false);
        }

        /// <summary>
        /// Get Themes in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all themes in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Theme</returns>
        public ICollection<Theme> GetThemes(ThemeFilter filter, bool includeSubprojects) {
			return Instance.Get.Themes(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get SecondaryWorkitems in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all tasks and tests in the project are returned.</param>
        /// <returns>An ICollection of SecondaryWorkitem</returns>
        public ICollection<SecondaryWorkitem> GetSecondaryWorkitems(SecondaryWorkitemFilter filter) {
            return GetSecondaryWorkitems(filter, false);
        }

        /// <summary>
        /// Get SecondaryWorkitems in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all tasks and tests in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>An ICollection of SecondaryWorkitem</returns>
        public ICollection<SecondaryWorkitem> GetSecondaryWorkitems(SecondaryWorkitemFilter filter,
            bool includeSubprojects) {
            return Instance.Get.SecondaryWorkitems(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Tasks in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all tasks in the project are returned.</param>
        /// <returns>An ICollection of SecondaryWorkitem</returns>
        public ICollection<Task> GetTasks(TaskFilter filter) {
            return Instance.Get.Tasks(WithThisProjectIncludedIn(filter, false));
        }

        /// <summary>
        /// Get Tests in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all tests in the project are returned.</param>
        /// <returns>An ICollection of Tests</returns>
        public ICollection<Test> GetTests(TestFilter filter) {
            return Instance.Get.Tests(WithThisProjectIncludedIn(filter, false));
        }

        /// <summary>
        /// Get Requests in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Requests in the project are returned.</param>
        /// <returns>An ICollection of Requests</returns>
        public ICollection<Request> GetRequests(RequestFilter filter) {
            return GetRequests(filter, false);
        }

        /// <summary>
        /// Get Requests in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Requests in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Request</returns>
        public ICollection<Request> GetRequests(RequestFilter filter, bool includeSubprojects) {
            return Instance.Get.Requests(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Goals in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Goals in the project are returned.</param>
        /// <returns>An ICollection of Goals</returns>
        public ICollection<Goal> GetGoals(GoalFilter filter) {
            return GetGoals(filter, false);
        }

        /// <summary>
        /// Get Goals in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Goals in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>An ICollection of Goals</returns>
        public ICollection<Goal> GetGoals(GoalFilter filter, bool includeSubprojects) {
            return Instance.Get.Goals(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Retrospective in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Retrospective in the project are returned.</param>
        /// <returns>A readonly ICollection of Retrospective</returns>
        public ICollection<Retrospective> GetRetrospectives(RetrospectiveFilter filter) {
            return GetRetrospectives(filter, false);
        }

        /// <summary>
        /// Get Retrospective in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Retrospective in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Retrospective</returns>
        public ICollection<Retrospective> GetRetrospectives(RetrospectiveFilter filter, bool includeSubprojects) {
            return Instance.Get.Retrospectives(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Get Issues in this Project filtered as specified in the passed in filter. Does not include subprojects.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Issues in the project are returned.</param>
        /// <returns>A readonly ICollection of Issues</returns>
        public ICollection<Issue> GetIssues(IssueFilter filter) {
            return GetIssues(filter, false);
        }

        /// <summary>
        /// Get Issues in this Project filtered as specified in the passed in filter.
        /// </summary>
        /// <param name="filter">Criteria to filter on. Project will be set automatically. If null, all Issues in the project are returned.</param>
        /// <param name="includeSubprojects">Specifies whether to include items from sub project or not. This only adds open subprojects.</param>
        /// <returns>A readonly ICollection of Issues</returns>
        public ICollection<Issue> GetIssues(IssueFilter filter, bool includeSubprojects) {
            return Instance.Get.Issues(WithThisProjectIncludedIn(filter, includeSubprojects));
        }

        /// <summary>
        /// Inactivates the Project
        /// </summary>
        /// <exception cref="InvalidOperationException">The Project is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl() {
            Instance.ExecuteOperation<Project>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Project
        /// </summary>
        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Project>(this, "Reactivate");
        }

        private double? GetRollup(string multiRelation, string attribute, EntityFilter filter, bool includeChildProjects) {
            if (includeChildProjects) {
                multiRelation = "ChildrenMeAndDown[AssetState!='Closed']." + multiRelation;
            }

            return GetSum(multiRelation, filter, attribute);
        }

        /// <summary>
        /// Return the total estimate for all stories and defects in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter stories and defects on.</param>
        /// <param name="includeChildProjects">If true, include open sub projects, otherwise only include this project</param>
        /// <returns></returns>
        public double? GetTotalEstimate(PrimaryWorkitemFilter filter, bool includeChildProjects) {
            return GetRollup("Workitems:PrimaryWorkitem", "Estimate", filter ?? new PrimaryWorkitemFilter(), includeChildProjects);
        }

        /// <summary>
        /// Return the total estimate for all stories and defects in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter stories and defects on.</param>
        /// <returns></returns>
        public double? GetTotalEstimate(PrimaryWorkitemFilter filter) {
            return GetTotalEstimate(filter, false);
        }

        /// <summary>
        /// Return the total detail estimate for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <param name="includeChildProjects">If true, include open sub projects, otherwise only include this project</param>
        /// <returns></returns>
        public double? GetTotalDetailEstimate(WorkitemFilter filter, bool includeChildProjects) {
            return GetRollup("Workitems", "DetailEstimate", filter ?? new WorkitemFilter(), includeChildProjects);
        }

        /// <summary>
        /// Return the total detail estimate for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalDetailEstimate(WorkitemFilter filter) {
            return GetTotalDetailEstimate(filter, false);
        }

        /// <summary>
        /// Return the total to do for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <param name="includeChildProjects">If true, include open sub projects, otherwise only include this project</param>
        /// <returns></returns>
        public double? GetTotalToDo(WorkitemFilter filter, bool includeChildProjects) {
            return GetRollup("Workitems", "ToDo", filter ?? new WorkitemFilter(), includeChildProjects);
        }

        /// <summary>
        /// Return the total to do for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalToDo(WorkitemFilter filter) {
            return GetTotalToDo(filter, false);
        }

        /// <summary>
        /// Return the total done for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <param name="includeChildProjects">If true, include open sub projects, otherwise only include this project</param>
        /// <returns></returns>
        public double? GetTotalDone(WorkitemFilter filter, bool includeChildProjects) {
            return GetRollup("Workitems", "Actuals.Value", filter ?? new WorkitemFilter(), includeChildProjects);
        }

        /// <summary>
        /// Return the total done for all workitems in this project optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalDone(WorkitemFilter filter) {
            return GetTotalDone(filter, false);
        }
    }
}