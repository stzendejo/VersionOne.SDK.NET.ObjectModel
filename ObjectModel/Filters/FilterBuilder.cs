using System.Collections.Generic;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel.Filters {
    internal class FilterBuilder {
        private readonly IAssetType assetType;
        public readonly V1Instance Instance;

        public FilterBuilder(IAssetType assetType, V1Instance instance) {
            this.assetType = assetType;
            Instance = instance;
        }

        public readonly GroupFilterTerm Root = new AndFilterTerm();

        private IAttributeDefinition Resolve(string name) {
            return assetType.GetAttributeDefinition(name);
        }

        private Oid GetOid(AssetID id) {
            return Instance.ApiClient.Services.GetOid(id);
        }

        public void Simple<T>(string name, T value) {
            Root.Term(Resolve(name)).Equal(new object[] {value});
        }

        public void Simple<T>(string name, ICollection<T> values) {
            if(values.Count > 0) {
                Root.Term(Resolve(name)).Equal(new CastTransformEnumerable<object>(values).ToArray());
            }
        }

        public void Relation<T>(string name, ICollection<T> values) where T : Entity {
            if(values.Count > 0) {
                Root.Term(Resolve(name)).Equal(new TransformEnumerable<T, Oid>(values, EntityToOid).ToArray());
            }
        }

        public void MultiRelation<T>(string name, ICollection<T> values) where T : Entity {
            if (values.Count > 0) {
                var def = Resolve(name);
                FilterTerm valuesTerm = null;
                FilterTerm notExistsTerm = null;
                
                foreach (var t in values) {
                    if (t == null) {
                        if (notExistsTerm == null) {
                            notExistsTerm = new FilterTerm(def);
                            notExistsTerm.NotExists();
                        }
                    } else {
                        if(valuesTerm == null) {
                            valuesTerm = new FilterTerm(def);
                        }

                        valuesTerm.Equal(GetOid(t.ID));
                    }
                }

                Root.Or(valuesTerm, notExistsTerm);
            }
        }

        public void ListRelation<T>(string name, ICollection<string> values) where T : ListValue {
            if(values.Count > 0) {
                Root.Term(Resolve(name)).Equal(new TransformEnumerable<string, Oid>(values, ListToOid<T>).ToArray());
            }
        }

        public void Comparison<T>(string name, FilterTerm.Operator @operator, T value) {
            var term = Root.Term(Resolve(name));

            switch(@operator) {
                case FilterTerm.Operator.Exists:
                    term.Exists();
                    break;
                case FilterTerm.Operator.NotExists:
                    term.NotExists();
                    break;
                default:
                    term.Operate(@operator, value);
                    break;
            }
        }

        public void Comparison<T>(string attributeName, IComparisonSearcher<T> searcher) {
            foreach (var entry in searcher.Terms) {
                Comparison(attributeName, entry.Key, entry.Value);
            }
        }

        private Oid EntityToOid<T>(T e) where T : Entity {
            return e == null ? Oid.Null : GetOid(e.ID);
        }

        private Oid ListToOid<T>(string s) where T : ListValue {
            return s == null ? Oid.Null : GetOid(Instance.GetListValueByName<T>(s).ID);
        }
    }
}