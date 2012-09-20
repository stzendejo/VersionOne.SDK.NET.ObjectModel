using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests {
    public class PrimaryWorkitemFilterTesterBase : BaseSDKTester {
        protected Project sandboxProject;
        protected Story story1;
        private Story story2;
        private Story story3;
        private Epic epic1;
        private Epic epic2;
        protected Defect defect1;
        private Defect defect2;
        private Defect defect3;
        protected Member andre;
        protected Member danny;

        [SetUp]
        public void CreateAssets() {
            if (sandboxProject == null) {
                sandboxProject = SandboxProject;
                andre = Instance.Get.MemberByID("Member:1000");
                danny = Instance.Get.MemberByID("Member:1005");
                epic1 = Instance.Create.Epic("Epic 1", SandboxProject);
                epic2 = epic1.GenerateChildEpic();
                epic2.Name = "Epic 2";
                epic2.Save();
                story1 = SandboxProject.CreateStory("Story 1");
                story2 = SandboxProject.CreateStory("Story 2");
                story3 = SandboxProject.CreateStory("Story 3");
                defect1 = SandboxProject.CreateDefect("Defect 1");
                defect2 = SandboxProject.CreateDefect("Defect 2");
                defect3 = SandboxProject.CreateDefect("Defect 3");

                story1.Description = "ABCDEFGHIJKJMNOPQRSTUVWXYZ";
                story1.Save();

                story2.Description = "1234567890";
                story2.Save();

                story3.Description = "123 ABC";
                story3.Save();

                story1.Owners.Add(andre);
                story1.Owners.Add(danny);
                story3.Owners.Add(andre);

                defect2.Owners.Add(andre);
                defect3.Owners.Add(danny);

                defect1.FoundInBuild = "1.0.0.0";
                defect1.ResolvedInBuild = "1.0.0.2";
                defect1.Environment = "Windows";
                defect1.Estimate = 2.0;
                defect1.Reference = "123456";
                defect1.Save();

                defect2.AffectedByDefects.Add(defect1);
                defect2.FoundInBuild = "1.0.0.0";
                defect2.FoundBy = "Joe";
                defect2.VerifiedBy = andre;
                defect2.Environment = "Mac";
                defect2.Type.CurrentValue = "Documentation";
                defect2.ResolutionReason.CurrentValue = "Duplicate";
                defect2.Estimate = 1.0;
                defect2.Save();

                defect3.FoundInBuild = "1.0.0.1";
                defect3.FoundBy = "Bob";
                defect3.VerifiedBy = danny;
                defect3.Type.CurrentValue = "Code";
                defect3.ResolutionReason.CurrentValue = "Fixed";
                defect3.Save();
            }
        }
    }
}