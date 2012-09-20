using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class FindTester : BaseSDKTester {
        [Test]
        public void FindInDefautFields() {
            var projectFilter = new ProjectFilter();
            projectFilter.Find.SearchString = "System";
            Assert.AreEqual(1, Instance.Get.Projects(projectFilter).Count);
        }
    }
}