namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents an associate between a Message and a recipient in the VersionOne System
	/// </summary>
	[MetaData("MessageReceipt")]
	internal class MessageReceipt : Entity
	{
		internal MessageReceipt(AssetID id, V1Instance instance) : base(id, instance) { }
		internal MessageReceipt(V1Instance instance) : base(instance) { }

		/// <summary>
		/// Member this message is addressed to.
		/// </summary>
		public Member Recipient { get { return GetRelation<Member>("Recipient"); } set { SetRelation("Recipient", value); } }

		/// <summary>
		/// Member this message is addressed to.
		/// </summary>
		public Message Message { get { return GetRelation<Message>("Message"); } set { SetRelation("Message", value); } }

		/// <summary>
		/// True if the item has not been read
		/// </summary>
		public bool IsUnread { get { return Get<bool>("IsUnread"); } }

		/// <summary>
		/// True if the item has been read
		/// </summary>
		public bool IsRead { get { return Get<bool>("IsRead"); } }

		/// <summary>
		/// True if the item has not been archived
		/// </summary>
		public bool IsUnarchived { get { return Get<bool>("IsUnarchived"); } }

		/// <summary>
		/// True if the item has been archived
		/// </summary>
		public bool IsArchived { get { return Get<bool>("IsArchived"); } }

		/// <summary>
		/// Mark the message as having been read by the recipient.
		/// </summary>
		public void MarkAsRead()
		{
			Instance.ExecuteOperation<MessageReceipt>(this, "MarkAsRead");
		}

		/// <summary>
		/// Mark the message as having not been read by the recipient.
		/// </summary>
		public void MarkAsUnread()
		{
			Instance.ExecuteOperation<MessageReceipt>(this, "MarkAsUnread");
		}

		/// <summary>
		/// Mark the message as having been archived.
		/// </summary>
		public void MarkAsArchived()
		{
			Instance.ExecuteOperation<MessageReceipt>(this, "MarkAsArchived");
		}

		/// <summary>
		/// Mark the message as not having been archived.
		/// </summary>
		public void MarkAsUnarchived()
		{
			Instance.ExecuteOperation<MessageReceipt>(this, "MarkAsUnarchived");
		}

		/// <summary>
		/// Mark the message as having been archived.
		/// </summary>
		public void Delete()
		{
			Instance.ExecuteOperation<MessageReceipt>(this, "Delete");
		}
	}
}