using System;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// When SDK user adds term:
    /// date&gt;YYYY-MM-DD HH:MM:SS.TTT we build query: date&gt;=YYYY-MM-DD HH:MM:(SS+1)
    /// date&gt;=YYYY-MM-DD HH:MM:SS.TTT we build query: date&gt;=YYYY-MM-DD HH:MM:SS
    /// date&lt;YYYY-MM-DD HH:MM:SS.TTT we build query: date&lt;YYYY-MM-DD HH:MM:SS
    /// date&lt;=YYYY-MM-DD HH:MM:SS.TTT we build query: date&lt;YYYY-MM-DD HH:MM:(SS+1)
    /// date=YYYY-MM-DD HH:MM:SS.TTT we build query: date&gt;=YYYY-MM-DD HH:MM:SS &amp; date&lt;YYYY-MM-DD HH:MM:(SS+1)
    /// </summary>
    public class DateSearcher : ComparisonSearcher<DateTime> {
        /// <summary>
        /// Adds comparison term. Only specified Operators supported:
        /// <ul>
        /// <li>GreaterThan</li>
        /// <li>GreaterThanOrEqual</li>
        /// <li>LessThan</li>
        /// <li>LessThanOrEqual</li>
        /// <li>Equal</li>
        /// </ul>
        /// </summary>
        /// <param name="op">Operator to apply.</param>
        /// <param name="date">Value to compare with.</param>
        public override void AddTerm(FilterTerm.Operator op, DateTime date) {
            DateTime roundedDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

            switch (op) {
                case FilterTerm.Operator.GreaterThan:
                    base.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, roundedDate.AddSeconds(1));
                    break;
                case FilterTerm.Operator.GreaterThanOrEqual:
                    base.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, roundedDate);
                    break;
                case FilterTerm.Operator.LessThan:
                    base.AddTerm(FilterTerm.Operator.LessThan, roundedDate);
                    break;
                case FilterTerm.Operator.LessThanOrEqual:
                    base.AddTerm(FilterTerm.Operator.LessThan, roundedDate.AddSeconds(1));
                    break;
                case FilterTerm.Operator.Equal:
                    base.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, roundedDate);
                    base.AddTerm(FilterTerm.Operator.LessThan, roundedDate.AddSeconds(1));
                    break;
                default:
                    throw new NotSupportedException("This operation is not supported: " + op);
            }
        }
    }
}
