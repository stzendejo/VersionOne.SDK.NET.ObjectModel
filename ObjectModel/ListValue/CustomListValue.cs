using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.List
{
	internal class CustomListValue : ListValue
	{
		internal CustomListValue(V1Instance instance) : base(instance) {}

		internal CustomListValue(AssetID id, V1Instance instance) : base(id, instance) {}

		/// <summary>
		/// The Name
		/// </summary>
		public override string Name { get { return GetCustomListTypeAttribute<string>("Name"); } }

		/// <summary>
		/// The (optional) Description
		/// </summary>
		public override string Description { get { return GetCustomListTypeAttribute<string>("Description"); } }

		private T GetCustomListTypeAttribute<T>(string name)
		{
			return GetCustomListTypeAttribute<T>(name, true);
		}

		private T GetCustomListTypeAttribute<T>(string name, bool cachable)
		{
			return Instance.GetPropertyOnCustomType<T>(this, name, cachable);
		}
	}
}