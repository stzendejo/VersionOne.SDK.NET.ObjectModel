using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    [TestFixture]
    public class EffortTester : BaseSDKTester {
        [Test]
        public void GetById() {
            var effort = Instance.Get.EffortByID(AssetID.FromToken("Actual:1448"));
            Assert.IsNotNull(effort);
            Assert.AreEqual(4.0, effort.Value, 0);
            Assert.AreEqual("Boris Tester", effort.Member.Name);    	
        }
    }
}
