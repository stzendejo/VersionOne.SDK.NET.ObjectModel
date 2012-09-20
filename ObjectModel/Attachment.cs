using System;
using System.IO;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents an attachment in the VersionOne system
	/// </summary>
	[MetaData("Attachment")]
	public class Attachment : Entity
	{
		internal Attachment(V1Instance instance) : base(instance)
		{
		    Set("Content",string.Empty);
		}
		internal Attachment(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// URL to VersionOne Detail Page for this Attachment
		/// </summary>
		public virtual string URL { get { return GetEntityURL(); } }

		/// <summary>
		/// VersionOne URL where the contents of this attachment may be downloaded from
		/// </summary>
		public string ContentURL { get { return Instance.GetAttachmentURL(this); } }

		/// <summary>
		/// Asset this attachment is related to
		/// </summary>
		public BaseAsset Asset { get { return GetRelation<BaseAsset>("Asset"); } set { SetRelation("Asset", value); } }

		/// <summary>
		/// Description of this attachment
		/// </summary>
		public string Description { get { return Get<string>("Description"); } set { Set("Description", value); } }

		/// <summary>
		/// Content Type of this attachment
		/// </summary>
		public string ContentType { get { return Get<string>("ContentType"); } set { Set("ContentType", value); } }

		/// <summary>
		/// Filename of this attachment
		/// </summary>
		public string Filename { get { return Get<string>("Filename"); } set { Set("Filename", value); } }

		/// <summary>
		/// Name of this attachment
		/// </summary>
		public string Name { get { return Get<string>("Name"); } set { Set("Name", value); } }

		/// <summary>
		/// Type of this attachment
		/// </summary>
		[MetaRenamed("Category")]
		public IListValueProperty Type { get { return GetListValue<AttachmentType>("Category"); } }

        /// <summary>
        /// Write this attachment's content to the output stream
        /// </summary>
        /// <param name="output">Stream to write the content to</param>
        public void WriteTo(Stream output)
        {
			using (Stream input = GetReadStream())
				StreamCopier.CopyStream(input, output);
        }

        /// <summary>
        /// Read the attachment's content from the input stream
        /// </summary>
        /// <remarks>Set the ContentType and Filename properties before calling this method</remarks>
        /// <param name="input">Stream to read the content from</param>
        public void ReadFrom(Stream input)
        {
			using (Stream output = GetWriteStream())
				StreamCopier.CopyStream(input, output);
			CommitWriteStream(ContentType);
        }

		/// <summary>
		/// True if the attachment can be deleted
		/// </summary>
		public virtual bool CanDelete { get { return Instance.CanExecuteOperation(this, "Delete"); } }

		/// <summary>
		/// Deletes the attachment
		/// </summary>
		/// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
		public void Delete()
		{
			Save();
			Instance.ExecuteOperation(this, "Delete");
		}
	}
}
