using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for Effort Records
    /// </summary>
    public class EffortFilter : EntityFilter {
        internal override Type EntityType {
            get { return typeof (Effort); }
        }

        /// <summary>
        /// Filter on Workitem effort records belong to
        /// </summary>
        public readonly ICollection<Workitem> Workitem = new List<Workitem>();

        /// <summary>
        /// Filter on Project effort records belong to
        /// </summary>
        public readonly ICollection<Project> Project = new List<Project>();

        /// <summary>
        /// Filter on Member effort records belong to
        /// </summary>
        public readonly ICollection<Member> Member = new List<Member>();

        /// <summary>
        /// Filter on Iteration effort records belong to
        /// </summary>
        public readonly ICollection<Iteration> Iteration = new List<Iteration>();

        /// <summary>
        /// Filter on Team effort records belong to
        /// </summary>
        public readonly ICollection<Team> Team = new List<Team>();

        /// <summary> 
        /// Filter on Date range effort records belong to.
        /// </summary>
        public readonly ComparisonSearcher<DateTime> Date = new DateSearcher();

        /// <summary>
        /// Filter on Effort value.
        /// </summary>
        public readonly ComparisonSearcher<double> Value = new ComparisonSearcher<double>(); 

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            builder.Relation("Workitem", Workitem);
            builder.Relation("Scope", Project);
            builder.Relation("Member", Member);
            builder.Relation("Timebox", Iteration);
            builder.Relation("Team", Team);

            builder.Comparison("Date", Date);
            builder.Comparison("Value", Value);
        }
    }
}