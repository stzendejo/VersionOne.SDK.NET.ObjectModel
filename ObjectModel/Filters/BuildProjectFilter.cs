using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters
{
    ///<summary>
    /// A filter for Build Projects
    ///</summary>
    public class BuildProjectFilter : BaseAssetFilter
    {
        internal override System.Type EntityType
        {
            get { return typeof(BuildProject); }
        }

        /// <summary>
        /// Reference of the Build Project
        /// </summary>
        public readonly ICollection<string> References = new List<string>();

        internal override void InternalModifyFilter(FilterBuilder builder)
        {
            base.InternalModifyFilter(builder);

            builder.Simple("Reference", References);
        }

        internal override void InternalModifyState(FilterBuilder builder)
        {
            if (HasState)
                if (HasActive)
                    builder.Root.And(new TokenTerm("AssetState='Active';AssetType='BuildProject'"));
                else
                    builder.Root.And(new TokenTerm("AssetState='Closed';AssetType='BuildProject'"));
            else
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='BuildProject'"));
        }
    }
}
