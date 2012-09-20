using System;
using System.Collections.Generic;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Specifies a set of filter expressions to compare specified attribute with. To specify &gt;, &lt;, &gt;=, &lt;=, =, != condition
    /// FilterTerm.Operator is used.
    /// </summary>
    /// <typeparam name="T">Type of argument to compare.</typeparam>
    public class ComparisonSearcher<T> : IComparisonSearcher<T> where T : IComparable<T> {
        private readonly Dictionary<FilterTerm.Operator, T> terms = new Dictionary<FilterTerm.Operator, T>();

        /// <summary>
        /// Return optimized term collection.
        /// </summary>
        public IDictionary<FilterTerm.Operator, T> Terms {
            get {
                Optimize();
                return terms;
            }
        }

        /// <summary>
        /// Add new filtering term.
        /// </summary>
        /// <param name="op">Filtering operator.</param>
        /// <param name="value">Value to compare with.</param>
        public virtual void AddTerm(FilterTerm.Operator op, T value) {
            T old;
            var operatorNotExists = !terms.TryGetValue(op, out old);

            switch (op) {
                case FilterTerm.Operator.Exists:
                    if (terms.ContainsKey(FilterTerm.Operator.NotExists)) {
                        throw new InvalidOperationException("Cannot contain both " + op + " and " + FilterTerm.Operator.NotExists + " terms.");
                    }

                    if (operatorNotExists) {
                        terms.Add(op, value);
                    }

                    break;
                case FilterTerm.Operator.NotExists:
                    if (terms.ContainsKey(FilterTerm.Operator.Exists)) {
                        throw new InvalidOperationException("Cannot contain both " + op + " and " + FilterTerm.Operator.Exists + " terms.");
                    }

                    if (operatorNotExists) {
                        terms.Add(op, value);
                    }

                    break;
                case FilterTerm.Operator.Equal:
                case FilterTerm.Operator.NotEqual:
                    if (operatorNotExists) {
                        terms.Add(op, value);
                    } else {
                        throw new InvalidOperationException("Cannot contain more than one " + op + " term.");
                    }

                    break;
                case FilterTerm.Operator.GreaterThan:
                case FilterTerm.Operator.GreaterThanOrEqual:
                    if (operatorNotExists) {
                        terms.Add(op, value);
                    } else if (old.CompareTo(value) < 0) {
                        // new > old
                        terms[op] = value;
                    }

                    break;
                case FilterTerm.Operator.LessThan:
                case FilterTerm.Operator.LessThanOrEqual:
                    if (operatorNotExists) {
                        terms.Add(op, value);
                    } else if (old.CompareTo(value) > 0) {
                        // new < old
                        terms[op] = value;
                    }
                    break;
            }
        }

        /// <summary>
        /// A shortcut for setting up range search, from minimum to maximum.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        public void Range(T min, T max) {
            if(min.CompareTo(max) > 0) {
                throw new InvalidOperationException("Lower bound of a range cannot exceed upper bound");
            }

            AddTerm(FilterTerm.Operator.GreaterThanOrEqual, min);
            AddTerm(FilterTerm.Operator.LessThanOrEqual, max);
        }

        /// <summary>
        /// Clear internal term collection.
        /// </summary>
        public void Clear() {
            terms.Clear();
        }

        private void Optimize() {
            T less;
            T lessOrEqual;

            if (terms.TryGetValue(FilterTerm.Operator.LessThan, out less) &&
                terms.TryGetValue(FilterTerm.Operator.LessThanOrEqual, out lessOrEqual)) {
                terms.Remove(lessOrEqual.CompareTo(less) >= 0
                                 ? FilterTerm.Operator.LessThanOrEqual
                                 : FilterTerm.Operator.LessThan);
            }

            T greater;
            T greaterOrEqual;

            if (terms.TryGetValue(FilterTerm.Operator.GreaterThan, out greater) &&
                terms.TryGetValue(FilterTerm.Operator.GreaterThanOrEqual, out greaterOrEqual)) {
                terms.Remove(greaterOrEqual.CompareTo(greater) <= 0
                                 ? FilterTerm.Operator.GreaterThanOrEqual
                                 : FilterTerm.Operator.GreaterThan);
            }
        }
    }
}