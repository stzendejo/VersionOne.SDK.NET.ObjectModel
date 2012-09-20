using System;
using System.Collections;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel {
    internal class EntityCollection<D> : ICollection<D> where D : Entity {
        private readonly V1Instance instance;
        private readonly Entity entity;
        private readonly string readAttributeName;
        private readonly string writeAttributeName;

        public EntityCollection(V1Instance instance, Entity entity, string readAttributeName, string writeAttributeName) {
            this.instance = instance;
            this.entity = entity;
            this.readAttributeName = readAttributeName;
            this.writeAttributeName = writeAttributeName;
        }

        public void Add(D item) {
            ReadOnlyGuardCondition();
            instance.AddRelation(entity, writeAttributeName, item);
            entity.Save();
        }

        public void Clear() {
            ReadOnlyGuardCondition();

            foreach(D item in this) {
                instance.RemoveRelation(entity, writeAttributeName, item);
            }

            entity.Save();
        }

        public bool Contains(D item) {
            return instance.MultiRelationContains(entity, readAttributeName, item);
        }

        public void CopyTo(D[] array, int arrayIndex) {
            if(arrayIndex < 0) {
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex cannot be less than 0.");
            }

            if (array == null) {
                throw new ArgumentNullException("array");
            }

            if (array.Rank > 1) {
                throw new ArgumentException("Cannot copy to a multi-deminsional array.");
            }

            if(array.Length < arrayIndex) {
                throw new ArgumentException("arrayIndex is greater than the length of the array.");
            }

            if((arrayIndex + Count) > array.Length) {
                throw new ArgumentException(
                    string.Format("The number of elements is greater than the length of the array.  Array length:{0}; arrayIndex: {1}; Count:{2}.",
                        array.Length, arrayIndex, Count));
            }

            var count = 0;

            foreach (D item in this) {
                array[arrayIndex + count] = item;
                count++;
            }
        }

        public bool Remove(D item) {
            ReadOnlyGuardCondition();

            if (Contains(item)) {
                instance.RemoveRelation(entity, writeAttributeName, item);
                entity.Save();
                return true;
            }
            
            return false;
        }

        public int Count {
            get { return instance.MultiRelationCount(entity, readAttributeName); }
        }

        public virtual bool IsReadOnly {
            get { return instance.MultiRelationIsReadOnly(entity, writeAttributeName); }
        }

        IEnumerator<D> IEnumerable<D>.GetEnumerator() {
            return instance.InternalGetMultiRelation<D>(entity, readAttributeName).GetEnumerator();
        }

        public IEnumerator GetEnumerator() {
            return ((IEnumerable<D>) this).GetEnumerator();
        }

        /// <summary>
        /// Throws if IsReadOnly
        /// </summary>
        private void ReadOnlyGuardCondition() {
            if(IsReadOnly) {
                throw new NotSupportedException("The collection is Read-only");
            }
        }

        public EntityCollection<D> AsReadOnly() {
            return new ReadOnly<D>(instance, entity, readAttributeName, writeAttributeName);
        }

        private class ReadOnly<T> : EntityCollection<T> where T : Entity {
            public ReadOnly(V1Instance instance, Entity entity, string readAttributeName, string writeAttributeName)
                : base(instance, entity, readAttributeName, writeAttributeName) {}

            public override bool IsReadOnly {
                get { return true; }
            }
        }
    }
}