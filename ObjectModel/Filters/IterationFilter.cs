using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters
{
	/// <summary>
	/// State of the Iteration.
	/// </summary>
	public enum IterationState
	{
		/// <summary>
		/// Not active yet.
		/// </summary>
		Future,

		/// <summary>
		/// Active or open.
		/// </summary>
		Active,

		/// <summary>
		/// Closed or inactive.
		/// </summary>
		Closed
	}

	/// <summary>
	/// Filter iterations.
	/// </summary>
    public class IterationFilter : BaseAssetFilter
	{
        internal override System.Type EntityType
        {
            get { return typeof(Iteration); }
        }

		/// <summary>
		/// State of the Iteration (Future, Active, or Closed)
		/// </summary>
		public new ICollection<IterationState> State = new List<IterationState>();

        /// <summary>
        /// Schedules this item belongs to.
        /// </summary>
        public readonly ICollection<Schedule> Schedule = new List<Schedule>();

		internal override bool HasState { get { return State.Count != 0; } }
		internal override bool HasActive { get { return State.Contains(IterationState.Active); } }
		internal override bool HasClosed { get { return State.Contains(IterationState.Closed); } }
		internal bool HasFuture { get { return State.Contains(IterationState.Future); } }

		internal override void InternalModifyState(FilterBuilder builder)
		{
			List<string> states = new List<string>();
			if (HasFuture)
				states.Add("Future");
			if (HasActive)
				states.Add("Active");
			if (HasClosed)
				states.Add("Closed");

			if (states.Count > 0 && states.Count < 3)
				builder.Root.And(new TokenTerm("AssetState=" + TextBuilder.Join(states, ",", delegate(string s) { return "'" + s + "'"; })));
			else
				builder.Root.And(new TokenTerm("AssetState!='Dead'"));
		}

        internal override void InternalModifyFilter(FilterBuilder builder)
        {
            base.InternalModifyFilter(builder);

            builder.Relation("Schedule", Schedule);
        }
	}
}
