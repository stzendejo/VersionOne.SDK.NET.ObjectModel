using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class ConversationTester : BaseSDKTester {
        private readonly IList<Conversation> disposableConversations = new List<Conversation>();

        [TestFixtureTearDown]
        public void TestFixtureTearDown() {
            foreach (Conversation item in disposableConversations) {
                item.Author = Instance.LoggedInMember;
                item.Save();
                
                if (item.CanDelete) {
                    item.Delete();
                }
            }
        }

        [Test]
        public void CreateConversation() {
            string converstaionContent = "testing conversation creation";
            Member author = EntityFactory.CreateMember("test");
            Conversation conversation = CreateDisposableConversation(author, converstaionContent);
            conversation.Save();
            ResetInstance();

            Conversation newConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(author, newConversation.Author);
            Assert.AreEqual(converstaionContent, newConversation.Content);
        }

        [Test]
        public void CreateConversationWithCurrentLoggedUser() {
            string converstaionContent = "testing conversation creation with logged memeber";
            Conversation conversation = Instance.Create.Conversation(converstaionContent);
            conversation.Save();
            disposableConversations.Add(conversation);
            ResetInstance();

            Conversation newConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(Instance.LoggedInMember, newConversation.Author);
            Assert.AreEqual(converstaionContent, newConversation.Content);
        }

        [Test]
        public void DeleteConversation() {
            Member currentUser = Instance.LoggedInMember;
            Conversation conversation = Instance.Create.Conversation(currentUser, "A disposable conversation");
            conversation.Save();

            AssetID id = conversation.ID;

            Conversation persistedConversation = Instance.Get.ConversationByID(id);
            Assert.IsNotNull(persistedConversation);

            conversation.Delete();

            persistedConversation = Instance.Get.ConversationByID(id);
            Assert.IsNull(persistedConversation);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteConversationAuthoredByAnotherUser() {
            Member author = EntityFactory.CreateMember("ConversationAuthor");
            Conversation conversation = CreateDisposableConversation(author, "A disposable conversation");

            AssetID id = conversation.ID;

            Conversation persistedConversation = Instance.Get.ConversationByID(id);
            Assert.IsNotNull(persistedConversation);

            conversation.Delete();
        }

        [Test]
        public void MentionsReference() {
            Conversation conversation = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation");
            conversation.Mentions.Add(Instance.LoggedInMember);
            conversation.Save();

            Conversation persistedConversation = Instance.Get.ConversationByID(conversation.ID);
            CollectionAssert.Contains(persistedConversation.Mentions, Instance.LoggedInMember);
            CollectionAssert.Contains(Instance.LoggedInMember.MentionedInExpressions, persistedConversation);
        }

        [Test]
        public void BaseAssetsReference() {
            Story story = EntityFactory.CreateStory("Conversations related", SandboxProject);
            Conversation conversation = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation");
            conversation.Mentions.Add(story);
            conversation.Save();

            Conversation persistedConversation = Instance.Get.ConversationByID(conversation.ID);
            Story persistedStory = Instance.Get.StoryByID(story.ID);
            CollectionAssert.Contains(persistedConversation.Mentions, persistedStory);
            CollectionAssert.Contains(persistedStory.MentionedInExpressions, persistedConversation);
        }

        [Test]
        public void AuthorReference() {
            Conversation conversation = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation");

            Conversation persistedConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(Instance.LoggedInMember, persistedConversation.Author);
            CollectionAssert.Contains(Instance.LoggedInMember.Expressions, persistedConversation);
        }

        [Test]
        public void ParentConversationReference() {
            Conversation parent = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation - parent");
            parent.Save();
            Conversation child = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation - child");
            child.ParentConversation = parent;
            child.Save();

            Conversation persistedParentConversation = Instance.Get.ConversationByID(parent.ID);
            Conversation persistedChildConversation = Instance.Get.ConversationByID(child.ID);
            Assert.AreEqual(persistedChildConversation.ParentConversation, persistedParentConversation);
            CollectionAssert.Contains(persistedParentConversation.ExpressionsInConversation, persistedChildConversation);
        }

        [Test]
        public void RepliesReference() {
            Conversation parent = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation - parent");
            Conversation child = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation - child");
            child.InReplyTo = parent;
            child.Save();

            Conversation persistedParentConversation = Instance.Get.ConversationByID(parent.ID);
            Conversation persistedChildConversation = Instance.Get.ConversationByID(child.ID);
            Assert.AreEqual(persistedChildConversation.InReplyTo, persistedParentConversation);
            CollectionAssert.Contains(persistedParentConversation.Replies, persistedChildConversation);
        }

        [Test]
        public void CreateConversationViaStory() {
            string conversationText = "Created through story";
            Story story = EntityFactory.CreateStory("Conversation Story", Instance.Get.ProjectByID(AssetID.FromToken("Scope:0")));
            Conversation conversation = story.CreateConversation(Instance.LoggedInMember, conversationText);
            disposableConversations.Add(conversation);
            ResetInstance();

            Conversation newConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(conversationText, newConversation.Content);
            Assert.AreEqual(1, newConversation.Mentions.Count);
            CollectionAssert.Contains(newConversation.Mentions, story);
            Story newStory = Instance.Get.StoryByID(story.ID);
            Assert.AreEqual(1, newStory.MentionedInExpressions.Count);
            CollectionAssert.Contains(newStory.MentionedInExpressions, newConversation);
        }

        [Test]
        public void CreateConversationViaMember() {
            string conversationText = "Created through member";
            Member member = Instance.LoggedInMember;
            Conversation conversation = member.CreateConversation(Instance.LoggedInMember, conversationText);
            disposableConversations.Add(conversation);
            ResetInstance();

            Conversation newConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(conversationText, newConversation.Content);
            Assert.AreEqual(1, newConversation.Mentions.Count);
            CollectionAssert.Contains(newConversation.Mentions, member);
            Member newMember = Instance.LoggedInMember;
            CollectionAssert.Contains(newMember.MentionedInExpressions, newConversation);
        }

        [Test]
        public void CreateConversationViaMemberWithCurrentAuthor() {
            string conversationText = "Created through member with this author";
            Member member = Instance.LoggedInMember;
            Conversation conversation = member.CreateConversation(conversationText);
            disposableConversations.Add(conversation);
            ResetInstance();

            Conversation newConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(conversationText, newConversation.Content);
            Assert.AreEqual(0, newConversation.Mentions.Count);
            Assert.AreEqual(member, newConversation.Author);
        }

        [Test]
        public void GetConversationById() {
            Conversation conversation = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation");
            conversation.Save();

            Conversation persistedConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(conversation, persistedConversation);
        }

        private Conversation CreateDisposableConversation(Member author, string content) {
            Conversation conversation = Instance.Create.Conversation(author, content);
            conversation.Save();
            disposableConversations.Add(conversation);

            return conversation;
        }
    }
}
