using System;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// A Defect template
	/// </summary>
	[MetaData("Defect", 200)]
	public class DefectTemplate : ProjectAsset
	{
		internal DefectTemplate(V1Instance instance) : base(instance) { }
		internal DefectTemplate(AssetID id, V1Instance instance) : base(id, instance) { }

		/// <summary>
		/// Creates a Defect from this Template, copying Attributes and Relationships
		/// </summary>
		/// <returns>A Defect just like this Template</returns>
		public Defect GenerateDefect()
		{
			Save();
			return Instance.ExecuteOperation<Defect>(this, "Copy");	
		}

		internal override void CloseImpl()
		{
			throw new NotSupportedException();
		}

		internal override void ReactivateImpl()
		{
			throw new NotSupportedException();
		}
	}
}
