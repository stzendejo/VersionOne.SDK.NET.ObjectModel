using System.Collections.Generic;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class RequestTester : BaseSDKTester {
        [Test]
        public void CreateAndRetrieveRequest() {
            const string name = "New Name";

            var id = Instance.Create.Request(name, Instance.Get.ProjectByID("Scope:0")).ID;

            ResetInstance();

            var request = Instance.Get.RequestByID(id);

            Assert.AreEqual(request.Name, name);
        }

        [Test]
        public void CreateWithAttributes() {
            var attributes = new Dictionary<string, object> {
                {"Description", "Test for Request creation with required attributes"}
            };

            const string name = "New Name";

            var id = Instance.Create.Request(name, Instance.Get.ProjectByID("Scope:0"), attributes).ID;

            ResetInstance();

            var request = Instance.Get.RequestByID(id);

            Assert.AreEqual(request.Name, name);
            Assert.AreEqual(request.Description, "Test for Request creation with required attributes");
        }

        [Test]
        public void RequestOrder() {
            var project = Instance.Get.ProjectByID("Scope:0");
            var request1 = project.CreateRequest("Request 1");
            var request2 = project.CreateRequest("Request 2");

            var id1 = request1.ID;
            var id2 = request2.ID;

            request1.RankOrder.SetBelow(request2);

            Assert.IsTrue(request1.RankOrder.IsBelow(request2));
            Assert.IsTrue(request2.RankOrder.IsAbove(request1));

            ResetInstance();

            request1 = Instance.Get.RequestByID(id1);
            request2 = Instance.Get.RequestByID(id2);

            request1.RankOrder.SetAbove(request2);

            Assert.IsTrue(request1.RankOrder.IsAbove(request2));
            Assert.IsTrue(request2.RankOrder.IsBelow(request1));
        }
    }
}