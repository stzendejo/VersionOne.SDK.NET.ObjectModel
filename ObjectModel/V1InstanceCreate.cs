using System;
using System.Collections.Generic;
using System.IO;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel {
    public partial class V1Instance {
        /// <summary>
        /// Create various assets in the system
        /// </summary>
        public CreateMethods Create {
            get {
                if(create == null) {
                    create = new CreateMethods(this);
                }
                return create;
            }
        }

        private CreateMethods create;


        /// <summary>
        /// Create Assets
        /// </summary>
        public class CreateMethods {
            private readonly V1Instance instance;

            internal CreateMethods(V1Instance instance) {
                this.instance = instance;
            }

            /// <summary>
            /// Create a new schedule entity with a name, iteration length, and iteration gap
            /// </summary>
            /// <param name="name">Name of the new schedule</param>
            /// <param name="iterationLength">The duration an iteration will last in this schedule</param>
            /// <param name="iterationGap">The duration between iterations in this schedule.</param>
            /// <returns>A newly minted Schedule that exists in the VersionOne system.</returns>
            public Schedule Schedule(string name, Duration iterationLength, Duration iterationGap) {
                return Schedule(name, iterationLength, iterationGap, null);
            }

            /// <summary>
            /// Create a new schedule entity with required attributes.
            /// </summary>
            /// <param name="name">Name of the new schedule</param>
            /// <param name="iterationLength">The duration an iteration will last in this schedule</param>
            /// <param name="iterationGap">The duration between iterations in this schedule.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Schedule that exists in the VersionOne system.</returns>
            public Schedule Schedule(string name, Duration iterationLength, Duration iterationGap,
                IDictionary<string, object> attributes) {
                var schedule = new Schedule(instance) {
                    Name = name, 
                    IterationLength = iterationLength, 
                    IterationGap = iterationGap
                };
                AddAttributes(schedule, attributes);
                schedule.Save();
                return schedule;
            }

            /// <summary>
            /// Create a new project entity with a name, parent project, begin date, and optional schedule
            /// </summary>
            /// <param name="name"></param>
            /// <param name="parentProject"></param>
            /// <param name="beginDate"></param>
            /// <param name="schedule"></param>
            /// <returns>A newly minted Project that exists in the VersionOne system.</returns>
            public Project Project(string name, Project parentProject, DateTime beginDate, Schedule schedule) {
                return Project(name, parentProject, beginDate, schedule, null);
            }

            /// <summary>
            /// Create a new project entity with a name, parent project, begin date, and optional schedule
            /// </summary>
            /// <param name="name"></param>
            /// <param name="parentProject"></param>
            /// <param name="beginDate"></param>
            /// <param name="schedule"></param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Project that exists in the VersionOne system.</returns>
            public Project Project(string name, Project parentProject, DateTime beginDate, Schedule schedule,
                IDictionary<string, object> attributes) {
                var project = new Project(instance) {
                    Name = name, 
                    ParentProject = parentProject, 
                    BeginDate = beginDate, 
                    Schedule = schedule
                };
                AddAttributes(project, attributes);
                project.Save();
                return project;
            }

            /// <summary>
            /// Create a new project entity with a name, parent project, begin date, and optional schedule
            /// </summary>
            /// <param name="name"></param>
            /// <param name="parentProjectID"></param>
            /// <param name="beginDate"></param>
            /// <param name="schedule"></param>
            /// <returns>A newly minted Project that exists in the VersionOne system.</returns>
            public Project Project(string name, AssetID parentProjectID, DateTime beginDate, Schedule schedule) {
                return Project(name, new Project(parentProjectID, instance), beginDate, schedule);
            }

            /// <summary>
            /// Create a new project entity with a name, parent project, begin date, and optional schedule
            /// </summary>
            /// <param name="name"></param>
            /// <param name="parentProjectID"></param>
            /// <param name="beginDate"></param>
            /// <param name="schedule"></param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Project that exists in the VersionOne system.</returns>
            public Project Project(string name, AssetID parentProjectID, DateTime beginDate, Schedule schedule,
                IDictionary<string, object> attributes) {
                return Project(name, new Project(parentProjectID, instance), beginDate, schedule, attributes);
            }

            /// <summary>
            /// Create a new member entity with a name, short name, and default role
            /// </summary>
            /// <param name="name">The full name of the user.</param>
            /// <param name="shortName">An alias or nickname used throughout the VersionOne user interface.</param>
            /// <param name="defaultRole">The new user's default role on projects.</param>
            /// <returns>A newly minted Member that exists in the VersionOne system.</returns>
            public Member Member(string name, string shortName, Role defaultRole) {
                return Member(name, shortName, defaultRole, null);
            }

            /// <summary>
            /// Create a new member entity with a name, short name, and default role
            /// </summary>
            /// <param name="name">The full name of the user.</param>
            /// <param name="shortName">An alias or nickname used throughout the VersionOne user interface.</param>
            /// <param name="defaultRole">The new user's default role on projects.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Member that exists in the VersionOne system.</returns>
            public Member Member(string name, string shortName, Role defaultRole,
                IDictionary<string, object> attributes) {
                var member = new Member(instance) {
                    Name = name, 
                    ShortName = shortName, 
                    DefaultRole = defaultRole
                };
                AddAttributes(member, attributes);
                member.Save();
                return member;
            }

            /// <summary>
            /// Create a new member entity with a name and short name
            /// </summary>
            /// <param name="name">The full name of the user.</param>
            /// <param name="shortName">An alias or nickname used throughout the VersionOne user interface.</param>
            /// <returns>A newly minted Member that exists in the VersionOne system.</returns>
            public Member Member(string name, string shortName) {
                return Member(name, shortName, Role.TeamMember);
            }

            /// <summary>
            /// Create a new member entity with a name and short name
            /// </summary>
            /// <param name="name">The full name of the user.</param>
            /// <param name="shortName">An alias or nickname used throughout the VersionOne user interface.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Member that exists in the VersionOne system.</returns>
            public Member Member(string name, string shortName, IDictionary<string, object> attributes) {
                return Member(name, shortName, Role.TeamMember, attributes);
            }

            /// <summary>
            /// Create a new team entity with a name
            /// </summary>
            /// <param name="name"></param>
            /// <returns>A newly minted Team that exists in the VersionOne system.</returns>
            public Team Team(string name) {
                return Team(name, null);
            }

            /// <summary>
            /// Create a new team entity with a name
            /// </summary>
            /// <param name="name"></param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Team that exists in the VersionOne system.</returns>
            public Team Team(string name, IDictionary<string, object> attributes) {
                var team = new Team(instance) {
                    Name = name
                };
                AddAttributes(team, attributes);
                team.Save();
                return team;
            }

            /// <summary>
            /// Create a new Story with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Story will be in.</param>
            /// <returns>A newly minted Story that exists in the VersionOne system.</returns>
            public Story Story(string name, Project project) {
                return Story(name, project, null);
            }

            /// <summary>
            /// Create a new Story with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Story will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Story that exists in the VersionOne system.</returns>
            public Story Story(string name, Project project, IDictionary<string, object> attributes) {
                var story = new Story(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(story, attributes);
                story.Save();
                return story;
            }

            /// <summary>
            /// Create a new story with a name. Set the story's IdentifiedIn to the given retrospective and the project to the retrospective's project.
            /// </summary>
            /// <param name="name">The initial name of the story.</param>
            /// <param name="retrospective">The retrospective this story was identified in.</param>
            /// <returns>A newly minted Story that exists in the VersionOne system.</returns>
            public Story Story(string name, Retrospective retrospective) {
                return Story(name, retrospective, null);
            }

            /// <summary>
            /// Create a new story with a name. Set the story's IdentifiedIn to the given retrospective and the project to the retrospective's project.
            /// </summary>
            /// <param name="name">The initial name of the story.</param>
            /// <param name="retrospective">The retrospective this story was identified in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Story that exists in the VersionOne system.</returns>
            public Story Story(string name, Retrospective retrospective, IDictionary<string, object> attributes) {
                var story = new Story(instance) {
                    Name = name, 
                    IdentifiedIn = retrospective, 
                    Project = retrospective.Project
                };
                AddAttributes(story, attributes);
                story.Save();
                return story;
            }

            /// <summary>
            /// Create a new Defect with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Defect will be in.</param>
            /// <returns>A newly minted Defect that exists in the VersionOne system.</returns>
            public Defect Defect(string name, Project project) {
                return Defect(name, project, null);
            }

            /// <summary>
            /// Create a new Defect with required attributes.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Defect will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns></returns>
            public Defect Defect(string name, Project project, IDictionary<string, object> attributes) {
                var defect = new Defect(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(defect, attributes);
                defect.Save();
                return defect;
            }

            /// <summary>
            /// Create a new Task with a name. Assign it to the given primary workitem.
            /// </summary>
            /// <param name="name">The initial name of the task.</param>
            /// <param name="primaryWorkitem">The PrimaryWorkitem this Task will belong to.</param>
            /// <returns>A newly minted Task that exists in the VersionOne system.</returns>
            public Task Task(string name, PrimaryWorkitem primaryWorkitem) {
                return Task(name, primaryWorkitem, null);
            }

            /// <summary>
            /// Create a new Task with required attributes. Assign it to the given primary workitem.
            /// </summary>
            /// <param name="name">The initial name of the task.</param>
            /// <param name="primaryWorkitem">The PrimaryWorkitem this Task will belong to.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Task that exists in the VersionOne system.</returns>
            public Task Task(string name, PrimaryWorkitem primaryWorkitem, IDictionary<string, object> attributes) {
                var task = new Task(instance) {
                    Name = name, 
                    Parent = primaryWorkitem
                };
                AddAttributes(task, attributes);
                task.Save();
                return task;
            }

            /// <summary>
            /// Create a new Test with a name. Assign it to the given primary workitem.
            /// </summary>
            /// <param name="name">The initial name of the test.</param>
            /// <param name="workitem">The Workitem(Epic, Story, Defect) this Test will belong to.</param>
            /// <returns>A newly minted Test that exists in the VersionOne system.</returns>
            public Test Test(string name, Workitem workitem) {
                return Test(name, workitem, null);
            }

            /// <summary>
            /// Create a new Test with required attributes. Assign it to the given primary workitem.
            /// </summary>
            /// <param name="name">The initial name of the test.</param>
            /// <param name="workitem">The Workitem(Epic, Story, Defect) this Test will belong to.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Test that exists in the VersionOne system.</returns>
            public Test Test(string name, Workitem workitem, IDictionary<string, object> attributes) {
                var test = new Test(instance) {
                    Name = name, 
                    Parent = workitem
                };
                AddAttributes(test, attributes);
                test.Save();
                return test;
            }

            /// <summary>
            /// Create a new Theme with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Theme will be in.</param>
            /// <returns>A newly minted Theme that exists in the VersionOne system.</returns>
            public Theme Theme(string name, Project project) {
                return Theme(name, project, null);
            }

            /// <summary>
            /// Create a new Theme with required attributes.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Theme will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Theme that exists in the VersionOne system.</returns>
            public Theme Theme(string name, Project project, IDictionary<string, object> attributes) {
                var theme = new Theme(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(theme, attributes);
                theme.Save();
                return theme;
            }

            /// <summary>
            /// Create a new Goal with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Goal will be in.</param>
            /// <returns>A newly minted Goal that exists in the VersionOne system.</returns>
            public Goal Goal(string name, Project project) {
                return Goal(name, project, null);
            }

            /// <summary>
            /// Create a new Goal with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Goal will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Goal that exists in the VersionOne system.</returns>
            public Goal Goal(string name, Project project, IDictionary<string, object> attributes) {
                var goal = new Goal(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(goal, attributes);
                goal.Save();
                return goal;
            }

            /// <summary>
            /// Create a new Issue with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Issue will be in.</param>
            /// <returns>A newly minted Issue that exists in the VersionOne system.</returns>
            public Issue Issue(string name, Project project) {
                return Issue(name, project, null);
            }

            /// <summary>
            /// Create a new Issue with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Issue will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Issue that exists in the VersionOne system.</returns>
            public Issue Issue(string name, Project project, IDictionary<string, object> attributes) {
                var issue = new Issue(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(issue, attributes);
                issue.Save();
                return issue;
            }

            /// <summary>
            /// Creates an Issue related to a Retrospective
            /// </summary>
            /// <param name="name">The name of the Issue</param>
            /// <param name="retrospective">The Retrospective to relate the Issue to</param>
            /// <returns>The newly saved Issue</returns>
            public Issue Issue(string name, Retrospective retrospective) {
                return Issue(name, retrospective, null);
            }

            /// <summary>
            /// Creates an Issue related to a Retrospective
            /// </summary>
            /// <param name="name">The name of the Issue</param>
            /// <param name="retrospective">The Retrospective to relate the Issue to</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>The newly saved Issue</returns>
            public Issue Issue(string name, Retrospective retrospective, IDictionary<string, object> attributes) {
                var issue = new Issue(instance) {
                    Name = name, 
                    Project = retrospective.Project
                };
                AddAttributes(issue, attributes);
                issue.Save();
                //TODO verify sequence
                issue.Retrospectives.Add(retrospective);
                return issue;
            }

            /// <summary>
            /// Create a new Request with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Request will be in.</param>
            /// <returns>A newly minted Request that exists in the VersionOne system.</returns>
            public Request Request(string name, Project project) {
                return Request(name, project, null);
            }

            /// <summary>
            /// Create a new Request with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Request will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Request that exists in the VersionOne system.</returns>
            public Request Request(string name, Project project, IDictionary<string, object> attributes) {
                var request = new Request(instance);
                AddAttributes(request, attributes);
                request.Name = name;
                request.Project = project;
                AddAttributes(request, attributes);
                request.Save();
                return request;
            }

            /// <summary>
            /// Create a new Epic with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Epic will be in.</param>
            /// <returns>A newly minted Epic that exists in the VersionOne system.</returns>
            public Epic Epic(string name, Project project) {
                return Epic(name, project, null);
            }

            /// <summary>
            /// Create a new Epic with a name.
            /// </summary>
            /// <param name="name">The initial name of the entity.</param>
            /// <param name="project">The Project this Epic will be in.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Epic that exists in the VersionOne system.</returns>
            public Epic Epic(string name, Project project, IDictionary<string, object> attributes) {
                var epic = new Epic(instance) {
                    Name = name, 
                    Project = project
                };
                AddAttributes(epic, attributes);
                epic.Save();

                return epic;
            }

            /// <summary>
            /// Create a new link with a name
            /// </summary>
            /// <param name="name">The initial name of the link</param>
            /// <param name="asset">The asset this link belongs to</param>
            /// <param name="url">The URL of the link</param>
            /// <param name="onMenu">True to show on the asset's detail page menu</param>
            /// <returns>A newly minted Link that exists in the VersionOne system.</returns>
            public Link Link(string name, BaseAsset asset, string url, bool onMenu) {
                return Link(name, asset, url, onMenu, null);
            }

            /// <summary>
            /// Create a new link with a name
            /// </summary>
            /// <param name="name">The initial name of the link</param>
            /// <param name="asset">The asset this link belongs to</param>
            /// <param name="url">The URL of the link</param>
            /// <param name="onMenu">True to show on the asset's detail page menu</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Link that exists in the VersionOne system.</returns>
            public Link Link(string name, BaseAsset asset, string url, bool onMenu,
                IDictionary<string, object> attributes) {
                var link = new Link(instance) {
                    Asset = asset, 
                    Name = name, 
                    URL = url, 
                    OnMenu = onMenu
                };
                AddAttributes(link, attributes);
                link.Save();
                return link;
            }

            /// <summary>
            /// Create a new n0te with a name, asset, content, and 'personal' flag 
            /// </summary>
            /// <param name="name">The initial name of the n0te</param>
            /// <param name="asset">The asset this n0te belongs to</param>
            /// <param name="content">The content of the n0te</param>
            /// <param name="personal">True if this n0te is only visible to </param>
            /// <returns>A newly minted n0te that exists in the VersionOne system.</returns>
            public Note Note(string name, BaseAsset asset, string content, bool personal) {
                return Note(name, asset, content, personal, null);
            }

            /// <summary>
            /// Create a new n0te with a name, asset, content, and 'personal' flag 
            /// </summary>
            /// <param name="name">The initial name of the n0te</param>
            /// <param name="asset">The asset this n0te belongs to</param>
            /// <param name="content">The content of the n0te</param>
            /// <param name="personal">True if this n0te is only visible to </param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted n0te that exists in the VersionOne system.</returns>
            public Note Note(string name, BaseAsset asset, string content, bool personal,
                IDictionary<string, object> attributes) {
                var note = new Note(instance) {
                    Asset = asset, 
                    Name = name, 
                    Content = content, 
                    Personal = personal
                };
                AddAttributes(note, attributes);
                note.Save();
                return note;
            }

            /// <summary>
            /// Create a new response to an existing n0te with a name, content, and 'personal' flag 
            /// </summary>
            /// <param name="responseTo">The n0te to respond to</param>
            /// <param name="name">The initial name of the n0te</param>
            /// <param name="content">The content of the n0te</param>
            /// <param name="personal">True if this n0te is only visible to </param>
            /// <returns>A newly minted n0te in response to the original one that exists in the VersionOne system.</returns>
            public Note Note(Note responseTo, string name, string content, bool personal) {
                return Note(responseTo, name, content, personal, null);
            }

            /// <summary>
            /// Create a new response to an existing n0te with a name, content, and 'personal' flag 
            /// </summary>
            /// <param name="responseTo">The n0te to respond to</param>
            /// <param name="name">The initial name of the n0te</param>
            /// <param name="content">The content of the n0te</param>
            /// <param name="personal">True if this n0te is only visible to </param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted n0te in response to the original one that exists in the VersionOne system.</returns>
            public Note Note(Note responseTo, string name, string content, bool personal,
                IDictionary<string, object> attributes) {
                var note = new Note(responseTo, instance) {
                    Name = name, 
                    Content = content, 
                    Personal = personal
                };
                AddAttributes(note, attributes);
                note.Save();
                return note;
            }

            /// <summary>
            /// Create a new TestSuite with a name
            /// </summary>
            /// <param name="name">The initial name of the TestSuite</param>
            /// <returns>A newly minted TestSuite that exists in the VersionOne system.</returns>
            /// <param name="reference">A free text field used for reference (perhaps to an external system)</param>
            public TestSuite TestSuite(string name, string reference) {
                return TestSuite(name, reference, null);
            }

            /// <summary>
            /// Create a new TestSuite with a name
            /// </summary>
            /// <param name="name">The initial name of the TestSuite</param>
            /// <returns>A newly minted TestSuite that exists in the VersionOne system.</returns>
            /// <param name="attributes">Required attributes.</param>
            /// <param name="reference">A free text field used for reference (perhaps to an external system)</param>
            public TestSuite TestSuite(string name, string reference, IDictionary<string, object> attributes) {
                var testSuite = new TestSuite(instance) {
                    Name = name, 
                    Reference = reference
                };
                AddAttributes(testSuite, attributes);
                testSuite.Save();
                return testSuite;
            }

            /// <summary>
            /// Create a new attachment with a name
            /// </summary>
            /// <param name="name">The name of the attachment</param>
            /// <param name="asset">The asset this attachment belongs to</param>
            /// <param name="filename">The name of the original attachment file</param>
            /// <param name="stream">The read-enabled stream that contains the attachment content to upload</param>
            /// <returns>A newly minted Attachment that exists in the VersionOne system.</returns>
            public Attachment Attachment(string name, BaseAsset asset, string filename, Stream stream) {
                return Attachment(name, asset, filename, stream, null);
            }

            /// <summary>
            /// Create a new attachment with a name
            /// </summary>
            /// <param name="name">The name of the attachment</param>
            /// <param name="asset">The asset this attachment belongs to</param>
            /// <param name="filename">The name of the original attachment file</param>
            /// <param name="stream">The read-enabled stream that contains the attachment content to upload</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Attachment that exists in the VersionOne system.</returns>
            public Attachment Attachment(string name, BaseAsset asset, string filename, Stream stream,
                IDictionary<string, object> attributes) {
                var attachment = new Attachment(instance) {
                    Asset = asset, 
                    Name = name, 
                    Filename = filename, 
                    ContentType = MimeType.Resolve(filename)
                };
                AddAttributes(attachment, attributes);
                attachment.Save();

                if(stream != null) {
                    attachment.ReadFrom(stream);
                }

                return attachment;
            }

            /// <summary>
            /// Create a new retrospective with a name
            /// </summary>
            /// <param name="name">The name of the retrospective</param>
            /// <param name="project">The project this retrospective belongs to</param>
            /// <returns>A newly minted Retrospective that exists in the VersionOne system.</returns>
            public Retrospective Retrospective(string name, Project project) {
                return Retrospective(name, project, null);
            }

            /// <summary>
            /// Create a new retrospective with a name
            /// </summary>
            /// <param name="name">The name of the retrospective</param>
            /// <param name="project">The project this retrospective belongs to</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Retrospective that exists in the VersionOne system.</returns>
            public Retrospective Retrospective(string name, Project project, IDictionary<string, object> attributes) {
                var retro = new Retrospective(instance) {
                    Project = project, 
                    Name = name
                };
                AddAttributes(retro, attributes);
                retro.Save();
                return retro;
            }

            /// <summary>
            /// Create a new iteration using suggested system values.
            /// </summary>
            /// <param name="project">The project to use to determine the schedule this iteration belongs to.</param>
            /// <returns>A newly minted Iteration that exists in the VersionOne system.</returns>
            public Iteration Iteration(Project project) {
                return Iteration(project, null);
            }

            /// <summary>
            /// Create a new iteration using suggested system values.
            /// </summary>
            /// <param name="project">The project to use to determine the schedule this iteration belongs to.</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Iteration that exists in the VersionOne system.</returns>
            public Iteration Iteration(Project project, IDictionary<string, object> attributes) {
                var iteration = instance.New<Iteration>(project);
                AddAttributes(iteration, attributes);
                iteration.Save();
                // TODO: Fix bug on the backend.
                iteration.MakeFuture();
                return iteration;
            }

            /// <summary>
            /// Create a new iteration with a name, begin date, and end date
            /// </summary>
            /// <param name="name">The name of the iteration</param>
            /// <param name="schedule">The schedule this iteration belongs to</param>
            /// <param name="beginDate">The begin date or start date of this iteration</param>
            /// <param name="endDate">The end date of this iteration</param>
            /// <returns>A newly minted Iteration that exists in the VersionOne system.</returns>
            public Iteration Iteration(string name, Schedule schedule, DateTime beginDate, DateTime endDate) {
                return Iteration(name, schedule, beginDate, endDate, null);
            }

            /// <summary>
            /// Create a new iteration with a name, begin date, and end date
            /// </summary>
            /// <param name="name">The name of the iteration</param>
            /// <param name="schedule">The schedule this iteration belongs to</param>
            /// <param name="beginDate">The begin date or start date of this iteration</param>
            /// <param name="endDate">The end date of this iteration</param>
            /// <param name="attributes">Required attributes.</param>
            /// <returns>A newly minted Iteration that exists in the VersionOne system.</returns>
            public Iteration Iteration(string name, Schedule schedule, DateTime beginDate, DateTime endDate,
                IDictionary<string, object> attributes) {
                var iteration = new Iteration(instance) {
                    Name = name, 
                    Schedule = schedule, 
                    BeginDate = beginDate, 
                    EndDate = endDate
                };
                AddAttributes(iteration, attributes);
                iteration.Save();
                return iteration;
            }

            /// <summary>
            /// Create a new effort record with a value and date, assigned to the given workitem and member
            /// </summary>
            /// <param name="value">The value of the effort record</param>
            /// <param name="item">The workitem to assign the effort record to</param>
            /// <param name="member">The member to assign the effort record to</param>
            /// <param name="date">The date to log the effort record against</param>
            /// <returns>A newly minted Effort Record that exists in the VersionOne system.</returns>
            /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
            public Effort Effort(double value, Workitem item, Member member, DateTime date) {
                return Effort(value, item, member, date, null);
            }

            /// <summary>
            /// Create a new effort record with a value and date, assigned to the given workitem and member
            /// </summary>
            /// <param name="value">The value of the effort record</param>
            /// <param name="item">The workitem to assign the effort record to</param>
            /// <param name="member">The member to assign the effort record to</param>
            /// <param name="date">The date to log the effort record against</param>
            /// <returns>A newly minted Effort Record that exists in the VersionOne system.</returns>
            /// <param name="attributes">Required attributes.</param>
            /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
            public Effort Effort(double value, Workitem item, Member member, DateTime date,
                IDictionary<string, object> attributes) {
                if(!instance.Configuration.EffortTrackingEnabled) {
                    throw new InvalidOperationException("Effort Tracking is disabled.");
                }

                instance.PreventTrackingLevelAbuse(item);

                var actual = instance.New<Effort>(item);
                actual.Value = value;
                actual.Member = member;
                actual.Date = date;
                AddAttributes(actual, attributes);
                actual.Save();
                return actual;
            }

            /// <summary>
            /// Create a new effort record for the currently logged in member.
            /// </summary>
            /// <param name="value">The value of the effort record</param>
            /// <param name="item">The workitem to assign the effort record to</param>
            /// <returns>A newly minted Effort Record that exists in the VersionOne system.</returns>
            /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
            public Effort Effort(double value, Workitem item) {
                return Effort(value, item, null);
            }

            /// <summary>
            /// Create a new effort record for the currently logged in member.
            /// </summary>
            /// <param name="value">The value of the effort record</param>
            /// <param name="item">The workitem to assign the effort record to</param>
            /// <returns>A newly minted Effort Record that exists in the VersionOne system.</returns>
            /// <param name="attributes">Required attributes.</param>
            /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
            public Effort Effort(double value, Workitem item, IDictionary<string, object> attributes) {
                if(!instance.Configuration.EffortTrackingEnabled) {
                    throw new InvalidOperationException("Effort Tracking is disabled.");
                }

                instance.PreventTrackingLevelAbuse(item);

                var actual = instance.New<Effort>(item);
                actual.Value = value;
                AddAttributes(actual, attributes);
                actual.Save();
                return actual;
            }

            ///<summary>
            /// Create a new Build Project with a name and reference
            ///</summary>
            ///<param name="name">Initial name.</param>
            ///<param name="reference">Reference value.</param>
            ///<returns>A newly minted Build Project that exists in the VersionOne system.</returns>
            public BuildProject BuildProject(string name, string reference) {
                return BuildProject(name, reference, null);
            }

            ///<summary>
            /// Create a new Build Project with a name and reference
            ///</summary>
            ///<param name="name">Initial name.</param>
            ///<param name="reference">Reference value.</param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted Build Project that exists in the VersionOne system.</returns>
            public BuildProject BuildProject(string name, string reference, IDictionary<string, object> attributes) {
                var buildProject = new BuildProject(instance) {
                    Name = name, 
                    Reference = reference
                };
                AddAttributes(buildProject, attributes);
                buildProject.Save();
                return buildProject;
            }

            ///<summary>
            /// Create a new Build Run in the given Build Project with a name and date
            ///</summary>
            ///<param name="buildProject"></param>
            ///<param name="name"></param>
            ///<param name="date"></param>
            ///<returns>A newly minted Build Run that exists in the VersionOne system.</returns>
            public BuildRun BuildRun(BuildProject buildProject, string name, DateTime date) {
                return BuildRun(buildProject, name, date, null);
            }

            ///<summary>
            /// Create a new Build Run in the given Build Project with a name and date
            ///</summary>
            ///<param name="buildProject"></param>
            ///<param name="name"></param>
            ///<param name="date"></param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted Build Run that exists in the VersionOne system.</returns>
            public BuildRun BuildRun(BuildProject buildProject, string name, DateTime date,
                IDictionary<string, object> attributes) {
                var buildRun = new BuildRun(instance) {
                    Name = name, 
                    BuildProject = buildProject, 
                    Date = date
                };
                AddAttributes(buildRun, attributes);
                buildRun.Save();
                return buildRun;
            }

            ///<summary>
            /// Create a new ChangeSet with a name and reference
            ///</summary>
            ///<param name="name">Initial name.</param>
            ///<param name="reference">Reference value.</param>
            ///<returns>A newly minted ChangeSet that exists in the VersionOne system.</returns>
            public ChangeSet ChangeSet(string name, string reference) {
                return ChangeSet(name, reference, null);
            }

            ///<summary>
            /// Create a new ChangeSet with a name and reference
            ///</summary>
            ///<param name="name">Initial name.</param>
            ///<param name="reference">Reference value.</param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted ChangeSet that exists in the VersionOne system.</returns>
            public ChangeSet ChangeSet(string name, string reference, IDictionary<string, object> attributes) {
                var changeSet = new ChangeSet(instance) {
                    Name = name, 
                    Reference = reference
                };
                AddAttributes(changeSet, attributes);
                changeSet.Save();
                return changeSet;
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody) {
                ICollection<Member> recipients = new List<Member>();
                return Message(subject, messageBody, recipients);
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody, IDictionary<string, object> attributes) {
                ICollection<Member> recipients = new List<Member>();
                return Message(subject, messageBody, recipients, attributes);
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<param name="recipient">Who this message will go to.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody, Member recipient) {
                ICollection<Member> recipients = new List<Member> {recipient};
                return Message(subject, messageBody, recipients);
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<param name="recipient">Who this message will go to.</param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody, Member recipient,
                IDictionary<string, object> attributes) {
                ICollection<Member> recipients = new List<Member> {recipient};
                return Message(subject, messageBody, recipients, attributes);
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<param name="recipients">Who this message will go to. May be null.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody, ICollection<Member> recipients) {
                return Message(subject, messageBody, recipients, null);
            }

            ///<summary>
            /// Create a new Message with a subject and recipient.
            ///</summary>
            ///<param name="subject">Message subject.</param>
            ///<param name="messageBody">Message body.</param>
            ///<param name="recipients">Who this message will go to. May be null.</param>
            ///<param name="attributes">Required attributes.</param>
            ///<returns>A newly minted Message that exists in the VersionOne system.</returns>
            public Message Message(string subject, string messageBody, ICollection<Member> recipients,
                IDictionary<string, object> attributes) {
                var message = new Message(instance) {
                    Name = subject, 
                    Description = messageBody
                };

                if(recipients != null) {
                    foreach(var recipient in recipients) {
                        message.Recipients.Add(recipient);
                    }
                }
                AddAttributes(message, attributes);
                message.Save();
                return message;
            }

            ///<summary>
            /// Create a new Regression Plan with title and project
            ///</summary>
            ///<param name="name">Title of the plan</param>
            ///<param name="project">Project to assign</param>
            ///<param name="attributes">Additional attributes for initialization Regression Plan.</param>
            ///<returns>Regression Plan</returns>
            public RegressionPlan RegressionPlan(string name, Project project, IDictionary<string, object> attributes) {
                var regressionPlan = new RegressionPlan(instance) {
                    Name = name, 
                    Project = project
                };

                AddAttributes(regressionPlan, attributes);
                regressionPlan.Save();
                return regressionPlan;
            }

            ///<summary>
            /// Create a new Regression Plan with title and project
            ///</summary>
            ///<param name="name">Title of the plan</param>
            ///<param name="project">Project to assign</param>
            ///<returns>Regression Plan</returns>
            public RegressionPlan RegressionPlan(string name, Project project) {
                return RegressionPlan(name, project, null);
            }

            /// <summary>
            /// Create a new Regression Suite with title and Regression Plan
            /// </summary>
            /// <param name="name">Title of the suite</param>
            /// <param name="regressionPlan">Regression Plan to assign</param>
            /// <returns>Regression Suite</returns>
            public RegressionSuite RegressionSuite(string name, RegressionPlan regressionPlan) {
                return RegressionSuite(name, regressionPlan, null);
            }

            ///<summary>
            /// Create a new Regression Suite with title and Regression Plan
            ///</summary>
            ///<param name="name">Title of the suite</param>
            ///<param name="regressionPlan">Regression Plan to assign</param>
            ///<param name="attributes">Additional attributes for initialization of Regression Suite.</param>
            ///<returns>Regression Suite</returns>
            public RegressionSuite RegressionSuite(string name, RegressionPlan regressionPlan, IDictionary<string, object> attributes) {
                var regressionSuite = new RegressionSuite(instance) {
                    Name = name, 
                    RegressionPlan = regressionPlan
                };

                AddAttributes(regressionSuite, attributes);
                regressionSuite.Save();
                return regressionSuite;
            }

            /// <summary>
            /// Create new Test Set
            /// </summary>
            /// <param name="name">Test Set name</param>
            /// <param name="suite">Parent RegressionSuite</param>
            /// <param name="project">Parent Project</param>
            /// <param name="attributes">Additional attributes that should be set in order to be able to save when there is validation</param>
            /// <returns>Newly created Test Set</returns>
            public TestSet TestSet(string name, RegressionSuite suite, Project project, IDictionary<string, object> attributes) {
                // TODO invent consistent solution, possibly remove ability to create Test Sets from project
                if(!suite.RegressionPlan.Project.Equals(project)) {
                    throw new InvalidOperationException("Suite should belong to the project passed in parameters");
                }

                var testSet = new TestSet(instance) {
                    Name = name, 
                    RegressionSuite = suite, 
                    Project = project
                };

                AddAttributes(testSet, attributes);
                testSet.Save();
                return testSet;
            }

            /// <summary>
            /// Create new Test Set
            /// </summary>
            /// <param name="name">Test Set name</param>
            /// <param name="suite">Parent RegressionSuite</param>
            /// <param name="project">Parent Project</param>
            /// <returns>Newly created Test Set</returns>
            public TestSet TestSet(string name, RegressionSuite suite, Project project) {
                return TestSet(name, suite, project, null);
            }

            /// <summary>
            /// Create new Environment.
            /// </summary>
            /// <param name="name">Environment name</param>
            /// <param name="project">Parent Project</param>
            /// <returns>Newly created Environment</returns>
            public Environment Environment(string name, Project project) {
                return Environment(name, project, null);
            }

            /// <summary>
            /// Create new Environment.
            /// </summary>
            /// <param name="name">Environment name</param>
            /// <param name="project">Parent Project</param>
            /// <param name="attributes">Additional attributes that should be set for Environment</param>
            /// <returns>Newly created Environment</returns>
            public Environment Environment(string name, Project project, IDictionary<string, object> attributes) {
                var environment = new Environment(instance) {
                    Name = name, 
                    Project = project
                };

                AddAttributes(environment, attributes);
                environment.Save();
                return environment;
            }

            /// <summary>
            /// Create new Regression Test.
            /// </summary>
            /// <param name="name">Regression Test name</param>
            /// <param name="scope">Regression Test project</param>
            /// <returns>Newly created Regression Test</returns>
            public RegressionTest RegressionTest(string name, Project scope) {
                return RegressionTest(name, scope, null);
            }

            /// <summary>
            /// Create new Regression Test.
            /// </summary>
            /// <param name="name">Regression Test name</param>
            /// <param name="scope">Regression Test project</param>
            /// <param name="attributes">Additional attributes that should be set for Environment</param>
            /// <returns>Newly created Regression Test</returns>
            public RegressionTest RegressionTest(string name, Project scope, IDictionary<string, object> attributes) {
                var regressionTest = new RegressionTest(instance) {
                    Name = name, 
                    Project = scope
                };
                AddAttributes(regressionTest, attributes);

                regressionTest.Save();
                return regressionTest;
            }

            /// <summary>
            /// Create new Regression Test based on Test
            /// </summary>
            /// <param name="test">Test which we will be used to create Regression Test</param>
            /// <returns>Newly created Regression Test</returns>
            public RegressionTest RegressionTest(Test test) {
                var regressionTest = new RegressionTest(instance) {
                    Description = test.Description, 
                    GeneratedFrom = test, 
                    Name = test.Name, 
                    Project = test.Project
                };

                regressionTest.Type.CurrentValue = test.Type.CurrentValue;

                foreach(var member in test.Owners) {
                    regressionTest.Owners.Add(member);
                }

                regressionTest.Save();

                return regressionTest;
            }


            /// <summary>
            /// Add new Conversation with author as current logged user.
            /// </summary>
            /// <param name="content">Content of Conversation.</param>
            /// <returns>Newly created Conversation.</returns>
            public Conversation Conversation(string content) {
                return Conversation(instance.LoggedInMember, content, null);
            }

            /// <summary>
            /// Add new Conversation.
            /// </summary>
            /// <param name="author">Author of Conversation.</param>
            /// <param name="content">Content of Conversation.</param>
            /// <returns>Newly created Conversation.</returns>
            public Conversation Conversation(Member author, string content) {
                return Conversation(author, content, null);
            }

            /// <summary>
            /// Add new Conversation.
            /// </summary>
            /// <param name="author">Author of Conversation.</param>
            /// <param name="content">Content of Conversation.</param>
            /// <param name="attributes">Additional attributes that should be set for Conversation</param>
            /// <returns>Newly created Conversation.</returns>
            public Conversation Conversation(Member author, string content, IDictionary<string, object> attributes) {
                var conversation = new Conversation(instance) {
                    Author = author, 
                    AuthoredAt = DateTime.UtcNow, 
                    Content = content
                };
                AddAttributes(conversation, attributes);
                conversation.Save();

                return conversation;
            }

            /// <summary>
            /// Fills required attributes
            /// </summary>
            /// <param name="entity">entity for filling</param>
            /// <param name="attributes">Required attributes.</param>
            public void AddAttributes(Entity entity, IDictionary<string, object> attributes) {
                if (attributes == null) {
                    return;
                }
                foreach(var attributeName in attributes.Keys) {
                    entity.Set(attributeName, attributes[attributeName]);
                }
            }
        }
    }
}