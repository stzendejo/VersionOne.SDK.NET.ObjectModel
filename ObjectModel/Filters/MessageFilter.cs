using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters
{
	///<summary>
	/// A filter for Messages
	///</summary>
	public class MessageFilter : BaseAssetFilter
	{
		internal override System.Type EntityType
		{
			get { return typeof(Message); }
		}

		/// <summary>
		/// Filter on RelatedAsset.
		/// </summary>
		public ICollection<BaseAsset> RelatedAsset = new List<BaseAsset>();

		/// <summary>
		/// Filter on read or unread messages.
		/// </summary>
		public bool? IsUnread;

		/// <summary>
		/// Filter on archived or unarchived messages.
		/// </summary>
		public bool? IsArchived;

		/// <summary>
		/// Only messages sent to a particular recipient.
		/// </summary>
		public ICollection<Member> Recipient = new List<Member>();

		/// <summary>
		/// Only messages sent by a particular recipient.
		/// </summary>
		public ICollection<Member> Sender = new List<Member>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("Asset", RelatedAsset);
			builder.Relation("CreatedBy", Sender);

			foreach (Member member in Recipient)
			{
				builder.Root.And(new TokenTerm(string.Format("Message.Receipts[Recipient='{0}';IsDeleted='false']",member.ID.Token)));
			}

			// Because we are masking the Message-Receipt relation from the user, 
			// and we will only ever get messages where I am the recipient or creator, 
			// We filter Read and Unread by the receipts, assuming only one.
			if (IsUnread.HasValue)
				builder.Simple("Receipts.IsUnread", IsUnread.Value);

			if (IsArchived.HasValue)
				builder.Simple("Receipts.IsArchived", IsArchived.Value);
		}
	}
}