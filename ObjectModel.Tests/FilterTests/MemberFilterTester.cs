using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class MemberFilterTester : BaseSDKTester
	{
		[Test] public void ShortName()
		{
			Member admin = Instance.Get.MemberByID("Member:20");

			MemberFilter filter = new MemberFilter();
			string shortName = admin.ShortName;
			filter.ShortName.Add(shortName);

			ICollection<Member> members = Instance.Get.Members(filter);
			Assert.IsTrue(FindRelated(admin, members));

			foreach (Member member in members)
				Assert.AreEqual(shortName, member.ShortName);
		}

		[Test] public void UserName()
		{
			Member admin = Instance.Get.MemberByID("Member:20");

			MemberFilter filter = new MemberFilter();
			string userName = admin.Username;
			filter.Username.Add(userName);

			ICollection<Member> members = Instance.Get.Members(filter);
			Assert.IsTrue(FindRelated(admin, members));

			foreach (Member member in members)
				Assert.AreEqual(userName, member.Username);
		}
	}
}
