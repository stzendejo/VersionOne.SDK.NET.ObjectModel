using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for getting themes.
    /// </summary>
    public class ThemeFilter : ProjectAssetFilter {
        internal override Type EntityType {
            get { return typeof(Theme); }
        }

        /// <summary>
        /// Parent theme's
        /// </summary>
        public readonly ICollection<Theme> Parent = new List<Theme>();

        /// <summary>
        /// Filter on Status.
        /// </summary>
        public readonly ICollection<string> Status = new List<string>();

        /// <summary>
        /// Filter on Source.
        /// </summary>
        public readonly ICollection<string> Source = new List<string>();

        /// <summary>
        /// Filter on Theme assigned.
        /// </summary>
        public readonly ICollection<Theme> Theme = new List<Theme>();

        /// <summary>
        /// Filter on Customer.
        /// </summary>
        public readonly ICollection<Member> Customer = new List<Member>();

        /// <summary>
        /// Filter on Risk.
        /// </summary>
        public readonly ICollection<string> Risk = new List<string>();

        /// <summary>
        /// Filter on Type.
        /// </summary>
        public readonly ICollection<string> Type = new List<string>();

        /// <summary>
        /// Filter on Priority.
        /// </summary>
        public readonly ICollection<string> Priority = new List<string>();

        /// <summary>
        /// Filter on Goals.
        /// </summary>
        public readonly ICollection<Goal> Goals = new List<Goal>();

        /// <summary>
        /// Filter on Owners.
        /// </summary>
        public readonly ICollection<Member> Owners = new List<Member>();

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Relation("Parent", Parent);
            builder.Relation("Customer", Customer);
            builder.Relation("Theme", Theme);

            builder.MultiRelation("Goals", Goals);
            builder.MultiRelation("Owners", Owners);

            builder.ListRelation<ThemeStatus>("Status", Status);
            builder.ListRelation<ThemeSource>("Source", Source);
            builder.ListRelation<WorkitemRisk>("Risk", Risk);
            builder.ListRelation<ThemeType>("Category", Type);
            builder.ListRelation<WorkitemPriority>("Priority", Priority);
        }
    }
}