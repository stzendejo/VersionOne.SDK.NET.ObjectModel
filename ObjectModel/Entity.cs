using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Abstract representation of entities in the VersionOne system
    /// </summary>
    public abstract class Entity {
        /// <summary>
        /// V1Instance this entity belongs to
        /// </summary>
        protected V1Instance Instance {
            get { return instance; }
        }

        private readonly V1Instance instance;

        /// <summary>
        /// Constructor used to represent an entity that does exist in the VersionOne System
        /// </summary>
        /// <param name="id">Unique ID of this entity</param>
        /// <param name="instance">Instance this entity belongs to</param>
        internal Entity(AssetID id, V1Instance instance) {
            assetID = id;
            this.instance = instance;
        }

        /// <summary>
        /// Constructor used to represent an entity that does not exist yet in the VersionOne System
        /// </summary>
        internal Entity(V1Instance instance) {
            stubAssetID = new StubAssetID();
            this.instance = instance;
        }

        /// <summary>
        /// Unique ID of this entity
        /// </summary>
        public AssetID ID {
            get { return assetID; }
        }

        private AssetID assetID;
        private StubAssetID stubAssetID;

        internal object InstanceKey {
            get { return assetID ?? (object) stubAssetID; }
        }

        #region Common Attributes

        /// <summary>
        /// Date this entity was last changed in UTC
        /// </summary>
        public DateTime ChangeDate {
            get {
                var date = Get<DateTime>("ChangeDateUTC", false);
                date = new DateTime(date.Ticks, DateTimeKind.Utc);
                return date.ToLocalTime();
            }
        }

        /// <summary>
        /// Comment entered when this entity was last updated
        /// </summary>
        public string ChangeComment {
            get { return Get<string>("ChangeComment", false); }
        }

        /// <summary>
        /// Member or user that last changed this entity
        /// </summary>
        public Member ChangedBy {
            get { return GetRelation<Member>("ChangedBy", false); }
        }

        /// <summary>
        /// Date this entity was created in UTC
        /// </summary>
        public DateTime CreateDate {
            get {
                var date = Get<DateTime>("CreateDateUTC", true);
                date = new DateTime(date.Ticks, DateTimeKind.Utc);
                return date.ToLocalTime();
            }
        }

        /// <summary>
        /// Comment entered when this entity was created
        /// </summary>
        public string CreateComment {
            get { return Get<string>("CreateComment", true); }
        }

        /// <summary>
        /// Member or user that created this entity
        /// </summary>
        public Member CreatedBy {
            get { return GetRelation<Member>("CreatedBy", true); }
        }

        #endregion

        internal string GetEntityURL() {
            return instance.GetEntityURL(this);
        }

        /// <summary>
        /// Gets a simple value by name for this entity
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>An attribute value.</returns>
        internal T Get<T>(string name) {
            return instance.GetProperty<T>(this, name, true);
        }

        /// <summary>
        /// Gets a simple value by name for this entity. Ignore cache if cachable is false
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="cachable">False if the attribute should not be cached.</param>
        /// <returns>An attribute value.</returns>
        internal T Get<T>(string name, bool cachable) {
            return instance.GetProperty<T>(this, name, cachable);
        }

        /// <summary>
        /// Sets a simple value by name for this entity
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="value">The value to set the attribute to.</param>
        internal void Set<T>(string name, T value) {
            instance.SetProperty(this, name, value);
        }

        /// <summary>
        /// Clears a cached property value.
        /// </summary>
        /// <param name="name">Name of the property;
        /// if null, all properties will be cleared from cache.</param>
        internal void ClearCache(string name) {
            instance.ClearCache(this, name);
        }

        /// <summary>
        /// Get a total of an attribute thru a multi-relation possibly slicing by a filter
        /// </summary>
        /// <param name="multiRelationName"></param>
        /// <param name="filter"></param>
        /// <param name="numericAttributeName"></param>
        /// <returns></returns>
        internal double? GetSum(string multiRelationName, EntityFilter filter, string numericAttributeName) {
            return instance.GetSum(this, multiRelationName, filter, numericAttributeName);
        }

        /// <summary>
        /// Get a read-only attachment stream for this entity.
        /// </summary>
        /// <returns></returns>
        internal Stream GetReadStream() {
            return instance.GetReadStream(this);
        }

        /// <summary>
        /// Gets a write-enabled attachment stream for this entity.
        /// </summary>
        /// <returns></returns>
        internal Stream GetWriteStream() {
            return instance.GetWriteStream(this);
        }

        /// <summary>
        /// Complete saving a write-enabled attachment stream for this entity.
        /// </summary>
        internal void CommitWriteStream(string contentType) {
            instance.CommitWriteStream(this, contentType);
        }

        /// <summary>
        /// Get Rank attribute for this Entity.
        /// </summary>
        /// <typeparam name="T">My type.</typeparam>
        /// <param name="attributeName">Name of the Rank attribute.</param>
        /// <returns>A Rank object.</returns>
        internal Rank<T> GetRank<T>(string attributeName) where T : Entity {
            return instance.GetRank<T>(this, attributeName);
        }

        /// <summary>
        /// Get a relation by name for this entity
        /// </summary>
        /// <typeparam name="T">The type of the related asset.</typeparam>
        /// <param name="name">Name of the relation attribute.</param>
        /// <returns>The related asset.</returns>
        internal T GetRelation<T>(string name) where T : Entity {
            return GetRelation<T>(name, true);
        }

        /// <summary>
        /// Get a relation by name for this entity. Ignore cache if cachable is false
        /// </summary>
        /// <typeparam name="T">The type of the related asset.</typeparam>
        /// <param name="name">Name of the relation attribute.</param>
        /// <param name="cachable">False if should not be cached.</param>
        /// <returns>The related asset.</returns>
        internal T GetRelation<T>(string name, bool cachable) where T : Entity {
            return instance.GetRelation<T>(this, name, cachable);
        }

        /// <summary>
        /// Sets a relation by name for this entity
        /// </summary>
        /// <typeparam name="T">The type of the related asset.</typeparam>
        /// <param name="name">Name of the relation attribute.</param>
        /// <param name="value">What to set the relation attribute to.</param>
        internal void SetRelation<T>(string name, T value) where T : Entity {
            instance.SetRelation(this, name, value);
        }

        /// <summary>
        /// Get a multi-value relation by name for this entity
        /// </summary>
        /// <typeparam name="T">The type of the related asset.</typeparam>
        /// <param name="name">Name of the relation attribute.</param>
        /// <returns>IEntityCollection of T</returns>
        internal EntityCollection<T> GetMultiRelation<T>(string name) where T : Entity {
            return GetMultiRelation<T>(name, true);
        }

        /// <summary>
        /// Get a multi-value relation by name for this entity. Ignore cache if cachable is false
        /// </summary>
        /// <typeparam name="T">The type of the related asset.</typeparam>
        /// <param name="name">Name of the relation attribute.</param>
        /// <param name="cachable">False if should not be cached.</param>
        /// <returns>IEntityCollection of T</returns>
        internal EntityCollection<T> GetMultiRelation<T>(string name, bool cachable) where T : Entity {
            return instance.GetMultiRelation<T>(this, name, cachable);
        }

        /// <summary>
        /// Save any changes to this entity to the VersionOne System with a comment
        /// </summary>
        /// <exception cref="DataException">Thrown when a rule or security violation has occurred.</exception>
        public void Save(string comment) {
            assetID = instance.Commit(this, comment);
            stubAssetID = null;
        }

        /// <summary>
        /// Save any changes to this entity to the VersionOne System
        /// </summary>
        /// /// <exception cref="DataException">Thrown when a rule or security violation has occurred.</exception>
        public void Save() {
            Save(null);
        }

        #region Object overrides

        /// <summary>
        /// Override Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            Entity other = obj as Entity;
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return assetID == other.assetID && stubAssetID == other.stubAssetID;
        }

        /// <summary>
        /// Override Equals
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return InstanceKey.GetHashCode();
        }

        /// <summary>
        /// Returns the AssetID token for the Entity.
        /// </summary>
        /// <returns>AssetID token for the Entity</returns>
        public override string ToString() {
            return ID.ToString();
        }

        /// <summary>
        /// Overload equal equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Entity a, Entity b) {
            if(ReferenceEquals(a, null) || ReferenceEquals(b, null)) {
                return ReferenceEquals(a, null) && ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Overload not equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Entity a, Entity b) {
            return !(a == b);
        }

        #endregion

        internal T GetListRelation<T>(string name) where T : ListValue {
            return GetRelation<T>(name, true);
        }

        /// <summary>
        /// Sets a List relation by name.
        /// </summary>
        /// <typeparam name="T">The Entity Type that represents the List Type</typeparam>
        /// <param name="attributeName">The name of the Relation Attribute</param>
        /// <param name="value">The name of the List value to be set in the relation</param>
        internal void SetListRelation<T>(string attributeName, string value) where T : ListValue {
            Instance.SetListRelation<T>(this, attributeName, value);
        }

        /// <summary>
        /// Gets the value of a Relation to a Custom List Type
        /// </summary>
        /// <param name="attributeName">The name of the relation attribute</param>
        /// <returns>A <see cref="CustomListValue"/> representing the related Custom List Type, or null.</returns>
        internal CustomListValue GetCustomListValue(string attributeName) {
            return Instance.GetCustomRelation(this, attributeName);
        }

        internal void SetCustomListRelation(string name, string value) {
            Instance.SetCustomListRelation(this, name, value);
        }

        internal bool ContainsAttribute(string attributeName) {
            return instance.AttributeExists(this, attributeName);
        }

        #region Nested Custom Attribute Intrastructure Classes

        internal class SimpleCustomAttributeDictionary : ICustomAttributeDictionary {
            private readonly Entity entity;

            public SimpleCustomAttributeDictionary(Entity entity) {
                this.entity = entity;
            }

            public object this[string attributeName] {
                get { return entity.Get<object>("Custom_" + attributeName); }
                set { entity.Set("Custom_" + attributeName, value); }
            }

            public double? GetNumeric(string attributeName) {
                return (double?) this[attributeName];
            }

            public bool? GetBool(string attributeName) {
                return (bool) this[attributeName];
            }

            public DateTime? GetDate(string attributeName) {
                return (DateTime) this[attributeName];
            }

            public string GetString(string attributeName) {
                return (string) this[attributeName];
            }

            public bool ContainsKey(string attributeName) {
                return entity.ContainsAttribute("Custom_" + attributeName);
            }
        }

        internal class ListCustomAttributeDictionary : ICustomDropdownDictionary {
            private readonly Entity entity;

            public ListCustomAttributeDictionary(Entity entity) {
                this.entity = entity;
            }

            public IListValueProperty this[string attributeName] {
                get { return new CustomListValueProperty(entity, attributeName); }
            }

            public bool ContainsKey(string attributeName) {
                return entity.ContainsAttribute("Custom_" + attributeName);
            }
        }

        #endregion

        #region List Relation Infrastructure

        internal IEnumerable<T> GetListTypeValues<T>() where T : ListValue {
            return Instance.Get.ListTypeValues<T>();
        }

        internal abstract class BaseListValueProperty<T> : IListValueProperty where T : ListValue {
            protected readonly Entity entity;
            protected readonly string attributeName;

            public override string ToString() {
                return CurrentValue;
            }

            internal BaseListValueProperty(Entity entity, string attributeName) {
                this.entity = entity;
                this.attributeName = attributeName;
            }

            public abstract string CurrentValue { get; set; }

            public string[] AllValues {
                get {
                    return Items.Select(value => value.Name).ToArray();
                }
            }

            protected abstract IEnumerable<T> Items { get; }

            public bool IsValid(string value) {
                return value == null || Items.Any(item => item.Name == value);
            }

            public abstract void ClearCurrentValue();
        }

        internal class ListValueProperty<T> : BaseListValueProperty<T> where T : ListValue {
            internal ListValueProperty(Entity entity, string attributeName) : base(entity, attributeName) {}

            public override string CurrentValue {
                get {
                    var value = SelectedValue;
                    return value != null ? value.ToString() : null;
                }
                set { entity.SetListRelation<T>(attributeName, value); }
            }

            public IEnumerable<T> GetValues() {
                return entity.GetListTypeValues<T>();
            }

            protected override IEnumerable<T> Items {
                get { return GetValues(); }
            }


            private T SelectedValue {
                get { return entity.GetListRelation<T>(attributeName); }
            }

            public override void ClearCurrentValue() {
                entity.SetRelation<T>(attributeName, null);
            }
        }

        internal class CustomListValueProperty : BaseListValueProperty<CustomListValue> {
            public CustomListValueProperty(Entity entity, string attributeName) : base(entity, "Custom_" + attributeName) { }

            public override string CurrentValue {
                get {
                    var value = entity.GetCustomListValue(attributeName);
                    return value != null ? value.Name : null;
                }
                set { entity.SetCustomListRelation(attributeName, value); }
            }

            public IEnumerable<CustomListValue> GetValues() {
                return entity.GetCustomListTypeValues(attributeName);
            }

            protected override IEnumerable<CustomListValue> Items {
                get { return GetValues(); }
            }

            public override void ClearCurrentValue() {
                entity.SetRelation<CustomListValue>(attributeName, null);
            }
        }

        #endregion

        /// <summary>
        /// Rank this entity above other entity.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="attributeName"></param>
        internal void RankAbove(Entity other, string attributeName) {
            Instance.RankAbove(this, other, attributeName);
        }

        /// <summary>
        /// Rank this entity below other entity.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="attributeName"></param>
        internal void RankBelow(Entity other, string attributeName) {
            Instance.RankBelow(this, other, attributeName);
        }

        /// <summary>
        /// Get a list value for this entity by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        internal IListValueProperty GetListValue<T>(string name) where T : ListValue {
            return new ListValueProperty<T>(this, name);
        }

        /// <summary>
        /// Gets a list of possible Custom List Type values for a relationship attribute
        /// </summary>
        /// <param name="attributeName">The "friendly" name of the attribute.</param>
        /// <returns></returns>
        internal IEnumerable<CustomListValue> GetCustomListTypeValues(string attributeName) {
            return Instance.GetCustomListTypeValues(this, attributeName);
        }
    }
}