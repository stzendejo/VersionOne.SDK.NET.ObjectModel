using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace VersionOne.SDK.ObjectModel.Tests
{
	[TestFixture]
	public class AttachmentTester : BaseSDKTester
	{
		[Test]
		public void AttachmentProperties()
		{
			Attachment attachment = Instance.Get.AttachmentByID("Attachment:1783");
			Project project = Instance.Get.ProjectByID("Scope:0");
			Assert.AreEqual(project, attachment.Asset);
			Assert.AreEqual("text/plain", attachment.ContentType);
			Assert.AreEqual("Sample attachment<br>", attachment.Description);
			Assert.AreEqual("Attachment A", attachment.Name);
			Assert.AreEqual("sample.txt", attachment.Filename);
			using (MemoryStream output = new MemoryStream())
			{
				attachment.WriteTo(output);

				string srcval = "This is a sample attachment";
				string destval = Encoding.ASCII.GetString(output.ToArray());
				Assert.AreEqual(srcval, destval);
			}
		}

		[Test] public void URLTester()
		{
			Attachment attachment = Instance.Get.AttachmentByID("Attachment:1783");
			Assert.AreEqual(ApplicationPath + "attachment.v1/1783", attachment.ContentURL);
			Assert.AreEqual(ApplicationPath + "assetdetail.v1/?oid=Attachment:1783", attachment.URL);
		}

		[Test]
		public void Create()
		{
			Project project = Instance.Get.ProjectByID("Scope:0");
			Attachment attachment;
			string content = "This is the first attachment's content. At: " + System.DateTime.Now;
			using (MemoryStream input = new MemoryStream(Encoding.ASCII.GetBytes(content)))
				attachment = project.CreateAttachment("First Attachment","test.txt",input);

			string attachmentID = attachment.ID;

			ResetInstance();

			Attachment newAttachment = Instance.Get.AttachmentByID(attachmentID);

			Project newProject = Instance.Get.ProjectByID("Scope:0");
			Assert.AreEqual(newProject, newAttachment.Asset);
			Assert.AreEqual("text/plain", attachment.ContentType);
			Assert.AreEqual("test.txt", attachment.Filename);
			Assert.AreEqual("First Attachment", attachment.Name);

			using (MemoryStream output = new MemoryStream())
			{
				newAttachment.WriteTo(output);
				Assert.AreEqual(content, Encoding.ASCII.GetString(output.ToArray()));
			}
		}

        [Test]
        public void CreateAttachmentWithAttributes()
        {
            Project project = Instance.Get.ProjectByID("Scope:0");
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            const string description = "Test for Attachment creation with required attributes";
            attributes.Add("Description", description);

            Attachment attachment;
            string content = "This is the first attachment's content. At: " + System.DateTime.Now;
            using (MemoryStream input = new MemoryStream(Encoding.ASCII.GetBytes(content)))
                attachment = project.CreateAttachment("First Attachment", "test.txt", input, attributes);

            string attachmentID = attachment.ID;

            ResetInstance();

            Attachment newAttachment = Instance.Get.AttachmentByID(attachmentID);

            Project newProject = Instance.Get.ProjectByID("Scope:0");
            Assert.AreEqual(newProject, newAttachment.Asset);
            Assert.AreEqual(description, attachment.Description);

            using (MemoryStream output = new MemoryStream())
            {
                newAttachment.WriteTo(output);
                Assert.AreEqual(content, Encoding.ASCII.GetString(output.ToArray()));
            }
        }

		[Test] public void CreateFromFile()
		{
			Project project = Instance.Get.ProjectByID("Scope:0");
			Attachment attachment;
			using (FileStream input = new FileStream("logo.png", FileMode.Open))
				attachment = project.CreateAttachment("Second Attachment","logo.png",input);

			string attachmentID = attachment.ID;

			ResetInstance();

			Attachment newAttachment = Instance.Get.AttachmentByID(attachmentID);

			Assert.AreEqual("image/png", newAttachment.ContentType);

			using (MemoryStream output = new MemoryStream())
			{
				newAttachment.WriteTo(output);
				using (FileStream input = new FileStream("logo.png", FileMode.Open))
					Assert.IsTrue(StreamComparer.CompareStream(input, output));
			}
		}

		[Test]
		public void Delete()
		{
            Project project = Instance.Get.ProjectByID("Scope:0");
            Attachment attachment;
            string content = "This is the first attachment's content. At: " + System.DateTime.Now;
            using (MemoryStream input = new MemoryStream(Encoding.ASCII.GetBytes(content)))
                attachment = project.CreateAttachment("First Attachment", "test.txt", input);

            string attachmentID = attachment.ID;

            ResetInstance();

            attachment = Instance.Get.AttachmentByID(attachmentID);
			Assert.IsNotNull(attachment);
			Assert.IsTrue(attachment.CanDelete);
			attachment.Delete();
			ResetInstance();
            Assert.IsNull(Instance.Get.AttachmentByID(attachmentID));
		}

		[ExpectedException(typeof(AttachmentLengthExceededException))]
		[Test] public void MaximumFileSize()
		{
			Project project = Instance.Get.ProjectByID("Scope:0");
			using (Stream input = new RandomStream(Instance.Configuration.MaximumAttachmentSize + 1))
				project.CreateAttachment("Random Attachment", "random.txt", input);
		}

		[Test] public void UnderMaximumFileSize()
		{
			Project project = Instance.Get.ProjectByID("Scope:0");
			using (Stream input = new RandomStream(Instance.Configuration.MaximumAttachmentSize))
				project.CreateAttachment("Random Attachment", "random.txt", input);
		}
	}
}