using System;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class ChangeSetFilterTester : BaseSDKTester
	{
		[Test] public void Reference()
		{
			string reference = Guid.NewGuid().ToString();
			Instance.Create.ChangeSet("Test", reference);
			ResetInstance();
			ChangeSetFilter filter = new ChangeSetFilter();
			filter.Reference.Add(reference);
			Assert.AreEqual(1, Instance.Get.ChangeSets(filter).Count);
		}
	}
}