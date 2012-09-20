using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class LinkTester : BaseSDKTester
	{
		[Test] 
        public void LinkAttributes()
		{
			Link link = Instance.Get.LinkByID("Link:1780");
			Project project = Instance.Get.ProjectByID("Scope:0");
			Assert.AreEqual(project, link.Asset);
			Assert.AreEqual("Link A", link.Name);
			Assert.IsTrue(link.OnMenu);
			Assert.AreEqual("http://www.google.com/?q=A",link.URL);
		}

		[Test] 
        public void Create()
		{
			Project project = Instance.Get.ProjectByID("Scope:0");
			Link linkY = project.CreateLink("Link Y", "http://www.google.com?q=Y", false);
			Link linkZ = project.CreateLink("Link Z","http://www.google.com?q=Z", true);

			string linkYid = linkY.ID;
			string linkZid = linkZ.ID;

			ResetInstance();

			Link newLinkY = Instance.Get.LinkByID(linkYid);
			Assert.AreEqual("Link Y", newLinkY.Name);
			Assert.AreEqual("http://www.google.com?q=Y", newLinkY.URL);
			Assert.IsFalse(newLinkY.OnMenu);

			Link newLinkZ = Instance.Get.LinkByID(linkZid);
			Assert.AreEqual("Link Z", newLinkZ.Name);
			Assert.AreEqual("http://www.google.com?q=Z", newLinkZ.URL);
			Assert.IsTrue(newLinkZ.OnMenu);
		}

	    [Test]
		public void Delete()
		{
            Project project = Instance.Get.ProjectByID("Scope:0");
            Link linkY = project.CreateLink("Link Y", "http://www.google.com?q=Y", false);

            string linkYid = linkY.ID;
            
            ResetInstance();

			Link link = Instance.Get.LinkByID(linkYid);
			Assert.IsNotNull(link);
			Assert.IsTrue(link.CanDelete);
			link.Delete();
			ResetInstance();
			Assert.IsNull(Instance.Get.LinkByID(linkYid));
		}
	}
}