using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel
{
    /// <summary>
    /// Expression entity.
    /// </summary>
    [MetaData("Expression")]
    public class Expression : Entity
    {
        internal Expression(V1Instance instance) : base(instance) {}
        internal Expression(AssetID id, V1Instance instance) : base(id, instance) { }

        /// <summary>
        /// Message author.
        /// </summary>
        public Member Author
        {
            get { return GetRelation<Member>("Author"); }
            set { SetRelation("Author", value); }
        }

        /// <summary>
        /// When this message was added.
        /// </summary>
        public DateTime AuthoredAt
        {
            get { return Get<DateTime>("AuthoredAt"); }
            set { Set("AuthoredAt", value); }
        }

        /// <summary>
        /// Parent conversation.
        /// </summary>
        public Conversation BelongsTo
        {
            get { return GetRelation<Conversation>("BelongsTo"); }
            set { SetRelation("BelongsTo", value); }
        }

        /// <summary>
        /// The content of this message.
        /// </summary>
        public string Content
        {
            get { return Get<string>("Content"); }
            set { Set("Content", value); }
        }

        /// <summary>
        /// Parent message.
        /// </summary>
        public Expression InReplyTo
        {
            get { return GetRelation<Expression>("InReplyTo"); }
            set { SetRelation("InReplyTo", value); }
        }

        /// <summary>
        /// Replies posted to current item.
        /// </summary>
        public ICollection<Expression> Replies
        {
            get { return GetMultiRelation<Expression>("Replies"); }
        }

        /// <summary>
        /// Mentioned members.
        /// </summary>
        public ICollection<BaseAsset> Mentions
        {
            get { return GetMultiRelation<BaseAsset>("Mentions"); }
        }

        /// <summary>
        /// True if the message can be deleted.
        /// </summary>
        public virtual bool CanDelete
        {
            get { return Instance.CanExecuteOperation(this, "Delete"); }
        }

        /// <summary>
        /// Deletes the expression.
        /// NOTE: only owner can delete expression.
        /// </summary>
        /// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
        public void Delete()
        {
            Save();
            Instance.ExecuteOperation(this, "Delete");
        }
    }
}