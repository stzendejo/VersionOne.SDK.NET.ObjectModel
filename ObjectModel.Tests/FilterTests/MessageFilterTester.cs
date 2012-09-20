using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class MessageFilterTester : BaseSDKTester
	{
		string _messageBody = "The message body.";
		private MessageFilter _myMessages = new MessageFilter();
		
		private Member _me;
		private Member Me
		{
			get
			{
				if (_me == null)
					_me = Instance.LoggedInMember;

				return _me;
			}
		}


		[TestFixtureSetUp]
		public void Setup()
		{
			_me = null;
			_myMessages.Recipient.Add(Me);

			// Cleanup any old messages for me
			foreach (Message myMessage in Instance.Get.Messages(_myMessages))
				myMessage.DeleteReceipt();

			Assert.AreEqual(0, Instance.Get.Messages(_myMessages).Count, "I need 0 messages to start for these test to run properly.");

			Instance.Create.Message("Basic Message", _messageBody, Me).Send();
			Instance.Create.Message("Read Message", _messageBody, Me).Send().MarkAsRead();
			Instance.Create.Message("Archived Message", _messageBody, Me).Send().MarkAsArchived();
			Message message = Instance.Create.Message("Archived Read Message", _messageBody, Me).Send();
			message.MarkAsRead();
			message.MarkAsArchived();

			ResetInstance();
		}

		[Test]
		public void AllMessages()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			ICollection<Message> messages = Instance.Get.Messages(filter);
			Assert.AreEqual(4, messages.Count);
		}

		[Test]
		public void UnReadMessagesOnly()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			filter.IsUnread = true;
			ICollection<Message> unreadMessages = Instance.Get.Messages(filter);
			Assert.AreEqual(2, unreadMessages.Count);
		}

		[Test]
		public void ReadMessagesOnly()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			filter.IsUnread = false;
			ICollection<Message> readMessages = Instance.Get.Messages(filter);
			Assert.AreEqual(2, readMessages.Count);
		}

		[Test]
		public void UnArchivedMessagesOnly()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			filter.IsArchived = false;
			ICollection<Message> unarchivedMessages = Instance.Get.Messages(filter);
			Assert.AreEqual(2, unarchivedMessages.Count);
		}

		[Test]
		public void ArchivedMessagesOnly()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			filter.IsArchived = true;
			ICollection<Message> archivedMessages = Instance.Get.Messages(filter);
			Assert.AreEqual(2, archivedMessages.Count);
		}

		[Test]
		public void ArchivedReadMessagesOnly()
		{
			MessageFilter filter = new MessageFilter();
			filter.Recipient.Add(Me);
			filter.IsArchived = true;
			filter.IsUnread = false;
			ICollection<Message> archivedMessages = Instance.Get.Messages(filter);
			Assert.AreEqual(1, archivedMessages.Count);
		}
	}
}