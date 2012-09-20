namespace VersionOne.SDK.ObjectModel.List
{
	
	/// <summary>
	/// Represents a List Type Asset
	/// </summary>
	public abstract class ListValue : Entity
	{
		internal ListValue(V1Instance instance) : base(instance) {}

		internal ListValue(AssetID id, V1Instance instance) : base(id, instance) {}

		/// <summary>
		/// The Name
		/// </summary>
		public virtual string Name { get { return Get<string>("Name"); } }

		/// <summary>
		/// The (optional) Description
		/// </summary>
		public virtual string Description { get { return Get<string>("Description"); } }

		/// <summary>
		/// Return name of ListValue.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}
	}
}
