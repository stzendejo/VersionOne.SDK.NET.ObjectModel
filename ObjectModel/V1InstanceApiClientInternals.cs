using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using OAuth2Client;
using VersionOne.SDK.APIClient;

namespace VersionOne.SDK.ObjectModel
{
	public partial class V1Instance
	{
		/// <summary>
		/// Allows access to the underlying API Client structures.
		/// </summary>
        public class ApiClientInternals
        {
            private readonly string _applicationPath;
			private readonly IStorage _oauthStorage;
			private readonly string _username;
            private readonly string _password;
            private readonly bool _integratedAuth;

            private readonly ProxyProvider proxyProvider;

            internal ApiClientInternals(string applicationPath, string username, string password, bool integratedAuth, ProxyProvider proxyProvider)
            {
                _applicationPath = applicationPath;
                _username = username;
                _password = password;
                _integratedAuth = integratedAuth;

                this.proxyProvider = proxyProvider;
            }
			internal ApiClientInternals(string applicationPath, IStorage oauthStorage, ProxyProvider proxyProvider)
			{
				_applicationPath = applicationPath;
				_oauthStorage = oauthStorage;
				_username = null;
				_password = null;
				_integratedAuth = false;
				this.proxyProvider = proxyProvider;
			}

            internal string ApplicationPath { get { return _applicationPath; } }

            /// <summary>
            /// The underlying MetaModel provided by the API Client.
            /// </summary>
            public IMetaModel MetaModel
            {
                get
                {
                    if (_metaModel == null)
                    {
                        var connector = CreateConnector(_applicationPath + "meta.v1/");
                        _customHttpHeaders.AddDelegate(connector.CustomHttpHeaders);
                        _metaModel = new MetaModel(connector);
                    }
                    return _metaModel;
                }
            }
            private IMetaModel _metaModel;

            /// <summary>
            /// The underlying localizer provided by the API Client.
            /// </summary>
            public ILocalizer Localizer
            {
                get
                {
                    if (_loc == null)
                    {
                        var connector = CreateConnector(_applicationPath + "loc.v1/");
                        _customHttpHeaders.AddDelegate(connector.CustomHttpHeaders);
                        _loc = new Localizer(connector);
                    }
                    return _loc;
                }
            }
            private ILocalizer _loc;

            /// <summary>
            /// The underlying Services provided by the API Client.
            /// </summary>
            public IServices Services
            {
                get
                {
                    if (_services == null)
                    {
	                    var connector =
		                    CreateConnector(_applicationPath + "rest-1.v1/");
                        _customHttpHeaders.AddDelegate(connector.CustomHttpHeaders);
                        _services = new Services(MetaModel, connector);
                    }
                    return _services;
                }
            }
            private IServices _services;

            /// <summary>
            /// The underlying Attachments provided by the API Client.
            /// </summary>
            public IAttachments Attachments
            {
                get
                {
                    if (_attachments == null)
                    {
                        var connector = CreateConnector(_applicationPath + "attachment.img/");
                        _customHttpHeaders.AddDelegate(connector.CustomHttpHeaders);
                        _attachments = new Attachments(connector);
                    }
                    return _attachments;
                }
            }
            private IAttachments _attachments;

            /// <summary>
            /// The underlying V1Config provided by the API Client.
            /// </summary>
            public IV1Configuration V1Config
            {
                get
                {
                    if (_v1Config == null)
                    {
                        var connector = CreateConnector(_applicationPath + "config.v1/");
                        _customHttpHeaders.AddDelegate(connector.CustomHttpHeaders);
                        _v1Config = new V1Configuration(connector);
                    }
                    return _v1Config;
                }
            }

            /// <summary>
            /// Headers from this Dictionary will be added to all HTTP requests to VersionOne server.
            /// </summary>
            public IDictionary<string, string> CustomHttpHeaders
            {
                get { return (IDictionary<string, string>)_customHttpHeaders; }
            }

            private IV1Configuration _v1Config;
            private DelegatorDictionary _customHttpHeaders = new DelegatorDictionary();

            internal void Validate()
            {
//                V1ConnectionValidator validator = new V1ConnectionValidator(_applicationPath, _username, _password, _integratedAuth, proxyProvider);
//
//                foreach (KeyValuePair<string, string> pair in _customHttpHeaders)
//                {
//                    validator.CustomHttpHeaders.Add(pair);
//                }
//                
//                try
//                {
//                    validator.CheckConnection();
//                }
//                catch (ConnectionException)
//                {
//                    throw new ApplicationUnavailableException("Unable to connect to VersionOne.");
//                }
//
//                try
//                {
//                    validator.CheckAuthentication();
//                }
//                catch (ConnectionException)
//                {
//                    throw new AuthenticationException("Invalid username or password.");
//                }
            }

			
			private string _callerUserAgent = MakeUserAgent(RunningAssemblyName);
			/// <summary>
			/// Set the user agent that will be reported to the VersionOne Server.
			/// 
			/// Place some text that describes your client application here.  Perhaps:
			///   System.Reflection.Assembly.GetAssembly(typeof(YourClass)).GetName().FullName
			/// 
			/// </summary>
			/// <param name="userAgent"></param>
			public void SetCallerUserAgent(string userAgent)
			{
				_callerUserAgent = userAgent;
			}
			/// <summary>
			/// 
			/// </summary>
			public static AssemblyName MyAssemblyName = Assembly.GetAssembly(typeof(V1APIConnector)).GetName();
			/// <summary>
			/// 
			/// </summary>
			public static AssemblyName RunningAssemblyName = Assembly.GetExecutingAssembly().GetName();
			private static string MakeUserAgent(AssemblyName n, string upstream = "")
			{
				return String.Format("{0}/{1} ({2}) {3}", n.Name, n.Version, n.FullName, upstream);
			}
			private string MyUserAgent
			{
				get
				{
					return MakeUserAgent(MyAssemblyName, _callerUserAgent);
				}
			}


            private IAPIConnector CreateConnector(string url) 
            {
                // TODO check integratedAuth here
	            if (_oauthStorage == null)
	            {
		            var cc = new V1APIConnector(url, _username, _password, _integratedAuth, proxyProvider);
		            cc.SetCallerUserAgent(MyUserAgent);
		            return cc;
	            }
	            else
	            {
					if (url.EndsWith("rest-1.v1/"))
					{
						url = url.Replace("/rest-1.v1/", "/rest-1.oauth.v1/");
					}
		            var cc = new V1OAuth2APIConnector(url, _oauthStorage, proxyProvider);
		            cc.SetCallerUserAgent(MyUserAgent);
		            return cc;
	            }
            }


            private class DelegatorDictionary : IDictionary<string, string>
            {
                private readonly IDictionary<string, string> inner = new Dictionary<string, string>();
                private readonly IList<IDictionary<string, string>> Delegates = new List<IDictionary<string, string>>();

                public void AddDelegate(IDictionary<string, string> dictionary)
                {
                    Delegates.Add(dictionary);
                    foreach (KeyValuePair<string, string> pair in inner)
                    {
                        dictionary.Add(pair);
                    }
                }

                public void Add(KeyValuePair<string, string> item)
                {
                    inner.Add(item);
                    foreach (IDictionary<string, string> dictionary in Delegates)
                    {
                        dictionary.Add(item);
                    }
                }

                public void Clear()
                {
                    inner.Clear();
                    foreach (IDictionary<string, string> dictionary in Delegates)
                    {
                        dictionary.Clear();
                    }
                }

                public bool Remove(KeyValuePair<string, string> item)
                {
                    foreach (IDictionary<string, string> dictionary in Delegates)
                    {
                        dictionary.Remove(item);
                    }
                    return inner.Remove(item);
                }

                public void Add(string key, string value)
                {
                    inner.Add(key, value);
                    foreach (IDictionary<string, string> dictionary in Delegates)
                    {
                        dictionary.Add(key, value);
                    }
                }

                public bool Remove(string key)
                {
                    foreach (IDictionary<string, string> dictionary in Delegates)
                    {
                        dictionary.Remove(key);
                    }
                    return inner.Remove(key);
                }

                public string this[string key]
                {
                    get { return inner[key]; }
                    set
                    {
                        inner[key] = value;
                        foreach (IDictionary<string, string> dictionary in Delegates)
                        {
                            dictionary[key] = value;
                        }
                    }
                }

                #region Delegating to inner methods
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }

                public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
                {
                    return inner.GetEnumerator();
                }

                public bool Contains(KeyValuePair<string, string> item)
                {
                    return inner.Contains(item);
                }

                public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
                {
                    inner.CopyTo(array, arrayIndex);
                }

                public int Count
                {
                    get { return inner.Count; }
                }

                public bool IsReadOnly
                {
                    get { return inner.IsReadOnly; }
                }

                public bool ContainsKey(string key)
                {
                    return inner.ContainsKey(key);
                }

                public bool TryGetValue(string key, out string value)
                {
                    return inner.TryGetValue(key, out value);
                }

                public ICollection<string> Keys
                {
                    get { return inner.Keys; }
                }

                public ICollection<string> Values
                {
                    get { return inner.Values; }
                }
                #endregion
            }
        }
    }

}
