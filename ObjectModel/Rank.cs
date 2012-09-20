namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// How assets are ordered.
    /// </summary>
    /// <typeparam name="T">The type of asset being ranked.</typeparam>
    public class Rank<T> where T : Entity {
        private readonly Entity asset;
        private readonly string rankAttribute;
        private readonly V1Instance instance;

        internal Rank(V1Instance instance, Entity asset, string rankAttribute) {
            this.instance = instance;
            this.asset = asset;
            this.rankAttribute = rankAttribute;
        }

        /// <summary>
        /// Set this Entity ahead of the passed in Entity in rank order.
        /// </summary>
        /// <param name="assetToRankAheadOf">The Entity that will come next in order after this Entity.</param>
        public void SetAbove(T assetToRankAheadOf) {
            instance.RankAbove(asset, assetToRankAheadOf, rankAttribute);
            asset.Save();
        }

        /// <summary>
        /// Set this Entity after the passed in Entity in rank order.
        /// </summary>
        /// <param name="assetToRankAfter">The Entity that will come just before this Entity in rank order.</param>
        public void SetBelow(T assetToRankAfter) {
            instance.RankBelow(asset, assetToRankAfter, rankAttribute);
            asset.Save();
        }

        /// <summary>
        /// Is this Entity ahead of the passed in Entity in rank order?
        /// </summary>
        /// <param name="otherAsset">The Entity that will come next in order after this Entity.</param>
        public bool IsAbove(T otherAsset) {
            return instance.IsRankAbove(asset, otherAsset, rankAttribute);
        }

        /// <summary>
        /// Is this Entity after the passed in Entity in rank order.
        /// </summary>
        /// <param name="otherAsset">The Entity that will come just before this Entity in rank order.</param>
        public bool IsBelow(T otherAsset) {
            return instance.IsRankBelow(asset, otherAsset, rankAttribute);
        }
    }
}