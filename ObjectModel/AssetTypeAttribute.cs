namespace VersionOne.SDK.ObjectModel
{
	internal class MetaDataAttribute : System.Attribute
	{
		public readonly string AssetTypeToken;
		public readonly string DefaultAttributeSelectionNames;
	    public readonly string DefaultOrderByToken;
		public readonly byte? AssetState;

		public MetaDataAttribute(string assetTypeToken)
		{
			AssetTypeToken = assetTypeToken;
		}

		public MetaDataAttribute(string assetTypeToken, byte assetState) : this(assetTypeToken)
		{
			AssetState = assetState;
		}

		public MetaDataAttribute(string assetTypeToken, string defaultSelectionNames) : this(assetTypeToken)
		{
			DefaultAttributeSelectionNames = defaultSelectionNames;
		}

        public MetaDataAttribute(string assetTypeToken, string defaultSelectionNames, string defaultOrderBy) : this(assetTypeToken, defaultSelectionNames)
        {
            DefaultOrderByToken = defaultOrderBy;
        }

		public MetaDataAttribute(string assetTypeToken, byte assetState, string defaultSelectionNames) : this(assetTypeToken, assetState)
		{
			DefaultAttributeSelectionNames = defaultSelectionNames;
		}
	}

    internal class MetaRenamedAttribute : System.Attribute
    {
        public readonly string RealName;

        public MetaRenamedAttribute(string realName)
        {
            RealName = realName;
        }
    }
}
