using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
    /// Represents a M0te in the VersionOne system
	/// </summary>
	[MetaData("Note")]
	public class Note : Entity
	{
		internal Note(V1Instance instance) : base(instance) { }
		internal Note(AssetID id, V1Instance instance) : base(id, instance) { }
		internal Note(Note responseTo, V1Instance instance) : base(instance)
		{
			SetRelation("InResponseTo", responseTo);
		}

		/// <summary>
        /// The type this n0te belongs to
		/// </summary>
		[MetaRenamed("Category")]
        public IListValueProperty Type { get { return GetListValue<NoteType>("Category"); } }

		/// <summary>
        /// Asset this n0te is related to
		/// </summary>
		public BaseAsset Asset { get { return GetRelation<BaseAsset>("Asset"); } set { SetRelation("Asset", value); } }

		/// <summary>
        /// True if this n0te is visible only to the currently logged in user
		/// </summary>
		public bool Personal { get { return Get<bool>("Personal"); } set { Set("Personal", value); } }

		/// <summary>
        /// Content of this n0te
		/// </summary>
		public string Content { get { return Get<string>("Content"); } set { Set("Content", value); } }

		/// <summary>
        /// Name of this n0te
		/// </summary>
		public string Name { get { return Get<string>("Name"); } set { Set("Name", value); } }

		/// <summary>
        /// N0te this n0te is a response to
		/// </summary>
		public Note InResponseTo { get { return GetRelation<Note>("InResponseTo"); } }

		/// <summary>
        /// Responses to this n0te
		/// </summary>
		public ICollection<Note> Responses { get { return GetMultiRelation<Note>("Responses"); } }

		/// <summary>
        /// True if the n0te can be deleted
		/// </summary>
		public virtual bool CanDelete { get { return Instance.CanExecuteOperation(this, "Delete"); } }

		/// <summary>
        /// Deletes the n0te
		/// </summary>
		/// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
		public void Delete()
		{
			Save();
			Instance.ExecuteOperation(this, "Delete");
		}

		/// <summary>
        /// Create a response to this n0te
		/// </summary>
        /// <param name="name">The name of the n0te</param>
        /// <param name="content">The content of the n0te</param>
        /// <param name="personal">True if the n0te is only visible to the currently logged in user</param>
		/// <returns></returns>
		public Note CreateResponse(string name, string content, bool personal)
		{
			return Instance.Create.Note(this, name, content, personal);
		}

        /// <summary>
        /// Create a response to this n0te
        /// </summary>
        /// <param name="name">The name of the n0te</param>
        /// <param name="content">The content of the n0te</param>
        /// <param name="personal">True if the n0te is only visible to the currently logged in user</param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        public Note CreateResponse(string name, string content, bool personal, IDictionary<string, object> attributes)
        {
            return Instance.Create.Note(this, name, content, personal, attributes);
        }
	}
}