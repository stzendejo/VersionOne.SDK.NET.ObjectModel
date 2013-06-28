using System;
using System.Collections.Generic;
using System.IO;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents the base type of members, projects, teams, etc in the VersionOne system
    /// </summary>
    [MetaData("BaseAsset", "Name")]
    public abstract class BaseAsset : Entity {
        private ICustomAttributeDictionary customField;

        internal BaseAsset(V1Instance instance) : base(instance) {}
        internal BaseAsset(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// URL to VersionOne Detail Page for this BaseAsset
        /// </summary>
        public virtual string URL {
            get { return GetEntityURL(); }
        }

        /// <summary>
        /// Name of the base asset
        /// </summary>
        public string Name {
            get { return Get<string>("Name"); }
            set { Set("Name", value); }
        }

        /// <summary>
        /// Description of the base asset
        /// </summary>
        public string Description {
            get { return Get<string>("Description"); }
            set { Set("Description", value); }
        }

        /// <summary>
        /// Expressions that reference current item.
        /// </summary>
        public ICollection<Conversation> MentionedInExpressions {
            get { return GetMultiRelation<Conversation>("MentionedInExpressions"); }
        }

        /// <summary>
        /// A collection attachments that belong to this base asset filtered by the passed in filter
        /// </summary>
        public ICollection<Attachment> GetAttachments(AttachmentFilter filter) {
            filter = filter ?? new AttachmentFilter();
            filter.Asset.Clear();
            filter.Asset.Add(this);
            return Instance.Get.Attachments(filter);
        }

        /// <summary>
        /// A collection notes that belong to this base asset filtered by the passed in filter
        /// </summary>
        public ICollection<Note> GetNotes(NoteFilter filter) {
            filter = filter ?? new NoteFilter();
            filter.Asset.Clear();
            filter.Asset.Add(this);
            return Instance.Get.Notes(filter);
        }

        /// <summary>
        /// A collection links that belong to this base asset filtered by the passed in filter
        /// </summary>
        public ICollection<Link> GetLinks(LinkFilter filter) {
            filter = filter ?? new LinkFilter();
            filter.Asset.Clear();
            filter.Asset.Add(this);
            return Instance.Get.Links(filter);
        }


        /// <summary>
        /// Indicates if the entity is open or active.
        /// </summary>
        public bool IsActive {
            get { return IsActiveImpl; }
        }

        /// <summary>
        /// Indicates if the entity is closed or inactive.
        /// </summary>
        public bool IsClosed {
            get { return IsClosedImpl; }
        }

        /// <summary>
        /// Internal worker function to determine active state.
        /// </summary>
        internal virtual bool IsActiveImpl {
            get { return Get<byte>("AssetState", true) == 64; }
        }

        /// <summary>
        /// Internal worker function to determine closed state.
        /// </summary>
        internal virtual bool IsClosedImpl {
            get { return Get<byte>("AssetState", true) == 128; }
        }

        /// <summary>
        /// True if the item can be deleted
        /// </summary>
        public bool CanDelete {
            get { return CanDeleteImpl; }
        }

        internal virtual bool CanDeleteImpl {
            get { return Instance.CanExecuteOperation(this, "Delete"); }
        }

        /// <summary>
        /// Deletes the Item
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
        public void Delete() {
            Save();
            Instance.ExecuteOperation(this, "Delete");
        }        

        /// <summary>
        /// Simple Custom Fields
        /// </summary>
        public ICustomAttributeDictionary CustomField {
            get { return customField ?? (customField = new SimpleCustomAttributeDictionary(this)); }
        }

        private ICustomDropdownDictionary _customDropdown;

        /// <summary>
        /// Custom List-Type Fields
        /// </summary>
        public ICustomDropdownDictionary CustomDropdown {
            get { return _customDropdown ?? (_customDropdown = new ListCustomAttributeDictionary(this)); }
        }

        /// <summary>
        /// Create a link that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the link</param>
        /// <param name="url">The url of the link</param>
        /// <param name="onMenu">True if the link is visible on this asset's detail page menu</param>
        /// <returns></returns>
        public Link CreateLink(string name, string url, bool onMenu) {
            return Instance.Create.Link(name, this, url, onMenu);
        }

        /// <summary>
        /// Create a link that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the link</param>
        /// <param name="url">The url of the link</param>
        /// <param name="onMenu">True if the link is visible on this asset's detail page menu</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Link CreateLink(string name, string url, bool onMenu, IDictionary<string, object> attributes) {
            return Instance.Create.Link(name, this, url, onMenu, attributes);
        }

        /// <summary>
        /// Create a n0te that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the n0te</param>
        /// <param name="content">The content of the n0te</param>
        /// <param name="personal">True if the n0te is only visible to the currently logged in user</param>
        /// <returns></returns>
        public Note CreateNote(string name, string content, bool personal) {
            return Instance.Create.Note(name, this, content, personal);
        }

        /// <summary>
        /// Create a n0te that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the n0te</param>
        /// <param name="content">The content of the n0te</param>
        /// <param name="personal">True if the n0te is only visible to the currently logged in user</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Note CreateNote(string name, string content, bool personal, IDictionary<string, object> attributes) {
            return Instance.Create.Note(name, this, content, personal, attributes);
        }

        /// <summary>
        /// Create an attachment that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the attachment</param>
        /// <param name="filename">The name of the original attachment file</param>
        /// <param name="stream">The read-enabled stream that contains the attachment content to upload</param>
        /// <returns></returns>
        public Attachment CreateAttachment(string name, string filename, Stream stream) {
            return Instance.Create.Attachment(name, this, filename, stream);
        }

        /// <summary>
        /// Create an attachment that belongs to this asset
        /// </summary>
        /// <param name="name">The name of the attachment</param>
        /// <param name="filename">The name of the original attachment file</param>
        /// <param name="stream">The read-enabled stream that contains the attachment content to upload</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Attachment CreateAttachment(string name, string filename, Stream stream,
                                           IDictionary<string, object> attributes) {
            return Instance.Create.Attachment(name, this, filename, stream, attributes);
        }


        /// <summary>
        /// Creates conversation which mentioned this asset.
        /// </summary>
        /// <param name="author">Author of conversation.</param>
        /// <param name="content">Content of conversation</param>
        /// <returns>Created conversation.</returns>
        public virtual Conversation CreateConversation(Member author, string content) {
            var conversation = Instance.Create.Conversation(author, content);
            foreach (Expression containedExpression in conversation.ContainedExpressions)
            {
                containedExpression.Mentions.Add(this);
            }
            conversation.Save();
            return conversation;
        }

        /// <summary>
        /// True if the item can be closed.
        /// </summary>
        public bool CanClose {
            get { return CanCloseImpl; }
        }

        internal virtual bool CanCloseImpl {
            get { return Instance.CanExecuteOperation(this, "Inactivate"); }
        }

        /// <summary>
        /// Closes or Inactivates the item
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already closed or inactive.</exception>
        public void Close() {
            Save();
            CloseImpl();
            ClearCache("AssetState");
        }

        internal abstract void CloseImpl();

        /// <summary>
        /// True if item can be Reactivated.
        /// </summary>
        public bool CanReactivate {
            get { return CanReactivateImpl; }
        }

        internal virtual bool CanReactivateImpl {
            get { return Instance.CanExecuteOperation(this, "Reactivate"); }
        }

        /// <summary>
        /// Reactivates the item
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation, e.g. it is already open or active.</exception>
        public void Reactivate() {
            ReactivateImpl();
            Save();
            ClearCache("AssetState");
        }

        internal abstract void ReactivateImpl();
    }
}