using System;

namespace VersionOne.SDK.ObjectModel
{
	/// <summary>
	/// List of roles allowed on a member
	/// </summary>
	/// <remarks>The id's of these roles must match the Role id's in the VersionOne system</remarks>
	[MetaData("Role")]
	public class Role : Entity
	{
		internal Role(V1Instance instance) : base(instance) { }
		internal Role(AssetID id, V1Instance instance) : base(id, instance) { }

		private Role(AssetID id) : base(id, null) { }

		/// <summary>
		/// Has no read or write access to any entities in the VersionOne System
		/// </summary>
		public static Role NoAccess = new Role("Role:0");
		/// <summary>
		/// Has full access to all entities in the VersionOne System
		/// Can administer members, custom attributes, list values, and system wide configuration
		/// </summary>
		public static Role SystemAdmin = new Role("Role:1");
		/// <summary>
		/// Has full read and write access to all entities in the VersionOne System
		/// Can manage Project Membership, Program Membership, Members, Member Groups, Member Security, and Teams
		/// </summary>
		public static Role MemberAdmin = new Role("Role:12");
		/// <summary>
		/// Has full read and write access to all entities in the VersionOne System
		/// Can manage Project Membership, Program Membership, and Teams
		/// </summary>
		public static Role ProjectAdmin = new Role("Role:2");
		/// <summary>
		/// Has full read and write access to all entities in the VersionOne System
		/// Cannot manage Project Membership or Program Membership
		/// </summary>
		public static Role ProjectLead = new Role("Role:3");
		/// <summary>
		/// Has read access to all entities in the VersionOne System
		/// Has write access to all entities except Projects, Iterations, and Goals in the VersionOne System
		/// </summary>
		public static Role TeamMember = new Role("Role:4");
		/// <summary>
		/// Has read access to all entities in the VersionOne System
		/// Has write access to Tasks, Effort, Issues, Requests, and Defects in the VersionOne System
		/// </summary>
		public static Role Developer = new Role("Role:5");
		/// <summary>
		/// Has read access to all entities in the VersionOne System
		/// Has write access to Tests, Effort, Issues, Requests, and Defects in the VersionOne System
		/// </summary>
		public static Role Tester = new Role("Role:6");
		/// <summary>
		/// Has read access to all entities in the VersionOne System
		/// Has write access to Themes, Stories, Tests, Issues, Requests, Defects, and Goals in the VersionOne System
		/// </summary>
		public static Role Customer = new Role("Role:7");
		/// <summary>
		/// Has read access but no write access to all entities in the VersionOne System
		/// </summary>
		public static Role Observer = new Role("Role:8");
		/// <summary>
		/// Has read access to Projects, Themes, Stories, Iterations, Defects, Retrospectives, and Goals but no write access in the VersionOne System
		/// </summary>
		public static Role Visitor = new Role("Role:9");
		/// <summary>
		/// Has read access to Projects, Themes, Iterations, and Goals but no write access in the VersionOne System
		/// </summary>
		public static Role Inheritor = new Role("Role:10");
		/// <summary>
		/// Has read access to all entities in the VersionOne System and write access to Requests
		/// </summary>
		public static Role Requestor = new Role("Role:11");

		/// <summary>
		/// Returns the name of the Role.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Get<string>("Name");
		}

		/// <summary>
		/// Role's cannot be updated or saved
		/// </summary>
		/// <param name="comment"></param>
		private new void Save(string comment)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Role's cannot be updated or saved
		/// </summary>
		private new void Save()
		{
			throw new NotSupportedException();
		}
	}
}