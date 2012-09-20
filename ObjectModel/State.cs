namespace VersionOne.SDK.ObjectModel
{
	[MetaData("State")]
	internal class State : Entity
	{
		private State(AssetID id) : base(id, null) { }

		public static State Future = new State("State:100");
		public static State Active = new State("State:101");
		public static State Closed = new State("State:102");
	}
}
