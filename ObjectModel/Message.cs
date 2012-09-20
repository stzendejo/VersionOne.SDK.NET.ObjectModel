using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a notification Message in the VersionOne System
    /// </summary>
    [MetaData("Message")]
    public class Message : BaseAsset {
        private MessageReceipt receipt;

        internal Message(AssetID id, V1Instance instance) : base(id, instance) {
        }

        internal Message(V1Instance instance) : base(instance) {
        }

        /// <summary>
        /// Members this message is addressed to. Message may not be sent until there is at least one recipient.
        /// </summary>
        public ICollection<Member> Recipients {
            get { return GetMultiRelation<Member>("Recipients"); }
        }

        /// <summary>
        /// An asset associated with this message.
        /// </summary>
        [MetaRenamed("Asset")]
        public BaseAsset RelatedAsset {
            get { return GetRelation<BaseAsset>("Asset"); }
            set { SetRelation("Asset", value); }
        }

        /// <summary>
        /// True if the item is in a state in which it may be sent.
        /// </summary>
        [MetaRenamed("CheckSend")]
        public bool ReadyToSend {
            get { return Get<bool>("CheckSend"); }
        }

        /// <summary>
        /// True if the item has not been read
        /// </summary>
        public bool IsUnread {
            get { return Receipt.IsUnread; }
        }

        /// <summary>
        /// True if the item has been read
        /// </summary>
        public bool IsRead {
            get { return Receipt.IsRead; }
        }

        /// <summary>
        /// True if the item has not been archived
        /// </summary>
        public bool IsUnarchived {
            get { return Receipt.IsUnarchived; }
        }

        /// <summary>
        /// True if the item has been archived
        /// </summary>
        public bool IsArchived {
            get { return Receipt.IsArchived; }
        }

        /// <summary>
        /// Send this message. Must have at least one recipeint. Once sent it may not be modified. Message will always be saved as part of the send.
        /// </summary>
        public Message Send() {
            if (Recipients.Count == 0) {
                throw new MessageException("Must have at least one message recipient to send to.");
            }

            Save();
            Instance.ExecuteOperation<Message>(this, "Send");

            return this;
        }        

        private MessageReceipt Receipt {
            get { return receipt ?? (receipt = GetMessageReceipt()); }
        }

        private MessageReceipt GetMessageReceipt() {
            var filter = new MessageReceiptFilter();
            filter.Recipient.Add(Instance.LoggedInMember);
            filter.Message.Add(this);
            var receipts = Instance.Get.MessageReceipts(filter);
            if (receipts.Count == 1) {
                var enumerator = receipts.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current;
            }

            throw new NotRecipientofMessageException();
        }

        /// <summary>
        /// Mark the message as having been read by the recipient.
        /// </summary>
        public void MarkAsRead() {
            Receipt.MarkAsRead();
        }

        /// <summary>
        /// Mark the message as having not been read by the recipient.
        /// </summary>
        public void MarkAsUnread() {
            Receipt.MarkAsUnread();
        }

        /// <summary>
        /// Mark the message as having been archived.
        /// </summary>
        public void MarkAsArchived() {
            Receipt.MarkAsArchived();
        }

        /// <summary>
        /// Mark the message as not having been archived.
        /// </summary>
        public void MarkAsUnarchived() {
            Receipt.MarkAsUnarchived();
        }

        internal override void CloseImpl() {
            throw new InvalidOperationException();
        }

        internal override void ReactivateImpl() {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Deletes any message receipt associated with this message for the logged in user.
        /// </summary>
        public bool DeleteReceipt() {
            try {
                Receipt.Delete();
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }

    ///<summary>
    /// An exception with a message.
    ///</summary>
    public class MessageException : SDKException {
        ///<summary>
        /// Basic constructor.
        ///</summary>
        public MessageException(string message) : base(message) {
        }
    }

    /// <summary>
    /// The operation or attribute is only avalable when the logged in user is a recipient of the message.
    /// </summary>
    public class NotRecipientofMessageException : MessageException {
        ///<summary>
        /// Basic constructor.
        ///</summary>
        public NotRecipientofMessageException() : base("Must be a recipient of the message to perform operation.") {
        }
    }
}