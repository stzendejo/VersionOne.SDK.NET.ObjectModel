using System;

using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    [Ignore("VersionOne server accessible through both proxy and directly is required")]
    public class ProxyTester {
        private const string V1Path = "http://integsrv01.internal.corp/VersionOneTest/";
        private const string V1Username = "admin";
        private const string V1Password = "admin";

        private const string ProxyPath = "http://integvm01:3128";
        private const string ProxyUsername = "user1";
        private const string ProxyPassword = "user1";

        [Test]
        public void QueryProjects() {
            var proxySettings = new ProxySettings(new Uri(ProxyPath), ProxyUsername, ProxyPassword);

            var proxiedInstance = new V1Instance(V1Path, V1Username, V1Password, false, proxySettings);
            var instance = new V1Instance(V1Path, V1Username, V1Password, false);

            var projects = instance.Get.Projects(null);
            var projectsOverProxy = proxiedInstance.Get.Projects(null);

            Assert.AreEqual(projects.Count, projectsOverProxy.Count);
        }
    }
}