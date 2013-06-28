using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    public partial class V1Instance {
        /// <summary>
        /// Get various assets in the system
        /// </summary>
        public GetMethods Get {
            get { return get ?? (get = new GetMethods(this)); }
        }

        private GetMethods get;

        /// <summary>
        /// Methods to get things
        /// </summary>
        public class GetMethods {
            private readonly V1Instance instance;

            internal GetMethods(V1Instance instance) {
                this.instance = instance;
            }

            #region Get By Filter

            private IAssetType ResolveAssetType(Type type) {
                return instance.MetaModel.GetAssetType(GetAssetTypeToken(type));
            }

            internal ICollection<T> Get<T>(EntityFilter filter) where T : Entity {
                // The returned entity type is determined by 
                // 1) the filter passed in or 2) the type of T if there is no filter.
                var targetEntityType = filter != null ? filter.EntityType : typeof (T);
                var type = ResolveAssetType(targetEntityType);
                var query = new Query(type);

                if (filter != null) {
                    var defaultToken = GetDefaultOrderByToken(targetEntityType);
                    IAttributeDefinition defaultOrderBy = null;

                    if(defaultToken != null) {
                        defaultOrderBy = instance.MetaModel.GetAttributeDefinition(defaultToken);
                    }

                    query.Filter = filter.BuildFilter(type, instance);
                    query.Find = filter.BuildFind(type);
                    query.OrderBy = filter.BuildOrderBy(type, defaultOrderBy);
                }

                return instance.QueryToEntityEnum<T>(query);
            }

            /// <summary>
            /// Get attachments filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Attachment> Attachments(AttachmentFilter filter) {
                return Get<Attachment>(filter ?? new AttachmentFilter());
            }

            /// <summary>
            /// Get notes filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Note> Notes(NoteFilter filter) {
                return Get<Note>(filter ?? new NoteFilter());
            }

            /// <summary>
            /// Get links filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Link> Links(LinkFilter filter) {
                return Get<Link>(filter ?? new LinkFilter());
            }

            /// <summary>
            /// Get effort records filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Effort> EffortRecords(EffortFilter filter) {
                return Get<Effort>(filter ?? new EffortFilter());
            }

            /// <summary>
            /// Get assets filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<BaseAsset> BaseAssets(BaseAssetFilter filter) {
                return Get<BaseAsset>(filter ?? new BaseAssetFilter());
            }

            /// <summary>
            /// Get stories filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Story> Stories(StoryFilter filter) {
                return Get<Story>(filter ?? new StoryFilter());
            }

            /// <summary>
            /// Get Epics filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Epic> Epics(EpicFilter filter) {
                return Get<Epic>(filter ?? new EpicFilter());
            }

            /// <summary>
            /// Get defects filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Defect> Defects(DefectFilter filter) {
                return Get<Defect>(filter ?? new DefectFilter());
            }

            /// <summary>
            /// Get primary workitems (stories and defects) filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<PrimaryWorkitem> PrimaryWorkitems(PrimaryWorkitemFilter filter) {
                return Get<PrimaryWorkitem>(filter ?? new PrimaryWorkitemFilter());
            }

            /// <summary>
            /// Get workitems (stories, defects, tasks, and tests) filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Workitem> Workitems(WorkitemFilter filter) {
                return Get<Workitem>(filter ?? new WorkitemFilter());
            }

            /// <summary>
            /// Get secondary workitems (tasks and tests) filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<SecondaryWorkitem> SecondaryWorkitems(SecondaryWorkitemFilter filter) {
                return Get<SecondaryWorkitem>(filter ?? new SecondaryWorkitemFilter());
            }

            /// <summary>
            /// Get tasks filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Task> Tasks(TaskFilter filter) {
                return Get<Task>(filter ?? new TaskFilter());
            }

            /// <summary>
            /// Get Tests filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Test> Tests(TestFilter filter) {
                return Get<Test>(filter ?? new TestFilter());
            }

            /// <summary>
            /// Get iterations filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Iteration> Iterations(IterationFilter filter) {
                return Get<Iteration>(filter ?? new IterationFilter());
            }

            /// <summary>
            /// Get projects filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Project> Projects(ProjectFilter filter) {
                return Get<Project>(filter ?? new ProjectFilter());
            }

            /// <summary>
            /// Get regression plans filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items are returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<RegressionPlan> RegressionPlans(RegressionPlanFilter filter) {
                return Get<RegressionPlan>(filter ?? new RegressionPlanFilter());
            }

            /// <summary>
            /// Get Regression Suite filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items are returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<RegressionSuite> RegressionSuites(RegressionSuiteFilter filter) {
                return Get<RegressionSuite>(filter ?? new RegressionSuiteFilter());
            }

            /// <summary>
            /// Get Regression Tests filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items are returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<RegressionTest> RegressionTests(RegressionTestFilter filter) {
                return Get<RegressionTest>(filter ?? new RegressionTestFilter());
            }

            /// <summary>
            /// Get Test Sets filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items are returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<TestSet> TestSets(TestSetFilter filter) {
                return Get<TestSet>(filter ?? new TestSetFilter());
            }

            /// <summary>
            /// Get Environment filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter"></param>
            /// <returns></returns>
            public ICollection<Environment> Environments(EnvironmentFilter filter) {
                return Get<Environment>(filter ?? new EnvironmentFilter());
            }

            /// <summary>
            /// Get schedules filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Schedule> Schedules(ScheduleFilter filter) {
                return Get<Schedule>(filter ?? new ScheduleFilter());
            }

            /// <summary>
            /// Get teams filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Team> Teams(TeamFilter filter) {
                return Get<Team>(filter ?? new TeamFilter());
            }

            /// <summary>
            /// Get themes filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Theme> Themes(ThemeFilter filter) {
                return Get<Theme>(filter ?? new ThemeFilter());
            }

            /// <summary>
            /// Get Members filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Member> Members(MemberFilter filter) {
                return Get<Member>(filter ?? new MemberFilter());
            }

            /// <summary>
            /// Get Requests filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Request> Requests(RequestFilter filter) {
                return Get<Request>(filter ?? new RequestFilter());
            }

            /// <summary>
            /// Get Goals filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Goal> Goals(GoalFilter filter) {
                return Get<Goal>(filter ?? new GoalFilter());
            }

            /// <summary>
            /// Get Issues filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Issue> Issues(IssueFilter filter) {
                return Get<Issue>(filter ?? new IssueFilter());
            }

            /// <summary>
            /// Get Retrospective filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Retrospective> Retrospectives(RetrospectiveFilter filter) {
                return Get<Retrospective>(filter ?? new RetrospectiveFilter());
            }

            /// <summary>
            /// Get Build Runs filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<BuildRun> BuildRuns(BuildRunFilter filter) {
                return Get<BuildRun>(filter ?? new BuildRunFilter());
            }

            /// <summary>
            /// Get Build Projects filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<BuildProject> BuildProjects(BuildProjectFilter filter) {
                return Get<BuildProject>(filter ?? new BuildProjectFilter());
            }

            /// <summary>
            /// Get ChangeSets filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<ChangeSet> ChangeSets(ChangeSetFilter filter) {
                return Get<ChangeSet>(filter ?? new ChangeSetFilter());
            }

            /// <summary>
            /// Get Messages filtered by the criteria specified in the passed in filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of items as specified in the filter.</returns>
            public ICollection<Message> Messages(MessageFilter filter) {
                return Get<Message>(filter ?? new MessageFilter());
            }

            /// <summary>
            /// Get Conversation filtered by the criteria specified in the passed filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of the items as specified in the filter.</returns>
            public ICollection<Conversation> Conversations(ConversationFilter filter) {
                return Get<Conversation>(filter ?? new ConversationFilter());
            }

            /// <summary>
            /// Get Expression filtered by the criteria specified in the passed filter.
            /// </summary>
            /// <param name="filter">Limit the items returned. If null, then all items returned.</param>
            /// <returns>ICollection of the items as specified in the filter.</returns>
            public ICollection<Expression> Expressions(ExpressionFilter filter)
            {
                return Get<Expression>(filter ?? new ExpressionFilter());
            }

            internal ICollection<MessageReceipt> MessageReceipts(MessageReceiptFilter filter) {
                return Get<MessageReceipt>(filter ?? new MessageReceiptFilter());
            }

            /// <summary>
            /// Get tracked Epics for enlisted Projects.
            /// </summary>
            public ICollection<Epic> TrackedEpics(ICollection<Project> projects) {
                return Get<Epic>(new TrackedEpicFilter(projects));
            } 

            #endregion

            #region Get By ID Methods

            /// <summary>
            /// Returns a project with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the project to retrieve</param>
            /// <returns>an instance of a Project or null if ID is invalid</returns>
            public Project ProjectByID(AssetID id) {
                return GetByID<Project>(id);
            }

            /// <summary>
            /// Returns a schedule with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the schedule to retrieve</param>
            /// <returns>an instance of a Schedule or null if ID is invalid</returns>
            public Schedule ScheduleByID(AssetID id) {
                return GetByID<Schedule>(id);
            }

            /// <summary>
            /// Returns an iteration with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the iteration to retrieve</param>
            /// <returns>an instance of an Iteration or null if ID is invalid</returns>
            public Iteration IterationByID(AssetID id) {
                return GetByID<Iteration>(id);
            }

            /// <summary>
            /// Returns a retrospective with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the iteration to retrieve</param>
            /// <returns>an instance of a retrospective or null if ID is invalid</returns>
            public Retrospective RetrospectiveByID(AssetID id) {
                return GetByID<Retrospective>(id);
            }

            /// <summary>
            /// Returns a Member with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Member to retrieve</param>
            /// <returns>an instance of a Member or null if ID is invalid</returns>
            public Member MemberByID(AssetID id) {
                return GetByID<Member>(id);
            }

            /// <summary>
            /// Returns a Team with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Team to retrieve</param>
            /// <returns>an instance of a Team or null if ID is invalid</returns>
            public Team TeamByID(AssetID id) {
                return GetByID<Team>(id);
            }

            /// <summary>
            /// Returns a Story with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Story to retrieve</param>
            /// <returns>an instance of a story or null if ID is invalid</returns>
            public Story StoryByID(AssetID id) {
                return GetByID<Story>(id);
            }

            /// <summary>
            /// Returns a Defect with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Defect to retrieve</param>
            /// <returns>an instance of a Defect or null if ID is invalid</returns>
            public Defect DefectByID(AssetID id) {
                return GetByID<Defect>(id);
            }

            /// <summary>
            /// Returns an Issue with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Issue to retrieve</param>
            /// <returns>an instance of a Issue or null if ID is invalid</returns>
            public Issue IssueByID(AssetID id) {
                return GetByID<Issue>(id);
            }

            /// <summary>
            /// Returns a Request with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Request to retrieve</param>
            /// <returns>an instance of a Request or null if ID is invalid</returns>
            public Request RequestByID(AssetID id) {
                return GetByID<Request>(id);
            }

            /// <summary>
            /// Returns a Theme with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Theme to retrieve</param>
            /// <returns>an instance of a Theme or null if ID is invalid</returns>
            public Theme ThemeByID(AssetID id) {
                return GetByID<Theme>(id);
            }

            /// <summary>
            /// Returns a Goal with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Goal to retrieve</param>
            /// <returns>an instance of a Goal or null if ID is invalid</returns>
            public Goal GoalByID(AssetID id) {
                return GetByID<Goal>(id);
            }

            /// <summary>
            /// Returns a Epic with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Epic to retrieve</param>
            /// <returns>an instance of a Epic or null if ID is invalid</returns>
            public Epic EpicByID(AssetID id) {
                return GetByID<Epic>(id);
            }

            /// <summary>
            /// Returns a StoryTemplate with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the StoryTemplate to retrieve</param>
            /// <returns>an instance of a StoryTemplate or null if ID is invalid</returns>
            public StoryTemplate StoryTemplateByID(AssetID id) {
                return GetByID<StoryTemplate>(id);
            }

            /// <summary>
            /// Returns a DefectTemplate with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the DefectTemplate to retrieve</param>
            /// <returns>an instance of a DefectTemplate or null if ID is invalid</returns>
            public DefectTemplate DefectTemplateByID(AssetID id) {
                return GetByID<DefectTemplate>(id);
            }

            /// <summary>
            /// Returns a Note with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Note to retrieve</param>
            /// <returns>an instance of a Note or null if ID is invalid</returns>
            public Note NoteByID(AssetID id) {
                return GetByID<Note>(id);
            }

            /// <summary>
            /// Returns a Link with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Link to retrieve</param>
            /// <returns>an instance of a Link or null if ID is invalid</returns>
            public Link LinkByID(AssetID id) {
                return GetByID<Link>(id);
            }

            /// <summary>
            /// Returns an Attachment with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Attachment to retrieve</param>
            /// <returns>an instance of an Attachment or null if ID is invalid</returns>
            public Attachment AttachmentByID(AssetID id) {
                return GetByID<Attachment>(id);
            }

            /// <summary>
            /// Returns a TestSuite with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the TestSuite to retrieve</param>
            /// <returns>an instance of a TestSuite or null if ID is invalid</returns>
            public TestSuite TestSuiteByID(AssetID id) {
                return GetByID<TestSuite>(id);
            }

            /// <summary>
            /// Returns an Effort Record with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Effort Record to retrieve</param>
            /// <returns>an instance of an Effort Record or null if ID is invalid</returns>
            public Effort EffortByID(AssetID id) {
                return GetByID<Effort>(id);
            }

            /// <summary>
            /// Returns a Primary Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Primary Workitem to retrieve</param>
            /// <returns>an instance of a Primary Workitem or null if ID is invalid</returns>
            public PrimaryWorkitem PrimaryWorkitemByID(AssetID id) {
                return GetByID<PrimaryWorkitem>(id);
            }

            /// <summary>
            /// Returns a Secondary Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Secondary Workitem to retrieve</param>
            /// <returns>an instance of a Secondary Workitem or null if ID is invalid</returns>
            public SecondaryWorkitem SecondaryWorkitemByID(AssetID id) {
                return GetByID<SecondaryWorkitem>(id);
            }

            /// <summary>
            /// Returns a Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Workitem to retrieve</param>
            /// <returns>an instance of a Workitem or null if ID is invalid</returns>
            public Workitem WorkitemByID(AssetID id) {
                return GetByID<Workitem>(id);
            }

            /// <summary>
            /// Returns a BaseAsset with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the BaseAsset to retrieve</param>
            /// <returns>an instance of a BaseAsset or null if ID is invalid</returns>
            public BaseAsset BaseAssetByID(AssetID id) {
                return GetByID<BaseAsset>(id);
            }

            /// <summary>
            /// Returns a Build Run with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Build Run to retrieve</param>
            /// <returns>an instance of a Build Run or null if ID is invalid</returns>
            public BuildRun BuildRunByID(AssetID id) {
                return GetByID<BuildRun>(id);
            }

            /// <summary>
            /// Returns a Build Project with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Build Project to retrieve</param>
            /// <returns>an instance of a Build Project or null if ID is invalid</returns>
            public BuildProject BuildProjectByID(AssetID id) {
                return GetByID<BuildProject>(id);
            }

            /// <summary>
            /// Returns a ChangeSet with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the ChangeSet to retrieve</param>
            /// <returns>an instance of a ChangeSet or null if ID is invalid</returns>
            public ChangeSet ChangeSetByID(AssetID id) {
                return GetByID<ChangeSet>(id);
            }

            /// <summary>
            /// Returns a RegressionPlan with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the RegressionPlan to retrieve</param>
            /// <returns>an instance of a RegressionPlan or null if ID is invalid</returns>
            public RegressionPlan RegressionPlanByID(AssetID id) {
                return GetByID<RegressionPlan>(id);
            }

            /// <summary>
            /// Returns a Regression Suite with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Regression Suite to retrieve</param>
            /// <returns>an instance of a Regression Suite or null if ID is invalid</returns>
            public RegressionSuite RegressionSuiteByID(AssetID id) {
                return GetByID<RegressionSuite>(id);
            }

            /// <summary>
            /// Returns a Regression Test with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Regression Test to retrieve</param>
            /// <returns>an instance of a Regression Test or null if ID is invalid</returns>
            public RegressionTest RegressionTestByID(AssetID id) {
                return GetByID<RegressionTest>(id);
            }

            /// <summary>
            /// Returns a Test Set with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Test Set to retrieve</param>
            /// <returns>an instance of a Test Set or null if ID is invalid</returns>
            public TestSet TestSetByID(AssetID id) {
                return GetByID<TestSet>(id);
            }

            /// <summary>
            /// Returns a Environment with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="id">ID of the Environment to retrieve</param>
            /// <returns>an instance of a Environment or null if ID is invalid</returns>
            public Environment EnvironmentByID(AssetID id) {
                return GetByID<Environment>(id);
            }

            /// <summary>
            /// Returns a Conversation with the given ID or null if the ID is invalid.
            /// </summary>
            /// <param name="id">ID of the Conversation to retrieve.</param>
            /// <returns>an instance of an Conversation or null if ID is invalid.</returns>
            public Conversation ConversationByID(AssetID id) {
                return GetByID<Conversation>(id);
            }

            /// <summary>
            /// Returns a Expression with the given ID or null if the ID is invalid.
            /// </summary>
            /// <param name="id">ID of the Expression to retrieve.</param>
            /// <returns>an instance of an Expression or null if ID is invalid.</returns>
            public Expression ExpressionByID(AssetID id)
            {
                return GetByID<Expression>(id);
            }

            /// <summary>
            /// Returns an Entity of Type T with the given ID or null if the ID is invalid
            /// </summary>
            /// <typeparam name="T">Entity Type to retrieve</typeparam>
            /// <param name="id">ID of the Entity to retrieve</param>
            /// <returns>an instance of an Entity of Type T or null if ID is invalid</returns>
            public T GetByID<T>(AssetID id) where T : Entity {
                return instance.wrapperManager.Create<T>(id, true);
            }

            #endregion

            #region GetByDisplayID

            /// <summary>
            /// Returns an Entity of Type T with the given ID or null if the ID is invalid
            /// </summary>
            /// <typeparam name="T">Entity Type to retrieve</typeparam>
            /// <param name="displayID">DisplayID of the Entity to retrieve</param>
            /// <returns>an instance of an Entity of Type T or null if ID is invalid</returns>
            public T GetByDisplayID<T>(string displayID) where T : ProjectAsset {
                var assetTypeToken = GetAssetTypeToken(typeof (T));
                var projectAssetType = instance.MetaModel.GetAssetType(assetTypeToken);
                var idDef = projectAssetType.GetAttributeDefinition("Number");

                var query = new Query(projectAssetType);
                var idTerm = new FilterTerm(idDef);
                idTerm.Equal(displayID);
                query.Filter = idTerm;

                var result = instance.Services.Retrieve(query);
                
                if(result.Assets.Count == 0) {
                    return null;
                }
                
                var asset = result.Assets[0];

                // need to validate here to make sure the DisplayID is for the proper type
                // This is a problem with Epics and Stories.
                return instance.wrapperManager.Create<T>(asset.Oid.Token, true);
            }

            /// <summary>
            /// Returns a Story with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Story to retrieve</param>
            /// <returns>an instance of a story or null if ID is invalid</returns>
            public Story StoryByDisplayID(string displayID) {
                return GetByDisplayID<Story>(displayID);
            }

            /// <summary>
            /// Returns a Defect with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Defect to retrieve</param>
            /// <returns>an instance of a Defect or null if ID is invalid</returns>
            public Defect DefectByDisplayID(string displayID) {
                return GetByDisplayID<Defect>(displayID);
            }

            /// <summary>
            /// Returns a TestSet with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the TestSet to retrieve</param>
            /// <returns>an instance of a TestSet or null if ID is invalid</returns>
            public TestSet TestSetByDisplayID(string displayID) {
                return GetByDisplayID<TestSet>(displayID);
            }

            /// <summary>
            /// Returns an Issue with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Issue to retrieve</param>
            /// <returns>an instance of a Issue or null if ID is invalid</returns>
            public Issue IssueByDisplayID(string displayID) {
                return GetByDisplayID<Issue>(displayID);
            }

            /// <summary>
            /// Returns a Request with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Request to retrieve</param>
            /// <returns>an instance of a Request or null if ID is invalid</returns>
            public Request RequestByDisplayID(string displayID) {
                return GetByDisplayID<Request>(displayID);
            }

            /// <summary>
            /// Returns a Theme with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Theme to retrieve</param>
            /// <returns>an instance of a Theme or null if ID is invalid</returns>
            public Theme ThemeByDisplayID(string displayID) {
                return GetByDisplayID<Theme>(displayID);
            }

            /// <summary>
            /// Returns a Goal with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Goal to retrieve</param>
            /// <returns>an instance of a Goal or null if ID is invalid</returns>
            public Goal GoalByDisplayID(string displayID) {
                return GetByDisplayID<Goal>(displayID);
            }

            /// <summary>
            /// Returns a Epic with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Epic to retrieve</param>
            /// <returns>an instance of a Epic or null if ID is invalid</returns>
            public Epic EpicByDisplayID(string displayID) {
                return GetByDisplayID<Epic>(displayID);
            }

            /// <summary>
            /// Returns a Primary Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Primary Workitem to retrieve</param>
            /// <returns>an instance of a Primary Workitem or null if ID is invalid</returns>
            public PrimaryWorkitem PrimaryWorkitemByDisplayID(string displayID) {
                return GetByDisplayID<PrimaryWorkitem>(displayID);
            }

            /// <summary>
            /// Returns a Secondary Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Secondary Workitem to retrieve</param>
            /// <returns>an instance of a Secondary Workitem or null if ID is invalid</returns>
            public SecondaryWorkitem SecondaryWorkitemByDisplayID(string displayID) {
                return GetByDisplayID<SecondaryWorkitem>(displayID);
            }

            /// <summary>
            /// Returns a Workitem with the given ID or null if the ID is invalid
            /// </summary>
            /// <param name="displayID">DisplayID of the Workitem to retrieve</param>
            /// <returns>an instance of a Workitem or null if ID is invalid</returns>
            public Workitem WorkitemByDisplayID(string displayID) {
                return GetByDisplayID<Workitem>(displayID);
            }

            #endregion

            #region GetByName, Etc.

            /// <summary>
            /// Retrieves the first schedule with the given name or null
            /// </summary>
            /// <param name="name">name of the schedule to retrieve</param>
            /// <returns>the first instance of a Schedule that matches name or null</returns>
            public Schedule ScheduleByName(string name) {
                var scheduleAssetType = instance.MetaModel.GetAssetType("Schedule");
                var nameDef = scheduleAssetType.GetAttributeDefinition("Name");

                var query = new Query(scheduleAssetType);
                var nameTerm = new FilterTerm(nameDef);
                nameTerm.Equal(name);
                query.Filter = nameTerm;
                query.OrderBy.MajorSort(nameDef, OrderBy.Order.Ascending);

                var result = instance.Services.Retrieve(query);

                if(result.Assets.Count == 0) {
                    return null;
                }

                var asset = result.Assets[0];
                return new Schedule(new AssetID(asset.Oid.Token), instance);
            }

            /// <summary>
            /// Retrieves the first project with the given name or null
            /// </summary>
            /// <param name="name">name of the project to retrieve</param>
            /// <returns>the first instance of a Project that matches name or null</returns>
            public Project ProjectByName(string name) {
                var projectAssetType = instance.MetaModel.GetAssetType("Scope");
                var nameDef = projectAssetType.GetAttributeDefinition("Name");

                var query = new Query(projectAssetType);
                var nameTerm = new FilterTerm(nameDef);
                nameTerm.Equal(name);
                query.Filter = nameTerm;
                query.OrderBy.MajorSort(nameDef, OrderBy.Order.Ascending);

                var result = instance.Services.Retrieve(query);
                
                if(result.Assets.Count == 0) {
                    return null;
                }
                
                var asset = result.Assets[0];
                return new Project(new AssetID(asset.Oid.Token), instance);
            }

            /// <summary>
            /// Retrieves the first Member with the given username
            /// </summary>
            /// <param name="userName">The username the user or member uses to login to the VersionOne system</param>
            /// <returns>The first Member with the given username, or null if none found</returns>
            public Member MemberByUserName(string userName) {
                var memberAssetType = instance.MetaModel.GetAssetType("Member");
                var nameDef = memberAssetType.GetAttributeDefinition("Username");

                var query = new Query(memberAssetType);
                var usernameTerm = new FilterTerm(nameDef);
                usernameTerm.Equal(userName);
                query.Filter = usernameTerm;
                query.OrderBy.MajorSort(nameDef, OrderBy.Order.Ascending);

                var result = instance.Services.Retrieve(query);

                if(result.Assets.Count == 0) {
                    return null;
                }

                var asset = result.Assets[0];
                return new Member(new AssetID(asset.Oid.Token), instance);
            }

            #endregion

            /// <summary>
            /// Gets the active values of a standard list type.
            /// </summary>
            /// <typeparam name="T">The type of Entity that represents the V1 List Type.</typeparam>
            /// <returns>A list of active values for this list type.</returns>
            public IEnumerable<T> ListTypeValues<T>() where T : ListValue {
                var typeToGet = instance.MetaModel.GetAssetType(GetAssetTypeToken(typeof (T)));

                var query = new Query(typeToGet);
                var assetStateTerm = new FilterTerm(typeToGet.GetAttributeDefinition("AssetState"));
                assetStateTerm.NotEqual(AssetState.Closed);
                query.Filter = new AndFilterTerm(assetStateTerm);

                return instance.QueryToEntityEnum<T>(query);
            }
        }
    }
}