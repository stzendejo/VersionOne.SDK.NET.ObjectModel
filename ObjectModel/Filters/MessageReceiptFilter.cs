using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
	internal class MessageReceiptFilter : EntityFilter
	{
		internal override System.Type EntityType
		{
			get { return typeof(MessageReceipt); }
		}

		/// <summary>
		/// Message receiver.
		/// </summary>
		public readonly ICollection<Member> Recipient = new List<Member>();

		/// <summary>
		/// Message itself.
		/// </summary>
		public readonly ICollection<Message> Message = new List<Message>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Relation("Message", Message);
			builder.Relation("Recipient", Recipient);
		}
	}
}