using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
    /// <summary>
    /// Filter for getting schedules.
    /// </summary>
    public class ScheduleFilter : BaseAssetFilter
    {
        internal override Type EntityType
        {
            get { return typeof(Schedule); }
        }

        /// <summary>
        /// Filter on Projects
        /// </summary>
        public ICollection<Project> Projects = new List<Project>();

        internal override void InternalModifyFilter(FilterBuilder builder)
        {
            base.InternalModifyFilter(builder);

            builder.MultiRelation("ScheduledScopes", Projects);
        }
    }
}
