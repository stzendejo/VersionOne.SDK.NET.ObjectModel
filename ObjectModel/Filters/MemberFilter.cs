using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for getting Members
	/// </summary>
	public class MemberFilter : BaseAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Member); }
        }

		/// <summary>
		/// The short or abbreviated name of the user or member.  This name is often used in the owner's dropdown.
		/// </summary>
		public readonly ICollection<string> ShortName = new List<string>();
		
		/// <summary>
		/// The username this user or member uses to login to the VersionOne system
		/// </summary>
		public readonly ICollection<string> Username = new List<string>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("Nickname", ShortName);
			builder.Simple("Username", Username);
		}
	}
}
