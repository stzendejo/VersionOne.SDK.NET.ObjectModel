using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Comparison searcher that lets specify complex search constraints.
    /// </summary>
    /// <typeparam name="T">Type of compared attribute.</typeparam>
    public interface IComparisonSearcher<T> {
        /// <summary>
        /// Return optimized term collection.
        /// </summary>
        IDictionary<FilterTerm.Operator, T> Terms { get; }

        /// <summary>
        /// Add new filtering term.
        /// </summary>
        /// <param name="op">Filtering operator.</param>
        /// <param name="value">Value to compare with.</param>
        void AddTerm(FilterTerm.Operator op, T value);

        /// <summary>
        /// A shortcut for setting up range search, from minimum to maximum.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        void Range(T min, T max);

        /// <summary>
        /// Clear internal term collection.
        /// </summary>
        void Clear();
    }
}