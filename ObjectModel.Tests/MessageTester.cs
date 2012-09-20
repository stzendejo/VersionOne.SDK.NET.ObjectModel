using System.Collections.Generic;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class MessageTester : BaseSDKTester {
        private string messageName = "Test Message from SDK";
        private string messageBody = "The message body.";

        [Test]
        public void ArchiveMessage() {
            var message = Instance.Create.Message("Archived Message", messageBody, Instance.LoggedInMember);
            message.Send();

            message.MarkAsArchived();

            Assert.IsFalse(message.IsUnarchived);
            Assert.IsTrue(message.IsArchived);
            Assert.IsTrue(message.IsUnread, "Marking message archived should not mark the message as read.");
        }

        [Test]
        public void CannotDeleteOthersMessageReceipt() {
            var message = Instance.Create.Message("Deleted Message", messageBody, SandboxMember);
            message.Send();

            Assert.IsFalse(message.DeleteReceipt());
        }

        [Test]
        public void Create() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);

            Assert.AreEqual(messageName, message.Name);
            Assert.AreEqual(1, message.Recipients.Count);
            var recipient = message.Recipients.GetEnumerator();
            recipient.MoveNext();
            Assert.AreEqual(Instance.LoggedInMember, recipient.Current);
        }

        [Test]
        public void CreateMessageWithRequiredAttributes() {
            const string description = "Test for Message creation with required attributes";
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Description", description);

            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember, attributes);

            Assert.AreEqual(messageName, message.Name);
            Assert.AreEqual(description, message.Description);
        }

        [Test]
        public void CreateWithRelatedAsset() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);

            var story = Instance.Create.Story("Story with Message", SandboxProject);

            message.RelatedAsset = story;
            message.Save();

            Assert.AreEqual(story, message.RelatedAsset);
        }

        [Test]
        public void DeleteMyOwnMessageReceipt() {
            var message = Instance.Create.Message("Deleted Message", messageBody, Instance.LoggedInMember);
            message.Send();

            Assert.IsTrue(message.DeleteReceipt());
        }

        [Test]
        [ExpectedException(typeof(DataException))]
        public void EditSentMessage() {
            var message = Instance.Create.Message(messageName, messageBody, SandboxMember);
            message.Send();
            message.Description = "New description";
            message.Save();
        }

        [Test]
        public void MarkMessageRead() {
            var message = Instance.Create.Message("Read Message", messageBody, Instance.LoggedInMember);
            message.Send();

            message.MarkAsRead();

            Assert.IsFalse(message.IsUnread);
            Assert.IsTrue(message.IsRead);
            Assert.IsTrue(message.IsUnarchived, "Marking message read should not archive the message.");
        }

        [Test]
        public void MarkMessageUnread() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);
            message.Send();

            message.MarkAsRead();
            message.MarkAsUnread();

            Assert.IsTrue(message.IsUnread);
            Assert.IsFalse(message.IsRead);
        }

        [Test]
        [ExpectedException(typeof(NotRecipientofMessageException))]
        public void MarkSomeoneElsesMessageAsRead() {
            var message = Instance.Create.Message(messageName, messageBody, SandboxMember);
            message.Send();
            message.MarkAsRead();
        }

        [Test]
        public void NewMessageIsUnarchived() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);
            message.Send();

            Assert.IsTrue(message.IsUnarchived);
            Assert.IsFalse(message.IsArchived);
        }

        [Test]
        public void NewMessageIsUnread() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);
            message.Send();

            Assert.IsTrue(message.IsUnread);
            Assert.IsFalse(message.IsRead);
        }

        [Test]
        public void NotReadyToSend() {
            var message = Instance.Create.Message(messageName, messageBody);
            Assert.AreEqual(false, message.ReadyToSend);
        }

        [Test]
        public void ReadyToSend() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);
            Assert.AreEqual(true, message.ReadyToSend);
        }

        [Test]
        [ExpectedException(typeof(MessageException))]
        public void SendMessageWithNoRecipients() {
            var message = Instance.Create.Message(messageName, messageBody, (IList<Member>)null);
            message.Send();
        }

        [Test]
        public void UnarchiveMessage() {
            var message = Instance.Create.Message(messageName, messageBody, Instance.LoggedInMember);
            message.Send();

            message.MarkAsArchived();
            message.MarkAsUnarchived();

            Assert.IsTrue(message.IsUnarchived);
            Assert.IsFalse(message.IsArchived);
        }
    }
}