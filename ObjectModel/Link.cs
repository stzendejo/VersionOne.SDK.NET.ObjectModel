using System;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// Represents a Link in the VersionOne system
	/// </summary>
	[MetaData("Link")]
	public class Link : Entity
	{
		internal Link(V1Instance instance) : base(instance) { }
		internal Link(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// Asset this link is related to
		/// </summary>
		public BaseAsset Asset { get { return GetRelation<BaseAsset>("Asset"); } set { SetRelation("Asset", value); } }

		/// <summary>
		/// True is this link is visible on the asset's detail page menu
		/// </summary>
		public bool OnMenu { get { return Get<bool>("OnMenu"); } set { Set("OnMenu", value); } }

		/// <summary>
		/// URL this link points to
		/// </summary>
		public string URL { get { return Get<string>("URL"); } set { Set("URL", value); } }

		/// <summary>
		/// Name of this link
		/// </summary>
		public string Name { get { return Get<string>("Name"); } set { Set("Name", value); } }

		/// <summary>
		/// True if the link can be deleted
		/// </summary>
		public virtual bool CanDelete { get { return Instance.CanExecuteOperation(this, "Delete"); } }

		/// <summary>
		/// Deletes the link
		/// </summary>
		/// <exception cref="InvalidOperationException">The item is an invalid state for the Operation.</exception>
		public void Delete()
		{
			Save();
			Instance.ExecuteOperation(this, "Delete");
		}
	}
}