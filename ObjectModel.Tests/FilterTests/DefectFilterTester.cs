using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
	[TestFixture]
    public class DefectFilterTester : PrimaryWorkitemFilterTesterBase {
        private DefectFilter GetFilter() {
            var filter = new DefectFilter();
            filter.Project.Add(SandboxProject);
			return filter;
		}

        [Test]
        public void FoundBy() {
            var filter = GetFilter();
			filter.FoundBy.Add("Joe");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect FoundBy \"Joe\"");
		}

        [Test]
        public void Type() {
            var filter = GetFilter();
			filter.Type.Add("Code");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect of Type \"Code\"");
		}

        [Test]
        public void TypeAndFoundBy() {
            var filter = GetFilter();
			filter.Type.Add("Code");
			filter.FoundBy.Add("Bob");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect of Type \"Code\" and  FoundBy \"Bob\"");

			filter = GetFilter();
			filter.Type.Add("Documentation");
			filter.FoundBy.Add("Bob");
			Assert.AreEqual(0, Instance.Get.Defects(filter).Count, "Should be no defects of Type \"Documentation\" and  FoundBy \"Bob\"");
		}

        [Test]
        public void FoundInBuild() {
            var filter = GetFilter();
			filter.FoundInBuild.Add("1.0.0.0");
			Assert.AreEqual(2, Instance.Get.Defects(filter).Count, "Should be two defects FoundInBuild \"1.0.0.0\"");
		}

        [Test]
        public void Environment() {
            var filter = GetFilter();
			filter.Environment.Add("Windows");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect with Environment of \"Windows\"");
		}

        [Test]
        public void ResolvedInBuild() {
            var filter = GetFilter();
			filter.ResolvedInBuild.Add("1.0.0.2");
			filter.OrderBy.Add("ResolvedInBuild");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect ResolvedInBuild \"1.0.0.2\"");
		}

        [Test]
        public void VerifiedBy() {
            var filter = GetFilter();
			filter.VerifiedBy.Add(null);
            filter.VerifiedBy.Add(danny);
			Assert.AreEqual(2, Instance.Get.Defects(filter).Count, "Should be two defects VerifiedBy Danny or no one.");
		}

        [Test]
        public void ResolutionReason() {
            var filter = GetFilter();
			filter.ResolutionReason.Add("Fixed");
			Assert.AreEqual(1, Instance.Get.Defects(filter).Count, "Should be one defect of ResolutionReason \"Fixed\"");
		}

        [Test]
        public void Estimate() {
            var filter = GetFilter();
            filter.Estimate.AddTerm(FilterTerm.Operator.Equal, 1.0);
            Assert.AreEqual(1, Instance.Get.Defects(filter).Count);

            filter.Estimate.Clear();
            filter.Estimate.AddTerm(FilterTerm.Operator.NotExists);
            Assert.AreEqual(1, Instance.Get.Defects(filter).Count);

            filter.Estimate.Clear();
            filter.Estimate.AddTerm(FilterTerm.Operator.Exists);
			Assert.AreEqual(2, Instance.Get.Defects(filter).Count);
		}

        [Test]
        public void NoProjectAmongDefects() {
            var sandboxName = SandboxProject.Name;
			ResetInstance();
            CollectionAssert.DoesNotContain(DeriveListOfNamesFromAssets(Instance.Get.BaseAssets(new DefectFilter())), sandboxName);
		}
	}
}
