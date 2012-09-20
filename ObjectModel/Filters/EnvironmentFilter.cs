using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
    /// <summary>
    /// Filter for getting Environment.
    /// </summary>
    public class EnvironmentFilter : EntityFilter
    {
        internal override Type EntityType {
            get { return typeof(Environment); }
        }

        /// <summary>
        /// Name of this item.
        /// </summary>
        public readonly ICollection<string> Name = new List<string>();

        /// <summary>
        /// Project this item belongs to.
        /// </summary>
        public readonly ICollection<Project> Project = new List<Project>();

        /// <summary>
        /// DisplayID of this item.
        /// </summary>
        public readonly ICollection<string> DisplayID = new List<string>();

        /// <summary>
        /// TestSets of this item.
        /// </summary>
        public readonly ICollection<TestSet> TestSet = new List<TestSet>();

        internal override void InternalModifyFilter(FilterBuilder builder)
        {
            base.InternalModifyFilter(builder);

            builder.Relation("Scope", Project);
            builder.MultiRelation("TestSets", TestSet);            
            builder.Simple("Number", DisplayID);
            builder.Simple("Name", Name);
        }
    }
}
