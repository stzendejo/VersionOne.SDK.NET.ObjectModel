using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
	[TestFixture]
    public class MemberTester : BaseSDKTester {
        [Test]
        public void GetByName() {
            var members = Instance.Get.Members(null);

            var fred = members.FirstOrDefault(member => member.Username != null);

            Assert.IsNotNull(fred);

            var oldID = fred.ID;
			ResetInstance();
			fred = Instance.Get.MemberByID( oldID );

			Assert.AreEqual( fred.ID, Instance.Get.MemberByUserName( fred.Username ).ID );
		}

        [Test]
        public void Create() {
            const string phone = "555-555-5555";
            const string email = "test@test.com";

            var member = Instance.Create.Member("Test User", Guid.NewGuid().ToString());
			member.Email = email;
			member.Phone = phone;
			member.Save();

            var memberId = member.ID;

			ResetInstance();

			member = Instance.Get.MemberByID(memberId);

			Assert.AreEqual(email, member.Email);
			Assert.AreEqual(phone, member.Phone);
			Assert.IsFalse(member.NotifyViaEmail);
            Assert.IsTrue(DateTime.Now.Subtract(member.CreateDate).TotalMinutes < 10);
            Assert.IsTrue(DateTime.Now.Subtract(member.ChangeDate).TotalMinutes < 10);

            member.Delete();
        }

        [Test]
        public void CreateMemberWithRequiredAttributes() {
            const string description = "Test for Member creation with required attributes";
            const string phone = "555-555-5555";
            const string email = "test@test.com";

            var attributes = new Dictionary<string, object> {
                {"Description", description}, 
                {"Phone", phone}, 
                {"Email", email}
            };
            
            var member = Instance.Create.Member("Test User", Guid.NewGuid().ToString(), attributes);
            var memberId = member.ID;

            ResetInstance();

            member = Instance.Get.MemberByID(memberId);
            
            Assert.AreEqual(description, member.Description);
            Assert.AreEqual(email, member.Email);
            Assert.AreEqual(phone, member.Phone);

            member.Delete();
        }

        [Test]
        public void CreateWithComment() {
            const string phone = "!@#$%|^&\'<>555-555-5555";
            const string email = "!@#$%|^&\'<>test@test.com";
            const string user = "Test User with comment<>";
            const string change = " edited";
            const string comment = "Comment asd !@#$%^&*)' тест <>";

            var member = Instance.Create.Member(user, Guid.NewGuid().ToString());
            member.Email = email;
            member.Phone = phone;
            member.Save(comment);

            var memberId = member.ID;

            ResetInstance();

            member = Instance.Get.MemberByID(memberId);

            Assert.AreEqual(email, member.Email);
            Assert.AreEqual(phone, member.Phone);
            Assert.AreEqual(user, member.Name);

            member.Name += change;
            member.Save(comment + change);

            ResetInstance();

            member = Instance.Get.MemberByID(memberId);
            Assert.AreEqual(user + change, member.Name);

            member.Delete();
        }

        [Test]
        public void OwnedSeconardyWorkitems() {
            var story = SandboxProject.CreateStory("Story");
            var task = story.CreateTask("Task");
            story.CreateTest("Test");

            var member = Instance.Get.MemberByID("Member:20");

            task.Owners.Add(member);

            var filter = new SecondaryWorkitemFilter();
            filter.Project.Add(SandboxProject);
            var items = member.GetOwnedSecondaryWorkitems(filter);
            CollectionAssert.AreEqual(new string[] { "Task" }, DeriveListOfNamesFromAssets(items));
        }

        [Test]
        public void SendConversationEmailsSetting() {
            var member = Instance.LoggedInMember;
            var sendConversationEmails = member.SendConversationEmails;
            member.SendConversationEmails = !sendConversationEmails;
            member.Save();

            Assert.AreNotEqual(member.SendConversationEmails, sendConversationEmails);
        }
	}
}
