using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class ExpressionTester : BaseSDKTester {
        private readonly IList<Expression> disposableExpressions = new List<Expression>();
        private readonly IList<Conversation> disposableConversations = new List<Conversation>();

        public void TestFixtureTearDown() {
            foreach (Expression item in disposableExpressions)
            {
                item.Author = Instance.LoggedInMember;
                item.Save();
                
                if (item.CanDelete) {
                    item.Delete();
                }
            }

            foreach (Conversation item in disposableConversations)
            {
                item.Save();

                if (item.CanDelete)
                {
                    item.Delete();
                }
            }
        }

        [Test]
        public void CreateExpression()
        {
            string expressionContent = "testing expression creation";
            Member author = EntityFactory.CreateMember("test");
            Expression expression = CreateDisposableExpression(author, expressionContent);
            expression.Save();
            ResetInstance();

            Expression newExpression = Instance.Get.ExpressionByID(expression.ID);
            Assert.AreEqual(author, newExpression.Author);
            Assert.AreEqual(expressionContent, newExpression.Content);
        }

        [Test]
        public void CreateExpressionWithCurrentLoggedUser() {
            string expressionContent = "testing expression creation with logged member";
            Expression expression = CreateDisposableExpression(expressionContent);
            expression.Save();
            ResetInstance();

            Expression newExpression = Instance.Get.ExpressionByID(expression.ID);
            Assert.AreEqual(Instance.LoggedInMember, newExpression.Author);
            Assert.AreEqual(expression.Content, newExpression.Content);
        }

        [Test]
        public void DeleteExpression() {
            Expression expression = CreateDisposableExpression("A disposable conversation");
            expression.Save();

            AssetID id = expression.ID;

            Expression newExpression = Instance.Get.ExpressionByID(id);
            Assert.IsNotNull(newExpression);

            newExpression.Delete();

            newExpression = Instance.Get.ExpressionByID(id);
            Assert.IsNull(newExpression);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteExpressionAuthoredByAnotherUser() {
            Member author = EntityFactory.CreateMember("ConversationAuthor");
            Expression expression = CreateDisposableExpression(author, "A disposable expression");

            AssetID id = expression.ID;

            Expression persistedExpression = Instance.Get.ExpressionByID(id);
            Assert.IsNotNull(persistedExpression);

            persistedExpression.Delete();
        }

        [Test]
        public void MentionsReference() {
            Expression expression = CreateDisposableExpression(Instance.LoggedInMember, "A disposable conversation");
            expression.Mentions.Add(Instance.LoggedInMember);
            expression.Save();

            Expression persistedExpression = Instance.Get.ExpressionByID(expression.ID);
            CollectionAssert.Contains(persistedExpression.Mentions, Instance.LoggedInMember);
            CollectionAssert.Contains(Instance.LoggedInMember.MentionedInExpressions, persistedExpression);
        }

        [Test]
        public void BaseAssetsReference() {
            Story story = EntityFactory.CreateStory("Conversations related", SandboxProject);
            Expression expression = CreateDisposableExpression(Instance.LoggedInMember, "A disposable conversation");
            expression.Mentions.Add(story);
            expression.Save();

            Expression persistedExpression = Instance.Get.ExpressionByID(expression.ID);
            Story persistedStory = Instance.Get.StoryByID(story.ID);
            CollectionAssert.Contains(persistedExpression.Mentions, persistedStory);
            CollectionAssert.Contains(persistedStory.MentionedInExpressions, persistedExpression);
        }

        [Test]
        public void AuthorReference() {
            Expression expression = CreateDisposableExpression("A disposable conversation");

            Expression persistedExpression = Instance.Get.ExpressionByID(expression.ID);
            Assert.AreEqual(Instance.LoggedInMember, persistedExpression.Author);
            CollectionAssert.Contains(Instance.LoggedInMember.Expressions, persistedExpression);
        }

        //TODO:This test is no longer valid because expressions are not self referencing in this way
/*        [Test]
        public void ParentConversationReference() {
            Expression parent = CreateDisposableExpression(Instance.LoggedInMember, "A disposable conversation - parent");
            parent.Save();
            Conversation child = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation - child");
            child.ParentConversation = parent;
            child.Save();

            Conversation persistedParentConversation = Instance.Get.ConversationByID(parent.ID);
            Conversation persistedChildConversation = Instance.Get.ConversationByID(child.ID);
            Assert.AreEqual(persistedChildConversation.ParentConversation, persistedParentConversation);
            CollectionAssert.Contains(persistedParentConversation.ExpressionsInConversation, persistedChildConversation);
        }*/

        [Test]
        public void RepliesReference() {
            Expression parent = CreateDisposableExpression(Instance.LoggedInMember, "A disposable conversation - parent");
            Expression child = Instance.Create.Expression(Instance.LoggedInMember, "A disposable conversation - child", null, parent);
            child.Save();

            Expression topicExpression = Instance.Get.ExpressionByID(parent.ID);
            Expression replyExpression = Instance.Get.ExpressionByID(child.ID);
            Assert.AreEqual(replyExpression.InReplyTo, topicExpression);
            CollectionAssert.Contains(topicExpression.Replies, replyExpression);
        }

/*        [Test]
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
        }*/

/*        [Test]
        public void GetConversationById() {
            Conversation conversation = CreateDisposableConversation(Instance.LoggedInMember, "A disposable conversation");
            conversation.Save();

            Conversation persistedConversation = Instance.Get.ConversationByID(conversation.ID);
            Assert.AreEqual(conversation, persistedConversation);
        }*/

        private Expression CreateDisposableExpression(string content)
        {
            return CreateDisposableExpression(Instance.LoggedInMember, content);
        }

        private Expression CreateDisposableExpression(Member author, string content)
        {
            Conversation conversation = Instance.Create.Conversation(author, content);
            disposableConversations.Add(conversation);
            var expression = conversation.ContainedExpressions.FirstOrDefault();
            disposableExpressions.Add(expression);
            return expression;
        }

/*        private Conversation CreateDisposableConversation(Member author, string content) {
            Conversation conversation = Instance.Create.Conversation(author, content);
            conversation.Save();
            disposableConversations.Add(conversation);

            return conversation;
        }*/
    }
}
