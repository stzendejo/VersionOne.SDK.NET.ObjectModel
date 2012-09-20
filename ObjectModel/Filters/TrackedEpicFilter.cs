using System.Collections.Generic;
using System.Linq;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel.Filters {
    /// <summary>
    /// Filter for getting tracked Epics for a set of projects.
    /// NOTE that this filter is deliberately separated from EpicFilter as it may cause collisions.
    /// </summary>
    public class TrackedEpicFilter : BaseAssetFilter {
        private readonly List<Project> projects = new List<Project>();

        internal override System.Type EntityType {
            get { return typeof (Epic); }
        }

        /// <summary>
        /// Create filter to obtain tracked Epics for enlisted projects.
        /// </summary>
        /// <param name="projects"></param>
        public TrackedEpicFilter(IEnumerable<Project> projects) {
            if(projects != null) {
                this.projects.AddRange(projects.ToList());
            }
        }

        internal override void InternalModifyFilter(FilterBuilder builder) {
            base.InternalModifyFilter(builder);

            if(projects.Count == 1) {
                CreateSingleScopeFilter(builder);
            } else {
                CreateMultipleScopeFilter(builder);
            }
        }

        private void CreateMultipleScopeFilter(FilterBuilder builder) {
            builder.Relation("Scope", projects);

            var metaModel = builder.Instance.ApiClient.MetaModel;
            var epicType = metaModel.GetAssetType("Epic");

            var scopeAttribute = epicType.GetAttributeDefinition("Scope");
            var scopeTerm = new FilterTerm(scopeAttribute);
            scopeTerm.Operate(FilterTerm.Operator.Equal, projects.Select(x => x.ToString()).Cast<object>().ToArray());
            var superAndUpAttribute = epicType.GetAttributeDefinition("SuperAndUp").Filter(scopeTerm);

            var superAndUpTerm = builder.Root.Term(superAndUpAttribute);
            superAndUpTerm.NotExists();
        }

        private void CreateSingleScopeFilter(FilterBuilder builder) {
            var project = projects.First();

            var metaModel = builder.Instance.ApiClient.MetaModel;
            var epicType = metaModel.GetAssetType("Epic");
            var scopeType = metaModel.GetAssetType("Scope");

            var notClosedScopeAttribute = scopeType.GetAttributeDefinition("AssetState");
            var notClosedScopeTerm = new FilterTerm(notClosedScopeAttribute);
            notClosedScopeTerm.NotEqual("Closed");
            var scopeAttribute = epicType.GetAttributeDefinition("Scope.ParentMeAndUp").Filter(notClosedScopeTerm);
            var scopeTerm = builder.Root.Term(scopeAttribute);
            scopeTerm.Equal(project.ToString());

            var superAndUpAttribute = epicType.GetAttributeDefinition("SuperAndUp");
            superAndUpAttribute = superAndUpAttribute.Filter(scopeTerm);
            var superAndUpTerm = builder.Root.Term(superAndUpAttribute);
            superAndUpTerm.NotExists();
        }

        //TODO investigate if this method redundant and filter can work using base class implementation
        internal override void InternalModifyState(FilterBuilder builder) {
            if(HasState) {
                builder.Root.And(HasActive ? new TokenTerm("AssetState='Active';AssetType='Epic'") : new TokenTerm("AssetState='Closed';AssetType='Epic'"));
            } else {
                builder.Root.And(new TokenTerm("AssetState!='Dead';AssetType='Epic'"));
            }
        }
    }
}