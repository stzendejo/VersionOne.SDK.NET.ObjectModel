using System.Collections.Generic;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
    [Ignore("Notes are no longer supported in recent versions of VersionOne server")]
	public class NoteTester : BaseSDKTester
	{
		[Test]
		public void PublicNoteAttributes()
		{
			Note note = Instance.Get.NoteByID("Note:1785");
			Project project = Instance.Get.ProjectByID("Scope:0");
			Assert.AreEqual(project, note.Asset);
			Assert.AreEqual("Public Note", note.Name);
			Assert.AreEqual("This is a public note.<br>", note.Content);
			Assert.IsFalse(note.Personal);
			Assert.IsNotNull(note.Type);
			Assert.AreEqual("Status",note.Type.CurrentValue);
			Assert.AreEqual(Instance.LoggedInMember, note.CreatedBy);
		}

		[Test]
		public void PrivateNoteAttributes()
		{
			Note note = Instance.Get.NoteByID("Note:1786");
			Project project = Instance.Get.ProjectByID("Scope:0");
			Assert.AreEqual(project, note.Asset);
			Assert.AreEqual("Private Note", note.Name);
			Assert.AreEqual("This is a private note.<br>", note.Content);
			Assert.IsTrue(note.Personal);
			Assert.IsNotNull(note.Type);
			Assert.AreEqual("Comment",note.Type.CurrentValue);
			Assert.AreEqual(Instance.LoggedInMember, note.CreatedBy);
		}

		[Test]
		public void Create()
		{
			Project project = SandboxProject;
			Note noteY = project.CreateNote("Note Y", "This is Note Y.", false);
			Note noteZ = project.CreateNote("Note Z", "This is Note Z.", true);

			string noteYid = noteY.ID;
			string noteZid = noteZ.ID;

			ResetInstance();

			Note newNoteY = Instance.Get.NoteByID(noteYid);
			Assert.AreEqual("Note Y", newNoteY.Name);
			Assert.AreEqual("This is Note Y.", newNoteY.Content);
			Assert.IsFalse(newNoteY.Personal);

			Note newNoteZ = Instance.Get.NoteByID(noteZid);
			Assert.AreEqual("Note Z", newNoteZ.Name);
			Assert.AreEqual("This is Note Z.", newNoteZ.Content);
			Assert.IsTrue(newNoteZ.Personal);
		}

        [Test]
        public void CreateNoteWithRequiredAttributes()
        {
            const string description = "Test for Member creation with required attributes";
            const string name = "CreateNote";

            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Content", description);

            Project project = SandboxProject;
            Note note = project.CreateNote(name, "Wrong content", false, attributes);

            string noteId = note.ID;

            ResetInstance();

            Note newNote = Instance.Get.NoteByID(noteId);
            Assert.AreEqual(description, newNote.Content);
            Assert.AreEqual(name, newNote.Name);
        }

		[Test]
		public void Delete()
		{
			Note note = SandboxProject.CreateNote("New Note", "This is a new Note", false);
			string noteId = note.ID;

			ResetInstance();

			note = Instance.Get.NoteByID(noteId);
			Assert.IsTrue(note.CanDelete);
			note.Delete();

			ResetInstance();

			Assert.IsNull(Instance.Get.NoteByID(noteId));
		}

		[Test]
		public void CreateResponse()
		{
			Note note = SandboxProject.CreateNote("New Note", "This is a new Note", false);
			string noteId = note.ID;

			Note response = note.CreateResponse("A Response", "Back to you", true);
			string responseID = response.ID;

			ResetInstance();

			note = Instance.Get.NoteByID(noteId);
			response = Instance.Get.NoteByID(responseID);

			Assert.IsTrue(note.Responses.Contains(response));

			Assert.AreEqual("A Response", response.Name);
			Assert.AreEqual("Back to you", response.Content);
			Assert.IsTrue(response.Personal);
			Assert.AreEqual(note, response.InResponseTo);

			ICollection<Note> notes = SandboxProject.GetNotes(null);
			Assert.IsTrue(notes.Contains(note));
			Assert.IsTrue(notes.Contains(response));
		}
	}
}