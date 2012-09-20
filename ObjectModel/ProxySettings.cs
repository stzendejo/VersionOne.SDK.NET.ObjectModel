using System;
using System.Collections.Generic;
using System.Text;

namespace VersionOne.SDK.ObjectModel 
{
    /// <summary>
    /// Proxy settings that you can pass to V1Instance.
    /// </summary>
    public class ProxySettings 
    {
        /// <summary>
        /// Proxy server path.
        /// </summary>
        public readonly Uri Path;
        /// <summary>
        /// Proxy username
        /// </summary>
        public readonly string Username;
        /// <summary>
        /// Proxy password
        /// </summary>
        public readonly string Password;
        /// <summary>
        /// Proxy domain
        /// </summary>
        public readonly string Domain;

        /// <summary>
        /// Create proxy settings
        /// </summary>
        /// <param name="path">Proxy server path, ex. http://proxy:3128</param>
        /// <param name="username">Proxy username</param>
        /// <param name="password">Proxy user password</param>
        /// <param name="domain">Proxy domain</param>
        public ProxySettings(Uri path, string username, string password, string domain) 
        {
            Path = path;
            Username = username;
            Password = password;
            Domain = domain;
        }

        /// <summary>
        /// Create proxy settings
        /// </summary>
        /// <param name="path">Proxy server path, ex. http://proxy:3128</param>
        /// <param name="username">Proxy username</param>
        /// <param name="password">Proxy user password</param>
        public ProxySettings(Uri path, string username, string password) : this(path, username, password, null) { } 
    }
}
