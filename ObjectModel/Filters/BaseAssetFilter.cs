using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter on BaseAssets
    /// </summary>
    public class BaseAssetFilter : EntityFilter {
        internal override Type EntityType {
            get { return typeof (BaseAsset); }
        }

        /// <summary>
        /// Item name. Must be complete match.
        /// </summary>
        public readonly ICollection<string> Name = new List<string>();

        /// <summary>
        /// Item description. Must be complete match.
        /// </summary>
        public readonly ICollection<string> Description = new List<string>();

        /// <summary>
        /// Current State of the asset.
        /// </summary>
        public ICollection<State> State {
            get { return state; }
        }

        private readonly IList<State> state = new List<State>();

        internal virtual bool HasState {
            get { return State.Count == 1; }
        }

        internal virtual bool HasActive {
            get { return State.Contains(Filters.State.Active); }
        }

        internal virtual bool HasClosed {
            get { return State.Contains(Filters.State.Closed); }
        }

		/// <summary>
		///  A collection of name=value pairs that will be added to the "where" query
		/// </summary>
		public readonly List<KeyValuePair<string, string>> ArbitraryWhereTerms = new List<KeyValuePair<string, string>>();


        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Simple("Name", Name);
            builder.Simple("Description", Description);
			foreach (var kvp in ArbitraryWhereTerms)
			{
				builder.Simple(kvp.Key, kvp.Value);
			}
        }

        internal override void InternalModifyState(FilterBuilder builder) {
            // The basic idea is to exclude 'dead' (Epic'd and Deleted) stuff, exept Epic'd Stories
            // Also take into account the fact that we usually want only active stuff, unless 'Closed' is specified
            if(HasState) {
                builder.Root.And(HasActive ? new TokenTerm("(AssetState='Active')") : new TokenTerm("(AssetState='Closed')"));
            } else {
                builder.Root.And(new TokenTerm("(AssetState!='Dead')"));
            }
        }
    }
}