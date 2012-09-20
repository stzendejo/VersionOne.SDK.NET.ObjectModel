using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class ConversationFilterTester : BaseSDKTester {
        private readonly Stack<Conversation> conversationsForDelete = new Stack<Conversation>();

        [Test]
        public void GetConversationByFilter() {
            const string content = "testing converstation";

            Member member = Instance.LoggedInMember;
            Conversation conversation = CreateConversation(member, content);                        
            ConversationFilter filter = new ConversationFilter();
            filter.Author.Add(member);
            filter.AuthoredAt.AddTerm(FilterTerm.Operator.Equal, conversation.AuthoredAt);

            IList<Conversation> conversations = new List<Conversation>(Instance.Get.Conversations(filter));
            Assert.AreEqual(1, conversations.Count);
            Conversation newConv = conversations[0];
            Assert.AreEqual(conversation.ID, newConv.ID);
            Assert.AreEqual(conversation.Content, newConv.Content);
            Assert.AreEqual(member, newConv.Author);
        }

        [Test]
        public void GetConversationByParent() {
            Conversation parent = CreateConversation(Instance.LoggedInMember, "parent");
            Conversation child = CreateConversation(Instance.LoggedInMember, "child");
            child.ParentConversation = parent;
            child.Save();
            Conversation unrelated = CreateConversation(Instance.LoggedInMember, "something else");
            unrelated.ParentConversation = child;
            unrelated.Save();

            ConversationFilter filter = new ConversationFilter();
            filter.Conversation.Add(parent);
            ICollection<Conversation> conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(2, conversations.Count);
            CollectionAssert.Contains(conversations, child);
            CollectionAssert.Contains(conversations, parent);
            CollectionAssert.DoesNotContain(conversations, unrelated);
        }

        [Test]
        public void GetConversationByReplyReferences() {
            Conversation @base = CreateConversation(Instance.LoggedInMember, "base");
            Conversation reply = CreateConversation(Instance.LoggedInMember, "a reply");
            reply.InReplyTo = @base;
            reply.Save();
            Conversation unrelated = CreateConversation(Instance.LoggedInMember, "something else");

            ConversationFilter filter = new ConversationFilter();
            filter.InReplyTo.Add(@base);
            ICollection<Conversation> conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, reply);
            CollectionAssert.DoesNotContain(conversations, unrelated);

            filter = new ConversationFilter();
            filter.Replies.Add(reply);
            conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, @base);
            CollectionAssert.DoesNotContain(conversations, unrelated);
        }

        [Test]
        public void GetConversationByMentions() {
            Member firstMember = EntityFactory.CreateMember("test1");
            Conversation firstConversation = CreateConversation(Instance.LoggedInMember, "testing - #1");
            firstConversation.Mentions.Add(firstMember);
            firstConversation.Save();

            Member secondMember = EntityFactory.CreateMember("test2");
            Conversation secondConversation = CreateConversation(Instance.LoggedInMember, "testing - #2");
            secondConversation.Mentions.Add(secondMember);
            secondConversation.Save();

            ConversationFilter filter = new ConversationFilter();
            filter.Mentions.Add(firstMember);
            ICollection<Conversation> conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, firstConversation);
            CollectionAssert.DoesNotContain(conversations, secondConversation);

            filter = new ConversationFilter();
            filter.Mentions.Add(secondMember);
            conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, secondConversation);
            CollectionAssert.DoesNotContain(conversations, firstConversation);

            filter = new ConversationFilter();
            filter.Mentions.Add(firstMember);
            filter.Mentions.Add(secondMember);
            conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(2, conversations.Count);
            CollectionAssert.Contains(conversations, secondConversation);
            CollectionAssert.Contains(conversations, firstConversation);
        }

        [Test]
        public void GetConversationByBaseAssets() {
            Story story = EntityFactory.CreateStory("fly to the Moon using a magnet and will power", SandboxProject);
            Conversation firstConversation = CreateConversation(Instance.LoggedInMember, "testing - #1");
            firstConversation.Mentions.Add(story);
            firstConversation.Save();

            Test test = EntityFactory.CreateTest("check the direction", story);
            Conversation secondConversation = CreateConversation(Instance.LoggedInMember, "testing - #2");
            secondConversation.Mentions.Add(test);
            secondConversation.Save();

            ConversationFilter filter = new ConversationFilter();
            filter.Mentions.Add(story);
            ICollection<Conversation> conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, firstConversation);
            CollectionAssert.DoesNotContain(conversations, secondConversation);

            filter = new ConversationFilter();
            filter.Mentions.Add(test);
            conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(1, conversations.Count);
            CollectionAssert.Contains(conversations, secondConversation);
            CollectionAssert.DoesNotContain(conversations, firstConversation);

            filter = new ConversationFilter();
            filter.Mentions.Add(story);
            filter.Mentions.Add(test);
            conversations = Instance.Get.Conversations(filter);

            Assert.AreEqual(2, conversations.Count);
            CollectionAssert.Contains(conversations, firstConversation);
            CollectionAssert.Contains(conversations, secondConversation);
        }

        private Conversation CreateConversation(Member member, string content) {
            Conversation conversation = Instance.Create.Conversation(member, content);
            conversationsForDelete.Push(conversation);
            return conversation;
        }

        [TearDown]
        public new void TearDown() 
        {
            while (conversationsForDelete.Count > 0) 
            {
                Conversation item = conversationsForDelete.Pop();
            
                if (item.CanDelete) {
                    item.Delete();
                }
            }
        }
    }
}
