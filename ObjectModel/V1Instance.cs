using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;
using Attribute = VersionOne.SDK.APIClient.Attribute;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Provides access to a single instance of a VersionOne application. This is the starting point 
    /// for any application.
    /// </summary>
    public partial class V1Instance {
        private bool validationEnabled;
        private readonly WrapperManager wrapperManager;
        private ApiClientInternals apiClientInternals;
        private InstanceConfiguration configuration;
        private readonly IDictionary<object, Asset> assetCache = new Dictionary<object, Asset>();

        /// <summary>
        /// Initialize a V1Instance to communicate with an installation of VersionOne at a given URL using Integrated Authentication.
        /// </summary>
        /// <param name="applicationPath">Location of the VersionOne application. Ex: http://server/versionone/</param>
        public V1Instance(string applicationPath) : this(applicationPath, null, null, true) { }

        /// <summary>
        /// Initialize a V1Instance to communicate with an installation of VersionOne at a given URL using basic authentication.
        /// </summary>
        /// <param name="applicationPath">Location of the VersionOne application. Ex: http://server/versionone/</param>
        /// <param name="username">VersionOne member's username to authenticate with</param>
        /// <param name="password">VersioonOne member's password to authenticate with</param>
        public V1Instance(string applicationPath, string username, string password) : this(applicationPath, username, password, false) {}

        /// <summary>
        /// Initialize a V1Instance to communicate with an installation of VersionOne at a given URL.
        /// </summary>
        /// <param name="applicationPath">Location of the VersionOne application. Ex: http://server/versionone/</param>
        /// <param name="username">VersionOne member's username to authenticate with.  If using Integrated Authentication, must be in "user@domain" format.</param>
        /// <param name="password">VersioonOne member's password to authenticate with</param>
        /// <param name="integratedAuth">True to use Integrated Authentication</param>
        public V1Instance(string applicationPath, string username, string password, bool integratedAuth) {
            if(!applicationPath.EndsWith("/")) {
                applicationPath += "/";
            }

            apiClientInternals = new ApiClientInternals(applicationPath, username, password, integratedAuth, null);
            wrapperManager = new WrapperManager(this);
        }

        /// <summary>
        /// Initialize a V1Instance to communicate with an installation of VersionOne through a Proxy connection
        /// </summary>
        /// <param name="applicationPath">Location of the VersionOne application. Ex: http://server/versionone/</param>
        /// <param name="username">VersionOne member's username to authenticate with.  If using Integrated Authentication, must be in "user@domain" format.</param>
        /// <param name="password">VersioonOne member's password to authenticate with</param>
        /// <param name="integratedAuth">True to use Integrated Authentication</param>
        /// <param name="proxySettings">Proxy server settings. Here you can provide proxy path and credentials, if required.</param>
        public V1Instance(string applicationPath, string username, string password, bool integratedAuth, ProxySettings proxySettings)
            : this(applicationPath, username, password, integratedAuth) {
            ProxyProvider proxyProvider = null;

            if (proxySettings != null) {
                proxyProvider = new ProxyProvider(proxySettings.Path, proxySettings.Username, proxySettings.Password, proxySettings.Domain);
            }

            apiClientInternals = new ApiClientInternals(applicationPath, username, password, integratedAuth, proxyProvider);
        }

        /// <summary>
        /// Validate the application path, username, and password used to construct this instance
        /// </summary>
        /// <exception cref="ApplicationUnavailableException">The application path is unavailable or invalid.</exception>
        /// <exception cref="AuthenticationException">The supplied username or password was invalid.</exception>
        public void Validate() {
            apiClientInternals.Validate();
        }

        /// <summary>
        /// The underlying MetaModel provided by the API Client.
        /// </summary>
        private IMetaModel MetaModel {
            get { return apiClientInternals.MetaModel; }
        }

        /// <summary>
        /// The underlying localizer provided by the API Client.
        /// </summary>
        private ILocalizer Loc {
            get { return apiClientInternals.Localizer; }
        }

        /// <summary>
        /// The underlying Services provided by the API Client.
        /// </summary>
        private IServices Services {
            get { return apiClientInternals.Services; }
        }

        /// <summary>
        /// The underlying Attachments provided by the API Client.
        /// </summary>
        private IAttachments Attachments {
            get { return apiClientInternals.Attachments; }
        }

        /// <summary>
        /// The underlying V1Config provided by the API Client.
        /// </summary>
        private IV1Configuration V1Config {
            get { return apiClientInternals.V1Config; }
        }

        /// <summary>
        /// Contains configuration settings for the VersionOne application instance
        /// </summary>
        public InstanceConfiguration Configuration {
            get {
                return configuration ?? (configuration = new InstanceConfiguration(V1Config.EffortTracking,
                                                                                   (TrackingLevel) V1Config.StoryTrackingLevel,
                                                                                   (TrackingLevel) V1Config.DefectTrackingLevel,
                                                                                   V1Config.MaxAttachmentSize));
            }
        }

        /// <summary>
        /// Returns the currently logged in member
        /// </summary>
        public Member LoggedInMember {
            get { return Get.MemberByID(Services.LoggedIn.Token); }
        }

        /// <summary>
        /// Returns a read-only collection of root level Project entities
        /// </summary>
        public ICollection<Project> Projects {
            get {
                var projectAssetType = MetaModel.GetAssetType("Scope");
                var query = new Query(projectAssetType, projectAssetType.GetAttributeDefinition("Parent"));
                var assetStateTerm = new FilterTerm(projectAssetType.GetAttributeDefinition("AssetState"));
                assetStateTerm.NotEqual(AssetState.Closed);
                query.Filter = new AndFilterTerm(assetStateTerm);
                query.OrderBy.MajorSort(projectAssetType.GetAttributeDefinition("Name"), OrderBy.Order.Ascending);

                return QueryToEntityEnum<Project>(query);
            }
        }

        /// <summary>
        /// Returns a read-only collection of enumerable of Member entities
        /// </summary>
        public ICollection<Member> Members {
            get {
                var memberAssetType = MetaModel.GetAssetType("Member");
                var query = new Query(memberAssetType);
                var assetStateTerm = new FilterTerm(memberAssetType.GetAttributeDefinition("AssetState"));
                assetStateTerm.NotEqual(AssetState.Closed);
                query.Filter = new AndFilterTerm(assetStateTerm);
                query.OrderBy.MajorSort(memberAssetType.GetAttributeDefinition("Name"), OrderBy.Order.Ascending);

                return QueryToEntityEnum<Member>(query);
            }
        }

        /// <summary>
        /// Returns a read-only collection of Team entities
        /// </summary>
        public ICollection<Team> Teams {
            get {
                var teamAssetType = MetaModel.GetAssetType("Team");
                var query = new Query(teamAssetType);
                var assetStateTerm = new FilterTerm(teamAssetType.GetAttributeDefinition("AssetState"));
                assetStateTerm.NotEqual(AssetState.Closed);
                query.Filter = new AndFilterTerm(assetStateTerm);
                query.OrderBy.MajorSort(teamAssetType.GetAttributeDefinition("Name"), OrderBy.Order.Ascending);

                return QueryToEntityEnum<Team>(query);
            }
        }

        /// <summary>
        /// Returns a read-only collection of TestSuites
        /// </summary>
        public ICollection<TestSuite> TestSuites {
            get {
                var teamAssetType = MetaModel.GetAssetType("TestSuite");
                var query = new Query(teamAssetType);
                var assetStateTerm = new FilterTerm(teamAssetType.GetAttributeDefinition("AssetState"));
                assetStateTerm.NotEqual(AssetState.Closed);
                query.Filter = new AndFilterTerm(assetStateTerm);
                query.OrderBy.MajorSort(teamAssetType.GetAttributeDefinition("Name"), OrderBy.Order.Ascending);

                return QueryToEntityEnum<TestSuite>(query);
            }
        }

        /// <summary>
        /// Allows access to the underlying API Client structures. Only use this when you need access that
        /// this library does not provide.
        /// </summary>
        public ApiClientInternals ApiClient {
            get { return apiClientInternals; }
            set { apiClientInternals = value; }
        }

        /// <summary>
        /// Headers from this Dictionary will be added to all HTTP requests to VersionOne server.
        /// </summary>
        public IDictionary<string, string> CustomHttpHeaders {
            get { return apiClientInternals.CustomHttpHeaders; }
        }

        /// <summary>
        /// Get/set validation state. If enabled, required fields are validated on entity creation, save and operation execution.
        /// If entities are invalid, <see cref="EntityValidationException" /> is thrown.
        /// </summary>
        public bool ValidationEnabled {
            get { return validationEnabled; }
            set { validationEnabled = value; }
        }


        #region Helper Enum Conversion

        private static AttributeSelection FlattenSelection(IAssetType querytype, IEnumerable<IAttributeDefinition> orig) {
            var selection = new AttributeSelection();

            foreach (var def in orig) {
                IAttributeDefinition newdef = null;

                if (def.AssetType.Is(querytype)) {
                    if(def.AssetType == querytype || def.Base == null) {
                        newdef = def;
                    } else if(def.Base.AssetType.Is(querytype)) {
                        newdef = def.Base;
                    } else {
                        newdef = querytype.GetAttributeDefinition(def.Name);
                    }
                } else if(querytype.Is(def.AssetType)) {
                    newdef = querytype.GetAttributeDefinition(def.Name);
                }

                if(newdef != null && !selection.Contains(newdef)) {
                    selection.Add(newdef);
                }
            }

            return selection;
        }

        private static IEnumerable<IAttributeDefinition> GetSuggestedSelection(IAssetType assetType, Type type) {
            var result = new AttributeSelection();

            for (var t = type; t != null; t = t.BaseType) {
                var attributes = t.GetCustomAttributes(typeof (MetaDataAttribute), false);

                foreach (MetaDataAttribute attrib in attributes) {
                    var names = attrib.DefaultAttributeSelectionNames;

                    if(!string.IsNullOrEmpty(names)) {
                        foreach (var name in names.Split(',')) {
                            IAttributeDefinition def;
                            
                            if(assetType.TryGetAttributeDefinition(name, out def)) {
                                if (!result.Contains(def)) {
                                    result.Add(def);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private ICollection<T> QueryToEntityEnum<T>(Query query) where T : Entity {
            var assetStateSelection = new AttributeSelection();
            IAttributeDefinition assetStateDef;
            
            if (query.AssetType.TryGetAttributeDefinition("AssetState", out assetStateDef)) {
                assetStateSelection.Add(assetStateDef);
            }
            
            var suggestedSelection = GetSuggestedSelection(query.AssetType, typeof (T));
            var flattenedSelection = FlattenSelection(query.AssetType, query.Selection);
            query.Selection = AttributeSelection.Merge(assetStateSelection, suggestedSelection, flattenedSelection);

            return AssetEnumToEntityEnum<T>(Services.Retrieve(query).Assets);
        }

        private ICollection<T> AssetEnumToEntityEnum<T>(IEnumerable<Asset> assets) where T : Entity {
            var members = new List<T>();
            
            foreach (var asset in assets) {
                var id = new AssetID(asset.Oid.Token);
                SetAsset(id, asset);
                var wrapped = CreateWrapper<T>(id, false);
                
                //we could get an asset from a query that has no SDK wrapper, skip it here
                if(wrapped != null) {
                    members.Add(wrapped);
                }
            }

            return members.AsReadOnly();
        }

        private ICollection<T> OidEnumToEntityCollection<T>(IEnumerable<Oid> oids) where T : Entity {
            var members = oids.Select(oid => new AssetID(oid.Token)).Select(id => CreateWrapper<T>(id, false)).Where(t => t != null).ToList();
            return members.AsReadOnly();
        }

        #endregion

        internal RequiredFieldValidator GetRequiredFieldValidator() {
            return new RequiredFieldValidator(MetaModel, Services);
        }

        internal AssetID Commit(Entity entity, string comment) {
            try {
                var asset = GetAsset(entity);
                ValidateEntity(entity, asset);

                Services.Save(asset, comment);
                asset.Oid = asset.Oid.Momentless; // We cache this asset, and we don't want moments in the cache
                var assetId = new AssetID(asset.Oid.Token);
                SetAsset(assetId, asset);
                return assetId;
            } catch (APIException ex) {
                throw new DataException(Loc.Resolve(ex.Message));
            }
        }

        private void ValidateEntity(Entity entity, Asset correspondingAsset) {
            if (!validationEnabled) {
                return;
            }

            var validator = new EntityValidator(this);
            var invalidAttributes = validator.Validate(correspondingAsset);

            if (invalidAttributes.Count > 0) {
                throw new EntityValidationException(entity, invalidAttributes);
            }
        }

        internal T New<T>(Entity inTheContextOf) where T : Entity {
            var contextOid = GetOid(inTheContextOf);

            var assetTypeToken = GetAssetTypeToken(typeof (T));
            var typeToCreate = MetaModel.GetAssetType(assetTypeToken);

            var shell = Services.New(typeToCreate, contextOid);

            var result = CreateWrapper<T>();
            SetAsset(result.InstanceKey, shell);

            return result;
        }

        internal string GetEntityURL(Entity entity) {
            if(entity is TestSuite) {
                return null;
            }

            if(entity is BaseAsset) {
                return apiClientInternals.ApplicationPath + "assetdetail.v1/?oid=" + entity.ID;
            }

            if(entity is Attachment) {
                return apiClientInternals.ApplicationPath + "assetdetail.v1/?oid=" + entity.ID;
            }

            return null;
        }

        internal string GetAttachmentURL(Attachment attachment) {
            return apiClientInternals.ApplicationPath + "attachment.v1/" + GetOid(attachment).Key;
        }

        #region Asset Operation-Related Members

        internal bool CanExecuteOperation(Entity subject, string operationName) {
            var asset = GetAsset(subject);
            var toExecute = asset.AssetType.GetOperation(operationName);
            return (bool) RetrieveAttribute(asset.Oid, toExecute.ValidatorAttribute, false).Value;
        }

        /// <summary>
        /// Executes an Operation on an Entity, assuming it is safe to do so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="operationName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the Operation fails</exception>
        internal T ExecuteOperation<T>(Entity subject, string operationName) where T : Entity {
            var operationResult = ExecuteOperation(subject, operationName);
            var id = new AssetID(operationResult.Token);
            return CreateWrapper<T>(id, false);
        }

        /// Be sure to call Entity.Save() before or after calling this depending on the operation result
        /// <exception cref="InvalidOperationException">If the Operation fails</exception>
        internal Oid ExecuteOperation(Entity subject, string operationName) {
            var asset = GetAsset(subject);
            return ExecuteOperation(asset, operationName);
        }

        private Oid ExecuteOperation(Asset subject, string operationName) {
            try {
                var toExecute = subject.AssetType.GetOperation(operationName);
                var result = Services.ExecuteOperation(toExecute, subject.Oid);
                return result.Momentless;
            } catch (APIException ex) {
                throw new InvalidOperationException("Cannot execute operation: " + operationName, ex);
            }
        }

        #endregion

        #region Asset Cache

        private Oid GetOid(Entity entity) {
            if(entity == null) {
                return Oid.Null;
            }

            var contextAsset = GetAsset(entity);
            return contextAsset.Oid;
        }

        private Asset GetAsset(Oid oid) {
            return GetAsset(new AssetID(oid.Token), oid.AssetType.Token);
        }

        /// <summary>
        /// Find an asset in the asset cache or create one for this entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>An asset that will exist in the asset cache</returns>
        private Asset GetAsset(Entity entity) {
            return GetAsset(entity.InstanceKey, GetAssetTypeToken(entity.GetType()));
        }

        /// <summary>
        /// Find an asset in the asset cache or create one for this id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetTypeToken">The Asset Type Token of the asset to create if one does not already exist</param>
        /// <returns>An Asset that will exist in the asset cache</returns>
        private Asset GetAsset(object id, string assetTypeToken) {
            Asset result;

            if (!assetCache.TryGetValue(id, out result)) {
                var assetType = MetaModel.GetAssetType(assetTypeToken);
                var assetId = id as AssetID;
                result = assetId != null ? new Asset(Oid.FromToken(assetId, MetaModel)) : new Asset(assetType);
                SetAsset(id, result);
            }

            return result;
        }

        /// <summary>
        /// Retrieve an asset stored in asset cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an Asset or null if the asset cannot be found in the cache</returns>
        internal Asset GetAsset(object id) {
            Asset asset;
            assetCache.TryGetValue(id, out asset);
            return asset;
        }

        internal void SetAsset(object id, Asset asset) {
            assetCache[id] = asset;
        }

        internal static string GetDefaultOrderByToken(Type entityType) {
            var attributes = entityType.GetCustomAttributes(typeof (MetaDataAttribute), false);
            
            if (attributes.Length > 0) {
                var attribute = (MetaDataAttribute) attributes[0];
                return attribute.DefaultOrderByToken;
            }

            throw new ApplicationException("Missing MetaDataAttribute on type " + entityType.FullName);
        }

        internal static string GetAssetTypeToken(Type entityType) {
            var attributes = entityType.GetCustomAttributes(typeof (MetaDataAttribute), false);

            if(attributes.Length > 0) {
                var attribute = (MetaDataAttribute) attributes[0];
                return attribute.AssetTypeToken;
            }

            throw new ApplicationException("Missing MetaDataAttribute on type " + entityType.FullName);
        }

        private T CreateWrapper<T>() where T : Entity {
            return wrapperManager.Create<T>();
        }

        private T CreateWrapper<T>(AssetID id, bool validate) where T : Entity {
            return wrapperManager.Create<T>(id, validate);
        }

        private Attribute RetrieveAttribute(Oid oid, IAttributeDefinition def, bool cachable) {
            var query = new Query(oid);
            query.Selection.Add(def);
            var r = Services.Retrieve(query);
            var asset = r.Assets[0];

            if (cachable) {
                var cached = GetAsset(asset.Oid);
                var attribute = cached.EnsureAttribute(def);

                if(def.IsMultiValue) {
                    foreach(var value in asset.GetAttribute(def).Values) {
                        cached.LoadAttributeValue(def, value);
                    }
                } else {
                    cached.LoadAttributeValue(def, asset.GetAttribute(def).Value);
                }

                attribute.AcceptChanges();
                return attribute;
            }

            return asset.GetAttribute(def);
        }

        private Attribute GetAttribute(Entity entity, string name, bool cachable) {
            var asset = GetAsset(entity);
            var def = asset.AssetType.GetAttributeDefinition(name);
            var attribute = (cachable ? asset.GetAttribute(def) : null) ?? RetrieveAttribute(asset.Oid, def, cachable);

            return attribute;
        }

        /// <summary>
        /// Clear an property from cache of specified Entity.
        /// </summary>
        /// <param name="entity">to clear property of.</param>
        /// <param name="name">of the property to clear;
        /// if null, all properties will be cleared from cache.</param>
        internal void ClearCache(Entity entity, string name) {
            var asset = GetAsset(entity);
            IAttributeDefinition def = null;
            
            if(name != null) {
                def = asset.AssetType.GetAttributeDefinition(name);
            }

            asset.ClearAttributeCache(def);
        }

        internal bool AttributeExists(Entity entity, string name) {
            var asset = GetAsset(entity);
            IAttributeDefinition def = null;
            
            try {
                def = asset.AssetType.GetAttributeDefinition(name);
            } catch (MetaException) { }

            return def != null;
        }

        internal Stream GetReadStream(Entity entity) {
            var oid = GetOid(entity);
            return Attachments.GetReadStream(oid.Key.ToString());
        }

        internal Stream GetWriteStream(Entity entity) {
            var oid = GetOid(entity);
            return Attachments.GetWriteStream(oid.Key.ToString());
        }

        internal void CommitWriteStream(Entity entity, string contentType) {
            var oid = GetOid(entity);
            
            try {
                Attachments.SetWriteStream(oid.Key.ToString(), contentType);
            } catch (AttachmentLengthException ex) {
                throw new AttachmentLengthExceededException(ex.Message);
            }
        }

        internal T GetProperty<T>(Entity entity, string name, bool cachable) {
            return (T) GetAttribute(entity, name, cachable).Value;
        }

        internal void SetProperty<T>(Entity entity, string name, T value) {
            var asset = GetAsset(entity);
            var def = asset.AssetType.GetAttributeDefinition(name);
            asset.SetAttributeValue(def, value);
        }

        internal T GetRelation<T>(Entity entity, string name, bool cachable) where T : Entity {
            var oid = GetProperty<Oid>(entity, name, cachable);
            return oid.IsNull ? null : CreateWrapper<T>(new AssetID(oid.Token), false);
        }

        internal void SetRelation<T>(Entity entity, string name, T value) where T : Entity {
            var oid = value == null ? Oid.Null : Oid.FromToken(value.ID, MetaModel);
            SetProperty(entity, name, oid);
        }

        internal void AddRelation<T>(Entity entity, string name, T value) where T : Entity {
            var asset = GetAsset(entity);
            var def = asset.AssetType.GetAttributeDefinition(name);
            var oid = Oid.FromToken(value.ID, MetaModel);
            asset.AddAttributeValue(def, oid);
        }

        internal void RemoveRelation<T>(Entity entity, string name, T value) where T : Entity {
            var asset = GetAsset(entity);
            var def = asset.AssetType.GetAttributeDefinition(name);
            var oid = Oid.FromToken(value.ID, MetaModel);
            asset.RemoveAttributeValue(def, oid);
        }

        internal EntityCollection<T> GetMultiRelation<T>(Entity entity, string name, bool cachable) where T : Entity {
            return new EntityCollection<T>(this, entity, name, name);
        }

        internal EntityCollection<T> GetMultiRelation<T>(Entity entity, string readName, string writeName, bool cachable) where T : Entity {
            return new EntityCollection<T>(this, entity, readName, writeName);
        }

        internal bool MultiRelationContains<T>(Entity entity, string attributeName, T value) where T : Entity {
            var collection = InternalGetMultiRelation<T>(entity, attributeName);
            return collection.Contains(value);
        }

        internal int MultiRelationCount(Entity entity, string attributeName) {
            return (int) GetProperty<double>(entity, attributeName + ".@Count", false);
        }

        internal ICollection<T> InternalGetMultiRelation<T>(Entity entity, string attributeName) where T : Entity {
            var currentOids = GetAttribute(entity, attributeName, false).Values;
            var strongEnum = new CastTransformEnumerable<Oid>(currentOids);
            return OidEnumToEntityCollection<T>(strongEnum);
        }

        internal bool MultiRelationIsReadOnly(Entity entity, string attributeName) {
            return
                MetaModel.GetAssetType(GetAssetTypeToken(entity.GetType())).GetAttributeDefinition(attributeName).
                    IsReadOnly;
        }

        #endregion

        #region Relation to List Types

        internal T GetListValueByName<T>(string value) where T : ListValue {
            var typeToken = GetAssetTypeToken(typeof (T));
            var listType = MetaModel.GetAssetType(typeToken);
            return GetListValueByName<T>(listType, value);
        }

        private T GetListValueByName<T>(IAssetType listType, string value) where T : Entity {
            var nameDef = listType.GetAttributeDefinition("Name");

            var query = new Query(listType);
            var nameTerm = new FilterTerm(nameDef);
            nameTerm.Equal(value);
            query.Filter = nameTerm;
            query.OrderBy.MajorSort(nameDef, OrderBy.Order.Ascending);

            var result = Services.Retrieve(query);

            if(result.Assets.Count == 0) {
                throw new ArgumentOutOfRangeException("value", string.Format("There is no {0} value with name: {1}", listType.Token, value));
            }

            return CreateWrapper<T>(new AssetID(result.Assets[0].Oid.Token), false);
        }

        internal void SetListRelation<T>(Entity entity, string name, string value) where T : ListValue {
            T valueEntity = null;

            if(value != null) {
                valueEntity = GetListValueByName<T>(value);
            }

            SetRelation(entity, name, valueEntity);
        }

        #endregion

        #region Custom List Types

        internal void SetCustomListRelation(Entity entity, string name, string value) {
            CustomListValue valueEntity = null;

            if(value != null) {
                valueEntity = GetCustomListValueByName(entity, name, value);
            }

            SetRelation(entity, name, valueEntity);
        }

        internal CustomListValue GetCustomListValueByName(Entity entity, string name, string value) {
            var relatedType = GetRelatedType(entity, name);
            return GetListValueByName<CustomListValue>(relatedType, value);
        }

        private IAssetType GetRelatedType(Entity entity, string name) {
            var owningType = MetaModel.GetAssetType(GetAssetTypeToken(entity.GetType()));
            var listTypeDef = owningType.GetAttributeDefinition(name);
            return listTypeDef.RelatedAsset;
        }

        internal CustomListValue GetCustomRelation(Entity entity, string name) {
            return GetCustomRelation(entity, name, true);
        }

        internal CustomListValue GetCustomRelation(Entity entity, string name, bool cachable) {
            return GetRelation<CustomListValue>(entity, name, cachable);
        }

        internal T GetPropertyOnCustomType<T>(CustomListValue entity, string name, bool cachable) {
            return (T) GetAttributeOnCustomType(entity, name, cachable).Value;
        }

        private Attribute GetAttributeOnCustomType(Entity entity, string name, bool cachable) {
            var asset = GetAsset(Oid.FromToken(entity.ID, MetaModel));
            var def = asset.AssetType.GetAttributeDefinition(name);
            var attribute = (cachable ? asset.GetAttribute(def) : null) ?? RetrieveAttribute(asset.Oid, def, cachable);

            return attribute;
        }

        internal IEnumerable<CustomListValue> GetCustomListTypeValues(Entity entity, string attributeName) {
            var typeToGet = GetRelatedType(entity, attributeName);

            var query = new Query(typeToGet);
            var assetStateTerm = new FilterTerm(typeToGet.GetAttributeDefinition("AssetState"));
            assetStateTerm.NotEqual(AssetState.Closed);
            query.Filter = new AndFilterTerm(assetStateTerm);

            return Services.Retrieve(query).Assets.Select(asset => CreateWrapper<CustomListValue>(new AssetID(asset.Oid.Token), false)).ToList();
        }


        #endregion

        #region Rank Methods

        internal Rank<T> GetRank<T>(Entity entity, string attributeName) where T : Entity {
            return new Rank<T>(this, entity, attributeName);
        }

        internal void RankAbove(Entity rank, Entity above, string attributeName) {
            SetProperty(rank, attributeName, GetProperty<Rank>(above, attributeName, false).Before);
        }

        internal void RankBelow(Entity rank, Entity below, string attributeName) {
            SetProperty(rank, attributeName, GetProperty<Rank>(below, attributeName, false).After);
        }

        internal bool IsRankAbove(Entity rank, Entity above, string attributeName) {
            return GetProperty<Rank>(rank, attributeName, false) < GetProperty<Rank>(above, attributeName, false);
        }

        internal bool IsRankBelow(Entity rank, Entity below, string attributeName) {
            return GetProperty<Rank>(rank, attributeName, false) > GetProperty<Rank>(below, attributeName, false);
        }

        #endregion

        #region Calculations

        internal double? GetSum(Entity entity, string multiRelationName, EntityFilter filter, string numericAttributeName) {
            var oid = GetOid(entity);

            var multiRelation = oid.AssetType.GetAttributeDefinition(multiRelationName);
            var relatedType = multiRelation.RelatedAsset;
            var filtered = multiRelation;
            
            if(filter != null) {
                var term = filter.BuildFilter(relatedType, this);
                
                if(term != null) {
                    filtered = multiRelation.Filter(term);
                }
            }

            var relatedAttribute = relatedType.GetAttributeDefinition(numericAttributeName);
            var joined = filtered.Join(relatedAttribute);
            var sum = joined.Aggregate(Aggregate.Sum);

            return (double?) RetrieveAttribute(oid, sum, false).Value;
        }

        #endregion

        #region Tracking Levels

        internal bool CheckTracking(Workitem workitem) {
            var level = GetTrackingLevel(workitem);

            if(workitem is PrimaryWorkitem && level == TrackingLevel.SecondaryWorkitem) {
                return false;
            }

            if(workitem is SecondaryWorkitem && level == TrackingLevel.PrimaryWorkitem) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Call this before setting Detail Estimate or TODO on a workitem
        /// </summary>
        /// <param name="workitem"></param>
        /// <exception cref="System.InvalidOperationException">If setting DetailEstimate is not allowed at this level.</exception>
        internal void PreventTrackingLevelAbuse(Workitem workitem) {
            var level = GetTrackingLevel(workitem);
            
            if (workitem is PrimaryWorkitem) {
                if(level == TrackingLevel.SecondaryWorkitem) {
                    throw new InvalidOperationException(
                        "You cannot set DetailEstimate or ToDo on this item, nor can you log effort, because the system is configured to track Detail Estimate and ToDo at the Task/Test level.");
                }
            } else {
                if(level == TrackingLevel.PrimaryWorkitem) {
                    throw new InvalidOperationException(
                        "You cannot set DetailEstimate or ToDo on this item, nor can you log effort, because the system is configured to track Detail Estimate and ToDo at the Story/Defect level.");
                }
            }
        }

        private TrackingLevel GetTrackingLevel(Workitem workitem) {
            Workitem parent;

            if(workitem is SecondaryWorkitem) {
                parent = ((SecondaryWorkitem) workitem).Parent;
            } else {
                parent = workitem;
            }

            if(parent is Story) {
                return Configuration.StoryTrackingLevel;
            }

            if(parent is Defect) {
                return Configuration.DefectTrackingLevel;
            }

            throw new ArgumentOutOfRangeException("workitem", "Expected a Story, a Defect or a child work item of one.");
        }

        #endregion
    }
}