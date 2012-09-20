namespace VersionOne.SDK.ObjectModel {
    /// <summary>
    /// Represents a unique ID in the VersionOne system. The ToToken and FromToken methods are
    /// provided to convert an AssetID to and from a string for storage.
    /// </summary>
    public class AssetID {
        private readonly string token;

        /// <summary>
        /// Construct an AssetID by ID
        /// </summary>
        /// <param name="token">id of the Asset</param>
        internal AssetID(string token) {
            this.token = token;
        }

        /// <summary>
        /// String representation of the AssetID.
        /// </summary>
        public string Token {
            get { return ToString(); }
        }

        /// <summary>
        /// Create an AssetID form a tokenized ID.
        /// </summary>
        /// <param name="token"></param>
        public static AssetID FromToken(string token) {
            return new AssetID(token);
        }

        /// <summary>
        /// Override Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var other = obj as AssetID;
            
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return token == other.token;
        }

        /// <summary>
        /// Override Equals
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return token.GetHashCode();
        }

        /// <summary>
        /// Tokenize the ID.
        /// </summary>
        /// <returns>A string representation of the ID. The same a the Token property.</returns>
        public override string ToString() {
            return token;
        }

        /// <summary>
        /// Overload equal equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(AssetID a, AssetID b) {
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
        public static bool operator !=(AssetID a, AssetID b) {
            return !(a == b);
        }

        /// <summary>
        /// Convert a string to an AssetID.
        /// </summary>
        /// <param name="idString">The tokenized ID</param>
        /// <returns></returns>
        public static implicit operator AssetID(string idString) {
            return FromToken(idString);
        }

        /// <summary>
        /// Convert an AssetID to a string.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static implicit operator string(AssetID id) {
            return id.Token;
        }
    }
}