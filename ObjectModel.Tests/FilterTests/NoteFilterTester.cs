using System.Collections.Generic;
using NUnit.Framework;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests.FilterTests
{
	[TestFixture]
    [Ignore("Notes are no longer supported in recent versions of VersionOne server")]
	public class NoteFilterTester : BaseSDKTester
	{
		private const string MyUsername = "andre";

		private V1Instance _instance;
		private V1Instance MyInstance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new V1Instance(ApplicationPath, MyUsername, MyUsername);
					_instance.Validate();
				}
				return _instance;
			}
		}

		[Test]
		public void Type()
		{
			Note expected = Instance.Create.Note("Has Type", SandboxProject, "one", false);
			Note not = Instance.Create.Note("No Type", SandboxProject, "two", false);

			string expectedType = expected.Type.AllValues[0];
			expected.Type.CurrentValue = expectedType;
			expected.Save();

			TestType(expected, not, expectedType);
			TestType(not, expected, null);
		}

		private void TestType(Note expected, Note not, string expectedType)
		{
			NoteFilter filter = new NoteFilter();
			filter.Asset.Add(SandboxProject);
			filter.Type.Add(expectedType);

			ResetInstance();
			expected = Instance.Get.NoteByID(expected.ID);
			not = Instance.Get.NoteByID(not.ID);

			ICollection<Note> results = SandboxProject.GetNotes(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Note that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Note that doesn't match filter.");
			foreach (Note result in results)
				Assert.AreEqual(expectedType, result.Type.CurrentValue);
		}

		[Test]
		public void InResponseTo()
		{
			Note note = Instance.Create.Note("No InResponseTo", SandboxProject, "Content one.", false);
			Note response = note.CreateResponse("Has InResponseTo", "Content two.", false);

			TestInResponseTo(note, response, null);
			TestInResponseTo(response, note, note);
		}

		void TestInResponseTo(Note expected, Note not, Note expectedNote)
		{
			NoteFilter filter = new NoteFilter();
			filter.Asset.Add(SandboxProject);
			filter.InResponseTo.Add(expectedNote);

			ResetInstance();
			expected = Instance.Get.NoteByID(expected.ID);
			not = Instance.Get.NoteByID(not.ID);

			ICollection<Note> results = SandboxProject.GetNotes(filter);

			Assert.IsTrue(FindRelated(expected, results), "Expected to find Note that matched filter.");
			Assert.IsFalse(FindRelated(not, results), "Expected to NOT find Note that doesn't match filter.");
			foreach (Note result in results)
				Assert.AreEqual(expectedNote, result.InResponseTo);
		}

		[Test]
		public void Personal()
		{
			Member member = MyInstance.Get.MemberByUserName(MyUsername);
			string personalId = MyInstance.Create.Note("Note to self", member, "hello", true).ID;
			string publicId = MyInstance.Create.Note("Hear me out", member, "blah", false).ID;

			TestPersonal(publicId, personalId, MyInstance, true);
			TestPersonal(publicId, personalId, Instance, false);
		}

		private void TestPersonal(string publicId, string personalId, V1Instance instance, bool hasPersonal)
		{
			Note pub = instance.Get.NoteByID(publicId);
			Note personal = instance.Get.NoteByID(personalId);
			ICollection<Note> notes = instance.Get.MemberByUserName(MyUsername).GetNotes(null);

			Assert.IsTrue(notes.Contains(pub));
			Assert.IsFalse(hasPersonal ^ notes.Contains(personal));
		}
	}
}
