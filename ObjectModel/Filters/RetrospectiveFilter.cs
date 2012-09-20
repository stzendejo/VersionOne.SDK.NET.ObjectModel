using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// A filter for Retrospectives
	/// </summary>
	public class RetrospectiveFilter : ProjectAssetFilter
	{
        internal override Type EntityType
        {
            get { return typeof(Retrospective); }
        }

		/// <summary>
		/// The Iteration the Retrospective was conducted for
		/// </summary>
		public readonly ICollection<Iteration> Iteration = new List<Iteration>();
		/// <summary>
		/// The Retrospective Facilitator
		/// </summary>
		public readonly ICollection<Member> FacilitatedBy = new List<Member>();
		/// <summary>
		/// The Team the Retrospecive belongs to
		/// </summary>
		public readonly ICollection<Team> Team = new List<Team>();

		/// <summary>
		/// The date this Retrospective was conducted.  Typically stored as DateTime.Date (rounded to the day).
		/// </summary>
		public readonly ICollection<DateTime?> Date = new List<DateTime?>();

		internal override void InternalModifyFilter(FilterBuilder builder)
		{
			base.InternalModifyFilter(builder);

			builder.Simple("Date",Date);

			builder.Relation("Timebox", Iteration);
			builder.Relation("FacilitatedBy", FacilitatedBy);
			builder.Relation("Team", Team);
		}
	}
}