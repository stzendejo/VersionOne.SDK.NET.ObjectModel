using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
    [TestFixture]
    public class GetByIDTester : BaseSDKTester 
    {
        [Test]
        public void TestGetEffort()
        {
            Effort effort = Instance.Get.EffortByID(AssetID.FromToken("Actual:1448"));
            Assert.IsNotNull(effort);
            Assert.AreEqual(4, effort.Value);
            Assert.AreEqual("Boris Tester", effort.Member.Name);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestValidObjectWrongType() {
            Epic epic = EntityFactory.CreateEpic("test epic", SandboxProject);
            Story testMe = Instance.Get.StoryByID(epic.ID);
            Assert.IsNull(testMe);
//            _server.Get.StoryByID(AssetID.FromToken("Epic:1448"));  /// oid is correct for epic
                                                                    /// expected result if null
        }

    }
}
