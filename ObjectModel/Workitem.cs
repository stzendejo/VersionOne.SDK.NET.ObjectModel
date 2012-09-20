using System;
using System.Collections.Generic;
using VersionOne.SDK.ObjectModel.Filters;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Base class for Stories, Defects, Tasks, Tests
    /// </summary>
    [MetaData("Workitem")]
    public abstract class Workitem : ProjectAsset {
        internal Workitem(V1Instance instance) : base(instance) {}
        internal Workitem(AssetID id, V1Instance instance) : base(id, instance) {}

        /// <summary>
        /// Returns true if this workitem detail estimate and todo can be updated
        /// </summary>
        public bool CanTrack {
            get { return Instance.CheckTracking(this); }
        }

        /// <summary>
        /// Members that own this item.
        /// </summary>
        public ICollection<Member> Owners {
            get { return GetMultiRelation<Member>("Owners"); }
        }

        /// <summary>
        /// Estimate of effort required to implement this item.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If setting DetailEstimate is not allowed at this level.</exception>
        public double? DetailEstimate {
            get { return Get<double?>("DetailEstimate"); }
            set {
                Instance.PreventTrackingLevelAbuse(this);
                Set("DetailEstimate", value);
            }
        }

        /// <summary>
        /// Effort already expended to implement this item.
        /// </summary>
        public double? Done {
            get { return Get<double?>("Actuals.Value.@Sum", false); }
        }

        /// <summary>
        /// Remaining effort required to complete implementation of this item.
        /// </summary>
        public double? ToDo {
            get { return Get<double?>("ToDo"); }
            set {
                Instance.PreventTrackingLevelAbuse(this);
                Set("ToDo", value);
            }
        }

        /// <summary>
        /// Effort Records tracked against this item.
        /// </summary>
        public ICollection<Effort> GetEffortRecords(EffortFilter filter) {
            filter = filter ?? new EffortFilter();
            filter.Workitem.Clear();
            filter.Workitem.Add(this);
            return Instance.Get.EffortRecords(filter);
        }

        /// <summary>
        /// Cross-reference of this item with an external system.
        /// </summary>
        public string Reference {
            get { return Get<string>("Reference"); }
            set { Set("Reference", value); }
        }

        /// <summary>
        /// Log an effort record against this workitem
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(double value) {
            return Instance.Create.Effort(value, this);
        }

        /// <summary>
        /// Log an effort record against this workitem
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attributes">required attributes</param>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(double value, IDictionary<string, object> attributes) {
            return Instance.Create.Effort(value, this, attributes);
        }

        /// <summary>
        /// Log an effort record against this workitem with the given member and value
        /// </summary>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(Member member, double value) {
            return Instance.Create.Effort(value, this, member, DateTime.Now);
        }

        /// <summary>
        /// Log an effort record against this workitem with the given member and value
        /// </summary>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(Member member, double value, IDictionary<string, object> attributes) {
            return Instance.Create.Effort(value, this, member, DateTime.Now, attributes);
        }

        /// <summary>
        /// Log an effort record against this workitem with the given member, date, and value
        /// </summary>
        /// <param name="member"></param>
        /// <param name="date"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(Member member, DateTime date, double value) {
            return Instance.Create.Effort(value, this, member, date);
        }

        /// <summary>
        /// Log an effort record against this workitem with the given member, date, and value
        /// </summary>
        /// <param name="member"></param>
        /// <param name="date"></param>
        /// <param name="value"></param>
        /// <param name="attributes">required attributes</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Effort Tracking is not enabled.</exception>
        public Effort CreateEffort(Member member, DateTime date, double value, IDictionary<string, object> attributes) {
            return Instance.Create.Effort(value, this, member, date, attributes);
        }
    }
}