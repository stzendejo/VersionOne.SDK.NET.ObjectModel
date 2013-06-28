using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class ExpressionFilterTester : BaseSDKTester {
        private readonly Stack<Expression> expressionsForDelete = new Stack<Expression>();

        [Test]
        public void GetExpressionByFilter() {
            const string content = "testing converstation";

            Member member = Instance.LoggedInMember;
            Expression expression = CreateExpression(member, content);                        
            ExpressionFilter filter = new ExpressionFilter();
            filter.Author.Add(member);
            filter.AuthoredAt.AddTerm(FilterTerm.Operator.Equal, expression.AuthoredAt);

            IList<Expression> expressions = new List<Expression>(Instance.Get.Expressions(filter));
            Assert.AreEqual(1, expressions.Count);
            Expression newConv = expressions[0];
            Assert.AreEqual(expression.ID, newConv.ID);
            Assert.AreEqual(expression.Content, newConv.Content);
            Assert.AreEqual(member, newConv.Author);
        }

        //TODO:This test is no longer valid because expressions are not self referencing in this way
/*      [Test]
        public void GetExpressionByParent() {
            Expression parent = CreateExpression(Instance.LoggedInMember, "parent");
            Expression child = CreateExpression(Instance.LoggedInMember, "child");
            child.ParentExpression = parent;
            child.Save();
            Expression unrelated = CreateExpression(Instance.LoggedInMember, "something else");
            unrelated.ParentExpression = child;
            unrelated.Save();

            ExpressionFilter filter = new ExpressionFilter();
            filter.Expression.Add(parent);
            ICollection<Expression> Expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(2, Expressions.Count);
            CollectionAssert.Contains(Expressions, child);
            CollectionAssert.Contains(Expressions, parent);
            CollectionAssert.DoesNotContain(Expressions, unrelated);
        }*/

        [Test]
        public void GetExpressionByReplyReferences() {
            Expression @base = CreateExpression(Instance.LoggedInMember, "base");
            Expression reply = Instance.Create.Expression(Instance.LoggedInMember, "a reply", null, @base);
            Expression unrelated = CreateExpression(Instance.LoggedInMember, "something else");

            ExpressionFilter filter = new ExpressionFilter();
            filter.InReplyTo.Add(@base);
            ICollection<Expression> expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, reply);
            CollectionAssert.DoesNotContain(expressions, unrelated);

            filter = new ExpressionFilter();
            filter.Replies.Add(reply);
            expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, @base);
            CollectionAssert.DoesNotContain(expressions, unrelated);
        }

        [Test]
        public void GetExpressionByMentions() {
            Member firstMember = EntityFactory.CreateMember("test1");
            Expression firstExpression = CreateExpression(Instance.LoggedInMember, "testing - #1");
            firstExpression.Mentions.Add(firstMember);
            firstExpression.Save();

            Member secondMember = EntityFactory.CreateMember("test2");
            Expression secondExpression = CreateExpression(Instance.LoggedInMember, "testing - #2");
            secondExpression.Mentions.Add(secondMember);
            secondExpression.Save();

            ExpressionFilter filter = new ExpressionFilter();
            filter.Mentions.Add(firstMember);
            ICollection<Expression> expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, firstExpression);
            CollectionAssert.DoesNotContain(expressions, secondExpression);

            filter = new ExpressionFilter();
            filter.Mentions.Add(secondMember);
            expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, secondExpression);
            CollectionAssert.DoesNotContain(expressions, firstExpression);

            filter = new ExpressionFilter();
            filter.Mentions.Add(firstMember);
            filter.Mentions.Add(secondMember);
            expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(2, expressions.Count);
            CollectionAssert.Contains(expressions, secondExpression);
            CollectionAssert.Contains(expressions, firstExpression);
        }

        [Test]
        public void GetExpressionByBaseAssets() {
            Story story = EntityFactory.CreateStory("fly to the Moon using a magnet and will power", SandboxProject);
            Expression firstExpression = CreateExpression(Instance.LoggedInMember, "testing - #1");
            firstExpression.Mentions.Add(story);
            firstExpression.Save();

            Test test = EntityFactory.CreateTest("check the direction", story);
            Expression secondExpression = CreateExpression(Instance.LoggedInMember, "testing - #2");
            secondExpression.Mentions.Add(test);
            secondExpression.Save();

            ExpressionFilter filter = new ExpressionFilter();
            filter.Mentions.Add(story);
            ICollection<Expression> expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, firstExpression);
            CollectionAssert.DoesNotContain(expressions, secondExpression);

            filter = new ExpressionFilter();
            filter.Mentions.Add(test);
            expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(1, expressions.Count);
            CollectionAssert.Contains(expressions, secondExpression);
            CollectionAssert.DoesNotContain(expressions, firstExpression);

            filter = new ExpressionFilter();
            filter.Mentions.Add(story);
            filter.Mentions.Add(test);
            expressions = Instance.Get.Expressions(filter);

            Assert.AreEqual(2, expressions.Count);
            CollectionAssert.Contains(expressions, firstExpression);
            CollectionAssert.Contains(expressions, secondExpression);
        }

        private Expression CreateExpression(Member member, string content) {
            Expression expression = Instance.Create.Conversation(member, content).ContainedExpressions.FirstOrDefault();
            expressionsForDelete.Push(expression);
            return expression;
        }

        [TearDown]
        public new void TearDown() 
        {
            while (expressionsForDelete.Count > 0) 
            {
                Expression item = expressionsForDelete.Pop();
            
                if (item.CanDelete) {
                    item.Delete();
                }
            }
        }
    }
}
