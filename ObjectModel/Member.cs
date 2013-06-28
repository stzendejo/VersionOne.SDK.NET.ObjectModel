using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a user or member in the VersionOne system
    /// </summary>
    [MetaData("Member")]
    public class Member : BaseAsset {
        internal Member(AssetID id, V1Instance instance) : base(id, instance) {}
        internal Member(V1Instance instance) : base(instance) {}

        /// <summary>
        /// The short or abbreviated name of the user or member.  This name is often used in the owner's dropdown.
        /// </summary>
        public string ShortName {
            get { return Get<string>("Nickname"); }
            set { Set("Nickname", value); }
        }

        /// <summary>
        /// The username this user or member uses to login to the VersionOne system
        /// </summary>
        public string Username {
            get { return Get<string>("Username"); }
            set { Set("Username", value); }
        }

        /// <summary>
        /// The password this user or member uses to login to the VersionOne system.
        /// This is a write-only property.
        /// </summary>
        public string Password {
            set { Set("Password", value); }
        }

        /// <summary>
        /// The default role of the user or member. Determines the member's permissions when assigned to a project.  Also determines the member's global privileges.
        /// </summary>
        public Role DefaultRole {
            get { return GetRelation<Role>("DefaultRole"); }
            set { SetRelation("DefaultRole", value); }
        }

        /// <summary>
        /// The Email for this member.
        /// </summary>
        public string Email {
            get { return Get<string>("Email"); }
            set { Set("Email", value); }
        }

        /// <summary>
        /// The phone number for this member.
        /// </summary>
        public string Phone {
            get { return Get<string>("Phone"); }
            set { Set("Phone", value); }
        }

        /// <summary>
        /// A flag indicating whether this member desires to receive e-mail notifications.
        /// </summary>
        public bool NotifyViaEmail {
            get { return Get<bool>("NotifyViaEmail"); }
            set { Set("NotifyViaEmail", value); }
        }

        /// <summary>
        /// Projects this member is assigned to
        /// </summary>
        public ICollection<Project> AssignedProjects {
            get { return GetMultiRelation<Project>("Scopes"); }
        }

        /// <summary>
        /// Conversation messages created by the member.
        /// </summary>
        public ICollection<Conversation> Expressions {
            get { return GetMultiRelation<Conversation>("Expressions"); }
        }

        /// <summary>
        /// Turn conversation notifications on or off.
        /// </summary>
        public bool SendConversationEmails {
            get { return Get<bool>("SendConversationEmails"); }
            set { Set("SendConversationEmails", value); }
        }

        /// <summary>
        /// Stories and Defects owned by this member.
        /// </summary>
        /// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
        public ICollection<PrimaryWorkitem> GetOwnedPrimaryWorkitems(PrimaryWorkitemFilter filter) {
            filter = filter ?? new PrimaryWorkitemFilter();
            filter.Owners.Clear();
            filter.Owners.Add(this);
            return Instance.Get.PrimaryWorkitems(filter);
        }

        /// <summary>
        /// Tasks and Tests owned by this member.
        /// </summary>
        public ICollection<SecondaryWorkitem> GetOwnedSecondaryWorkitems(SecondaryWorkitemFilter filter) {
            filter = filter ?? new SecondaryWorkitemFilter();
            filter.Owners.Clear();
            filter.Owners.Add(this);
            return Instance.Get.SecondaryWorkitems(filter);
        }

        /// <summary>
        /// A collection of Themes owned by this member.
        /// </summary>
        public ICollection<Theme> GetOwnedThemes(ThemeFilter filter) {
            filter = filter ?? new ThemeFilter();
            filter.Owners.Clear();
            filter.Owners.Add(this);
            return Instance.Get.Themes(filter);
        }

        /// <summary>
        /// A read-only collection of Issues owned by this member.
        /// </summary>
        public ICollection<Issue> GetOwnedIssues(IssueFilter filter) {
            filter = filter ?? new IssueFilter();
            filter.Owner.Clear();
            filter.Owner.Add(this);
            return Instance.Get.Issues(filter);
        }

        /// <summary>
        /// A read-only collection of Requests owned by this member.
        /// </summary>
        public ICollection<Request> GetOwnedRequests(RequestFilter filter) {
            filter = filter ?? new RequestFilter();
            filter.Owner.Clear();
            filter.Owner.Add(this);
            return Instance.Get.Requests(filter);
        }

        /// <summary>
        /// Epics owned by this member.
        /// </summary>
        public ICollection<Epic> GetOwnedEpics(EpicFilter filter) {
            filter = filter ?? new EpicFilter();
            filter.Owners.Clear();
            filter.Owners.Add(this);
            return Instance.Get.Epics(filter);
        }

        /// <summary>
        /// A collection of Effort Records that belong to this member
        /// </summary>
        public ICollection<Effort> GetEffortRecords(EffortFilter filter) {
            filter = filter ?? new EffortFilter();
            filter.Member.Clear();
            filter.Member.Add(this);
            return Instance.Get.EffortRecords(filter);
        }

        /// <summary>
        /// Inactivates the Member
        /// </summary>
        /// <exception cref="InvalidOperationException">The Member is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl() {
            Instance.ExecuteOperation<Member>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Member
        /// </summary>
        /// <exception cref="InvalidOperationException">The Member is an invalid state for the Operation, e.g. it is already active.</exception>
        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Member>(this, "Reactivate");
        }

        /// <summary>
        /// Return the total estimate for all stories and defects owned by this member optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter stories and defects on. Pass a DefectFilter or StoryFilter to get only Defects or Stories, respectively.</param>
        /// <returns></returns>
        public double? GetTotalEstimate(PrimaryWorkitemFilter filter) {
            return GetSum("OwnedWorkitems:PrimaryWorkitem", filter ?? new PrimaryWorkitemFilter(), "Estimate");
        }

        /// <summary>
        /// Return the total detail estimate for all workitems owned by this member optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalDetailEstimate(WorkitemFilter filter) {
            return GetSum("OwnedWorkitems", filter ?? new WorkitemFilter(), "DetailEstimate");
        }

        /// <summary>
        /// Return the total to do for all workitems owned by this member optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalToDo(WorkitemFilter filter) {
            return GetSum("OwnedWorkitems", filter ?? new WorkitemFilter(), "ToDo");
        }

        /// <summary>
        /// Return the total detail estimate for all workitems owned by this member optionally filtered
        /// </summary>
        /// <param name="filter">Criteria to filter workitems on.</param>
        /// <returns></returns>
        public double? GetTotalDone(WorkitemFilter filter) {
            return GetSum("OwnedWorkitems", filter ?? new WorkitemFilter(), "Actuals.Value");
        }

        /// <summary>
        /// Creates conversation which mentioned this member.
        /// </summary>
        /// <param name="author">Author of conversation.</param>
        /// <param name="content">Content of conversation</param>
        /// <returns>Created conversation.</returns>
        public override Conversation CreateConversation(Member author, string content) {
            var conversation = Instance.Create.Conversation(author, content);
            foreach (Expression containedExpression in conversation.ContainedExpressions)
            {
                containedExpression.Mentions.Add(this);
            }
            conversation.Save();
            return conversation;
        }

        /// <summary>
        /// Creates conversation with this author.
        /// </summary>
        /// <param name="content">Content of conversation.</param>
        /// <returns>Created conversation.</returns>
        public Conversation CreateConversation(string content) {
            return Instance.Create.Conversation(this, content);
        }
    }
}