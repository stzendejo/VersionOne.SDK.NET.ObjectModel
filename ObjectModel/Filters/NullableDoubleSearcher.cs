using System;
using System.Linq;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Specifies a set of filter expressions to compare specified attribute with. To specify &gt;, &lt;, &gt;=, &lt;=, =, != condition FilterTerm.Operator is used.
    /// Use Exists and NotExists operators to obtain item with provided or blank estimates, correspondingly.
    /// </summary>
    public class NullableDoubleSearcher : IComparisonSearcher<double?> {
        private readonly ComparisonSearcher<double> searcher = new ComparisonSearcher<double>();
        private FilterTerm.Operator existsOperator = FilterTerm.Operator.None; 

        /// <summary>
        /// Return optimized term collection.
        /// </summary>
        public IDictionary<FilterTerm.Operator, double?> Terms {
            get {
                var terms = searcher.Terms.ToDictionary(x => x.Key, y => new double?(y.Value));
                
                if(existsOperator != FilterTerm.Operator.None) {
                    ValidateExistsOperator(existsOperator);
                    terms.Add(existsOperator, null);
                }

                return terms;
            }
        }

        /// <summary>
        /// Add new filtering term. Supplied terms are AND'ed.
        /// </summary>
        /// <param name="op">Filtering operator. In case of null value it can only be either Exists or NotExists.</param>
        /// <param name="value">Value to compare with.</param>
        public void AddTerm(FilterTerm.Operator op, double? value = null) {
            if(value.HasValue) {
                searcher.AddTerm(op, value.Value);
                return;
            }

            ValidateExistsOperator(op);
            existsOperator = op;
        }

        /// <summary>
        /// A shortcut for setting up range search, from minimum to maximum.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        public void Range(double? min, double? max) {
            if(!min.HasValue || !max.HasValue) {
                throw new InvalidOperationException("Cannot set either range bound to NULL");
            }

            if(min > max) {
                throw new InvalidOperationException("Lower bound of a range cannot exceed upper bound");
            }

            AddTerm(FilterTerm.Operator.GreaterThanOrEqual, min);
            AddTerm(FilterTerm.Operator.LessThanOrEqual, max);
        }

        /// <summary>
        /// Clear internal term collection.
        /// </summary>
        public void Clear() {
            searcher.Terms.Clear();
            existsOperator = FilterTerm.Operator.None;
        }

        private static void ValidateExistsOperator(FilterTerm.Operator op) {
            if(op != FilterTerm.Operator.Exists && op != FilterTerm.Operator.NotExists) {
                throw new InvalidOperationException(string.Format("You can only apply {0} or {1} operators to numbers having no value", FilterTerm.Operator.Exists, FilterTerm.Operator.NotExists));                
            }
        }
    }
}