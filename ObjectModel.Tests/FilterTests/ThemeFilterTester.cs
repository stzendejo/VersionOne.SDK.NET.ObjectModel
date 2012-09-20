using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
	public class ThemeFilterTester : BaseSDKTester
	{
		private Project _sandboxProject;
		protected Member _andre;
		protected Member _danny;
		protected Theme _theme1;

		private ThemeFilter GetFilter()
		{
			ThemeFilter filter = new ThemeFilter();
			filter.Project.Add(_sandboxProject);
			return filter;
		}

		[SetUp]
		public void CreateAssets()
		{
			if (_sandboxProject == null)
			{
				_sandboxProject = SandboxProject;

				_andre = Instance.Get.MemberByID("Member:1000");
				_danny = Instance.Get.MemberByID("Member:1005");

				Theme theme1 = _sandboxProject.CreateTheme("Theme 1");
				Theme theme2 = _sandboxProject.CreateTheme("Theme 2");
				Theme theme11 = theme1.CreateChildTheme("Child Theme 1");

				theme1.Customer = _andre;
				theme1.Source.CurrentValue = "Customer";
				theme1.Risk.CurrentValue = "Medium";
				theme1.Priority.CurrentValue = "Medium";
				theme1.Save();

				theme2.Customer = _danny;
				theme2.Source.CurrentValue = "Sales";
				theme2.Risk.CurrentValue = "Low";
				theme2.Priority.CurrentValue = "Low";
				theme2.Save();

				theme11.Customer = _andre;
				theme11.Source.CurrentValue = "Customer";
				theme11.Risk.CurrentValue = "Medium";
				theme11.Priority.CurrentValue = "Medium";
				theme11.Source.CurrentValue = "Sales";
				theme11.Save();

				_theme1 = theme1;
			}
		}

		[Test] public void Customer()
		{
			ThemeFilter filter = GetFilter();
			filter.Customer.Add(_andre);
			Assert.AreEqual(2, Instance.Get.Themes(filter).Count);
		}

		[Test] public void Risk()
		{
			ThemeFilter filter = GetFilter();
			filter.Risk.Add("Low");
			Assert.AreEqual(1, Instance.Get.Themes(filter).Count);
		}

		[Test] public void Priority()
		{
			ThemeFilter filter = GetFilter();
			filter.Priority.Add("Medium");
			Assert.AreEqual(2, Instance.Get.Themes(filter).Count);
		}

		[Test] public void Type()
		{
			ThemeFilter filter = GetFilter();
			filter.Type.Add(null);
			Assert.AreEqual(3, Instance.Get.Themes(filter).Count);
		}

		[Test] public void Source()
		{
			ThemeFilter filter = GetFilter();
			filter.Source.Add("Sales");
			Assert.AreEqual(2, Instance.Get.Themes(filter).Count);
		}

		[Test] public void Parent()
		{
			ThemeFilter filter = GetFilter();
			filter.Parent.Add(_theme1);
			Assert.AreEqual(1, Instance.Get.Themes(filter).Count);
		}
	}
}