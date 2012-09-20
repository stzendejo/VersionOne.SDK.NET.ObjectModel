using System;

namespace VersionOne.SDK.ObjectModel
{
	internal class StubAssetID
	{
		private readonly Guid _id = Guid.NewGuid();

		/// <summary>
		/// Override Equals
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			StubAssetID other = obj as StubAssetID;
			if (other == null) return false;
			if (object.ReferenceEquals(this, other)) return true;
			return _id == other._id;
		}

		/// <summary>
		/// Override Equals
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}

		/// <summary>
		/// Overload equal equal operator
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator ==(StubAssetID a, StubAssetID b)
		{
			if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
				return object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null);
			return a.Equals(b);
		}

		/// <summary>
		/// Overload not equal operator
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static bool operator !=(StubAssetID a, StubAssetID b)
		{
			return !(a == b);
		}
	}
}
