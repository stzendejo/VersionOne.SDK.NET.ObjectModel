namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// Filter for getting teams.
	/// </summary>
	public class TeamFilter : BaseAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Team); }
        }
	}
}
