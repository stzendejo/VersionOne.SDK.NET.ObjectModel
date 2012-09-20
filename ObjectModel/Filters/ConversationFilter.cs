using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for getting Expression/Conversation.
    /// </summary>
    public class ConversationFilter : EntityFilter {
        internal override Type EntityType {
            get { return typeof (Conversation); }
        }

        /// <summary>
        /// Filter on the Author property.
        /// </summary>
        public readonly ICollection<Member> Author = new List<Member>();

        /// <summary>
        /// Filter on the AuthoredAt property.
        /// </summary>
        public readonly DateSearcher AuthoredAt = new DateSearcher();

        /// <summary>
        /// Filter on the Conversation property.
        /// </summary>
        public readonly ICollection<Conversation> Conversation = new List<Conversation>();

        /// <summary>
        /// Filter on the ExpressionsInConversation property.
        /// </summary>
        public readonly ICollection<Conversation> ExpressionsInConversation = new List<Conversation>();

        /// <summary>
        /// Filter on the InReplyTo property.
        /// </summary>
        public readonly ICollection<Conversation> InReplyTo = new List<Conversation>();

        /// <summary>
        /// Filter on the Replies property.
        /// </summary>
        public readonly ICollection<Conversation> Replies = new List<Conversation>();

        /// <summary>
        /// Filter on the Mentions property.
        /// </summary>
        public readonly ICollection<BaseAsset> Mentions = new List<BaseAsset>();

        /// <summary>
        /// Current State of the asset.
        /// </summary>
        public ICollection<State> State {
            get { return state; }
        }

        private readonly IList<State> state = new List<State>();

        internal virtual bool HasState {
            get { return State.Count == 1; }
        }

        internal virtual bool HasActive {
            get { return State.Contains(Filters.State.Active); }
        }

        internal virtual bool HasClosed {
            get { return State.Contains(Filters.State.Closed); }
        }

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Relation("Author", Author);
            builder.Comparison("AuthoredAt", AuthoredAt);
            builder.Relation("Conversation", Conversation);
            builder.MultiRelation("ExpressionsInConversation", ExpressionsInConversation);
            builder.Relation("InReplyTo", InReplyTo);
            builder.MultiRelation("Replies", Replies);
            builder.MultiRelation("Mentions", Mentions);
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            if (HasState) {
                if(HasActive) {
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='Expression'"));
                } else {
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='Expression'"));
                }
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Expression'"));
            }
        }
    }
}