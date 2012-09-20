using System;
using System.Collections.Generic;

using VersionOne.SDK.ObjectModel.List;

namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// A story or backlog item
    /// </summary>
    [MetaData("Story", null, "Story.Order")]
    public class Story : PrimaryWorkitem {
        internal Story(V1Instance instance) : base(instance) {
        }

        internal Story(AssetID id, V1Instance instance) : base(id, instance) {
        }

        /// <summary>
        /// Name of person or organization requesting this Story.
        /// </summary>
        public string RequestedBy {
            get { return Get<string>("RequestedBy"); }
            set { Set("RequestedBy", value); }
        }

        /// <summary>
        /// Build number associated with this Story.
        /// </summary>
        [MetaRenamed("LastVersion")]
        public string Build {
            get { return Get<string>("LastVersion"); }
            set { Set("LastVersion", value); }
        }

        /// <summary>
        /// The Epic that this Story belongs to.
        /// </summary>
        [MetaRenamed("Super")]
        public Epic Epic {
            get { return GetRelation<Epic>("Super"); }
            set { SetRelation("Super", value); }
        }

        /// <summary>
        /// This Story's Risk
        /// </summary>
        public IListValueProperty Risk {
            get { return GetListValue<WorkitemRisk>("Risk"); }
        }

        /// <summary>
        /// This Story's Type
        /// </summary>
        [MetaRenamed("Category")]
        public IListValueProperty Type {
            get { return GetListValue<StoryType>("Category"); }
        }

        /// <summary>
        /// Member assigned as a customer for this Story.
        /// </summary>
        public Member Customer {
            get { return GetRelation<Member>("Customer"); }
            set { SetRelation("Customer", value); }
        }

        /// <summary>
        /// Stories that this Story depends on.
        /// </summary>
        public ICollection<Story> DependsOnStories {
            get { return GetMultiRelation<Story>("Dependencies"); }
        }

        /// <summary>
        /// Stories that depend on this Story.
        /// </summary>
        public ICollection<Story> DependentStories {
            get { return GetMultiRelation<Story>("Dependants"); }
        }

        /// <summary>
        /// The (optional) Retrospective this Story was Identified in.
        /// </summary>
        public Retrospective IdentifiedIn {
            get { return GetRelation<Retrospective>("IdentifiedIn"); }
            set { SetRelation("IdentifiedIn", value); }
        }

        /// <summary>
        /// Benefits of the Story
        /// </summary>
        public string Benefits {
            get { return Get<string>("Benefits"); }
            set { Set("Benefits", value); }
        }

        /// <summary>
        /// Inactivates the Story
        /// </summary>
        /// <exception cref="InvalidOperationException">The Story is an invalid state for the Operation, e.g. it is already closed.</exception>
        internal override void CloseImpl() {
            Instance.ExecuteOperation<Story>(this, "Inactivate");
        }

        /// <summary>
        /// Reactivates the Story
        /// </summary>
        /// <exception cref="InvalidOperationException">The Story is an invalid state for the Operation, e.g. it is already active.</exception>
        internal override void ReactivateImpl() {
            Instance.ExecuteOperation<Story>(this, "Reactivate");
        }

        /// <summary>
        /// Breakdown this Story to Epic
        /// </summary>
        public void Breakdown() {
            Save();
            Instance.ExecuteOperation<Story>(this, "Breakdown");
        }

        /// <summary>
        /// Can Story be converted to Epic.
        /// </summary>
        public bool CanBreakdown() {
            return Get<bool>("CheckBreakdown");
        }
    }
}