using System;
using System.Collections.Generic;
using System.Linq;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Base class of all filters
    /// </summary>
    public abstract class EntityFilter {
        /// <summary>
        /// Specifies a string to search for and an optional set of fields to search in.
        /// </summary>
        public class StringSearcher {
            /// <summary>
            /// Specify the fields to search in. Add the names of any string fields on the assets you are searching.
            /// If not set, the seach will search in Name and Description.
            /// </summary>
            public readonly ICollection<string> Fields = new List<string>();

            /// <summary>
            /// The string to search for.
            /// </summary>
            public string SearchString { get; set; }
        }

        // Used by the filtering code to know what types of entities to return when applying the filter.
        internal abstract Type EntityType { get; }

        /// <summary>
        /// Add the names of the properties you wish to sort on. The first in the list is the primary
        /// sort, the second is the secondary sort, etc.
        /// </summary>
        public readonly ICollection<string> OrderBy = new List<string>();

        /// <summary>
        /// Specify the fields to search in. Add the names of any string (free text) fields on the assets you are searching.
        /// If not set, the search will search in Name, Description, and ShortName if there is one.
        /// </summary>
        public readonly StringSearcher Find = new StringSearcher();

        /// <summary>
        /// Add the names of the properties you wish to eager load.
        /// </summary>
        public readonly ICollection<string> Selectors = new List<string>();

        /// <summary>
        /// Filtering conditions that restrict creation date of Entity.
        /// </summary>
        public readonly DateSearcher CreateDateUtc = new DateSearcher();

        /// <summary>
        /// Filtering conditions that restrict modification date of Entity.
        /// </summary>
        public readonly DateSearcher ChangeDateUtc = new DateSearcher();

        private string ResolvePropertyName(string propertyName) {
            var prop = EntityType.GetProperty(propertyName);
            
            if(prop != null) {
                var renamed = prop.GetCustomAttributes(typeof (MetaRenamedAttribute), false);

                if(renamed.Length > 0) {
                    return ((MetaRenamedAttribute) renamed[0]).RealName;
                }
            }

            return propertyName;
        }

        internal IFilterTerm BuildFilter(IAssetType assetType, V1Instance instance) {
            var builder = new FilterBuilder(assetType, instance);
            InternalModifyFilter(builder);
            InternalModifyState(builder);
            return builder.Root.HasTerms ? builder.Root : null;
        }

        internal virtual void InternalModifyFilter(FilterBuilder builder) {
            builder.Comparison("CreateDateUTC", CreateDateUtc);
            builder.Comparison("ChangeDateUTC", ChangeDateUtc);
        }

        internal virtual void InternalModifyState(FilterBuilder builder) {}

        internal OrderBy BuildOrderBy(IAssetType assetType, IAttributeDefinition defaultOrderBy) {
            var order = new OrderBy();
            
            if (OrderBy.Count > 0) {
                foreach(var s in OrderBy) {
                    order.MinorSort(assetType.GetAttributeDefinition(ResolvePropertyName(s)), APIClient.OrderBy.Order.Ascending);
                }
            } else {
                if(defaultOrderBy != null) {
                    order.MinorSort(defaultOrderBy, APIClient.OrderBy.Order.Ascending);
                }
            }

            return order;
        }

        internal QueryFind BuildFind(IAssetType assetType) {
            if (!string.IsNullOrEmpty(Find.SearchString)) {
                var attributes = new AttributeSelection();

                if (Find.Fields.Count > 0) {
                    attributes.AddRange(Find.Fields.Select(field => assetType.GetAttributeDefinition(ResolvePropertyName(field))));
                } else {
                    if(assetType.ShortNameAttribute != null) {
                        attributes.Add(assetType.ShortNameAttribute);
                    }

                    if(assetType.NameAttribute != null) {
                        attributes.Add(assetType.NameAttribute);
                    }

                    if(assetType.DescriptionAttribute != null) {
                        attributes.Add(assetType.DescriptionAttribute);
                    }
                }

                return new QueryFind(Find.SearchString, attributes);
            }

            return null;
        }

        internal AttributeSelection BuildSelection(IAssetType assetType) {
            var attributes = new AttributeSelection();
            attributes.AddRange(Selectors.Select(property => assetType.GetAttributeDefinition(ResolvePropertyName(property))));
            return attributes;
        }
    }
}