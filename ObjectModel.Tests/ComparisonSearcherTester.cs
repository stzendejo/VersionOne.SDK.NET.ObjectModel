using System;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel.Tests {
    [TestFixture]
    public class ComparisonSearcherTester {
        [Test]
        public void Optimize() {
            var date1 = DateTime.Now;
            var date2 = date1.AddSeconds(2);

            // date1 < date2
            var searcher = new ComparisonSearcher<DateTime>();
            searcher.AddTerm(FilterTerm.Operator.LessThan, date1);
            searcher.AddTerm(FilterTerm.Operator.LessThanOrEqual, date2);
            Assert.AreEqual(1, searcher.Terms.Count);
            var enumerator = searcher.Terms.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(FilterTerm.Operator.LessThan, enumerator.Current.Key);
            Assert.AreEqual(date1, enumerator.Current.Value);

            // date1 < date2
            searcher.Clear();
            searcher.AddTerm(FilterTerm.Operator.LessThanOrEqual, date1);
            searcher.AddTerm(FilterTerm.Operator.LessThan, date2);
            Assert.AreEqual(1, searcher.Terms.Count);
            enumerator = searcher.Terms.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(FilterTerm.Operator.LessThanOrEqual, enumerator.Current.Key);
            Assert.AreEqual(date1, enumerator.Current.Value);

            // date1 > date2
            searcher.Clear();
            searcher.AddTerm(FilterTerm.Operator.GreaterThan, date1);
            searcher.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, date2);
            Assert.AreEqual(1, searcher.Terms.Count);
            enumerator = searcher.Terms.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(FilterTerm.Operator.GreaterThanOrEqual, enumerator.Current.Key);
            Assert.AreEqual(date2, enumerator.Current.Value);

            // date1 > date2
            searcher.Clear();
            searcher.AddTerm(FilterTerm.Operator.GreaterThanOrEqual, date1);
            searcher.AddTerm(FilterTerm.Operator.GreaterThan, date2);
            Assert.AreEqual(1, searcher.Terms.Count);
            enumerator = searcher.Terms.GetEnumerator();
            enumerator.MoveNext();
            Assert.AreEqual(FilterTerm.Operator.GreaterThan, enumerator.Current.Key);
            Assert.AreEqual(date2, enumerator.Current.Value);
        }
    }
}