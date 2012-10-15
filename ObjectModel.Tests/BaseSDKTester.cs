using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.Filters;
using VersionOne.SDK.Utility;

namespace VersionOne.SDK.ObjectModel.Tests {
    public abstract class BaseSDKTester {
        private Oid defaultSchemeOid;
        private EntityFactory entityFactory;
        private V1Instance instance;
        
		protected virtual string ApplicationPath
		{
			get
			{
				const string settingName = "TEST_URL";
				var url = System.Environment.GetEnvironmentVariable(settingName);
				if (string.IsNullOrWhiteSpace(url))
				{
					url = System.Configuration.ConfigurationManager.AppSettings[settingName];
					if (string.IsNullOrWhiteSpace(url))
					{
						url = "http://localhost/V1SDKTests/";
					}
				}

				url = url.Trim();

				return url;
			}
		}
        		
        protected virtual string Username {
            get { return System.Environment.GetEnvironmentVariable("TEST_USER") ?? "admin"; }
        }

        protected virtual string Password {
			get { return System.Environment.GetEnvironmentVariable("TEST_PASSWORD") ?? "admin"; }
        }

        protected Oid DefaultSchemeOid {
            get { return defaultSchemeOid ?? (defaultSchemeOid = GetFirstAvailableScheme().Oid); }
                }

        internal EntityFactory EntityFactory {
            get { return entityFactory ?? (entityFactory = new EntityFactory(Instance)); }
            }

        protected V1Instance Instance {
            get {
                if (instance == null) {
                    instance = new V1Instance(ApplicationPath, Username, Password);
                    instance.Validate();
	            }

                return instance;
	        }
	    }

		#region Sandbox
		/// <summary>
		/// The name to be used when creating your sandbox projects and teams.  Override to specify a special name.  I like to call mine Fred.
		/// </summary>
        protected virtual string SandboxName {
            get { return GetType().Name; }
        }

        #region Schedule
        private AssetID sandboxScheduleID;

        /// <summary>
        /// The ID of your sandbox Schedule, so you can get it again yourself, Elvis.
        /// </summary>
        protected AssetID SandboxScheduleID {
            get {
                if (sandboxScheduleID == null) {
                    var sandbox = CreateSandboxSchedule();
                    sandboxScheduleID = sandbox.ID;
                }

                return sandboxScheduleID;
            }
                }

        /// <summary>
        /// A sandbox for you to play in.  The Entity is retrieved from the Instance on every call (so ResetInstance will force a re-query).  You don't need to do anything to initialize it.  Just use it, Mort.
        /// </summary>
        protected Schedule SandboxSchedule {
            get { return Instance.Get.ScheduleByID(SandboxScheduleID); }
            }

        protected void NewSandboxSchedule() {
            sandboxScheduleID = null;
        }

        /// <summary>
        /// Override to create your sandbox with properties other than the defaults (today as the start date, child of Scope:0, no schedule).  You go, Einstein.
        /// </summary>
        /// <returns></returns>
        protected virtual Schedule CreateSandboxSchedule() {
            return Instance.Create.Schedule(SandboxName, TimeSpan.FromDays(14), TimeSpan.FromDays(0) );
        }
        #endregion

        #region Project
        private AssetID sandboxProjectID;

        /// <summary>
        /// The ID of your sandbox project, so you can get it again yourself, Elvis.
        /// </summary>
        protected AssetID SandboxProjectID {
            get {
                if (sandboxProjectID == null) {
                    var rootProject = Instance.Get.ProjectByID("Scope:0");
                    var sandbox = CreateSandboxProject(rootProject);
                    sandboxProjectID = sandbox.ID;
        }

                return sandboxProjectID;
            }
		}

		/// <summary>
        /// A sandbox for you to play in.  The Entity is retrieved from the Instance on every call (so ResetInstance will force a re-query).  You don't need to do anything to initialize it.  Just use it, Mort.
		/// </summary>
        protected Project SandboxProject {
            get { return Instance.Get.ProjectByID(SandboxProjectID); }
        }

        protected void NewSandboxProject() {
            sandboxProjectID = null;
		}

		/// <summary>
		/// Override to create your sandbox with properties other than the defaults (today as the start date, child of Scope:0, no schedule).  You go, Einstein.
		/// </summary>
		/// <param name="rootProject"></param>
		/// <returns></returns>
        protected virtual Project CreateSandboxProject(Project rootProject) {
            var mandatoryAttributes = new Dictionary<string, object>(1) {{"Scheme", DefaultSchemeOid}};
			return Instance.Create.Project(SandboxName, rootProject, DateTime.Now, null, mandatoryAttributes);
		}
		#endregion

		#region Iteration
        private AssetID sandboxIterationID;

		/// <summary>
		/// The ID of your sandbox iteration, so you can get it again yourself, Elvis.
		/// </summary>
        protected AssetID SandboxIterationID {
            get { return sandboxIterationID ?? (sandboxIterationID = SandboxProject.CreateIteration().ID); }
			}

		/// <summary>
		/// A sandbox for you to play in.  The Entity is retrieved from the Instance on every call (so ResetInstance will force a re-query).  You don't need to do anything to initialize it.  Just use it, Mort.
		/// </summary>
        protected Iteration SandboxIteration {
			get { return Instance.Get.IterationByID(SandboxIterationID); }
		}

        protected void NewSandboxIteration() {
            sandboxIterationID = null;
        }
		#endregion

		#region Team
        private AssetID sandboxTeamID;

		/// <summary>
		/// The ID of your sandbox team, so you can get it again yourself, Elvis.
		/// </summary>
        protected AssetID SandboxTeamID {
            get {
                if (sandboxTeamID == null) {
                    var sanboxTeam = EntityFactory.Create(() => Instance.Create.Team(SandboxName));
                    sandboxTeamID = sanboxTeam.ID;
                }

                return sandboxTeamID;
			}
		}

		/// <summary>
		/// A sandbox for you to play in.  The Entity is retrieved from the Instance on every call (so ResetInstance will force a re-query).  You don't need to do anything to initialize it.  Just use it, Mort.
		/// </summary>
        protected Team SandboxTeam {
			get { return Instance.Get.TeamByID(SandboxTeamID); }
		}

        protected void NewSandboxTeam() {
            sandboxTeamID = null;
        }
		#endregion

		#region Member
        private AssetID sandboxMemberID;

		/// <summary>
		/// The ID of your sandbox member, so you can get it again yourself, Elvis.
		/// </summary>
        protected AssetID SandboxMemberID {
            get { return sandboxMemberID ?? (sandboxMemberID = Instance.Create.Member(SandboxName, SandboxName).ID); }
		}

		/// <summary>
		/// A sandbox for you to play in.  The Entity is retrieved from the Instance on every call (so ResetInstance will force a re-query).  You don't need to do anything to initialize it.  Just use it, Mort.
		/// </summary>
        protected Member SandboxMember {
			get { return Instance.Get.MemberByID(SandboxMemberID); }
		}

        protected void NewSandboxMember() {
            sandboxMemberID = null;
        }
		#endregion

        protected Story CreateStory(string name, Project project, Iteration iteration) {
            var story = EntityFactory.CreateStory(name, project);
            story.Iteration = iteration;
            story.Save();
            return story;
        }

        protected Defect CreateDefect(string name, Project project, Iteration iteration) {
            var defect = EntityFactory.CreateDefect(name, project);
            defect.Iteration = iteration;
            defect.Save();
            return defect;
        }
        #endregion

        protected Environment CreateEnvironment(string name, IDictionary<string, object> attributes) {
            //return Instance.Create.Environment(name, SandboxProject, attributes);
            return EntityFactory.CreateEnvironment(name, SandboxProject, attributes);
        }

        protected void ResetInstance() {
            instance = null;
		}

        protected static bool FindRelated<T>(T needle, IEnumerable<T> haystack) {
            return haystack.Contains(needle);
		}

        internal static T First<T>(IEnumerable<T> list) {
            var enumerator = list.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : default(T);
		}

        private Asset GetFirstAvailableScheme() {
            var schemaType = Instance.ApiClient.MetaModel.GetAssetType("Scheme");
            var nameDefinition = schemaType.GetAttributeDefinition("Name");

            var schemaQuery = new Query(schemaType);
            schemaQuery.Selection.Add(nameDefinition);
            
            var result = Instance.ApiClient.Services.Retrieve(schemaQuery);
            return result.Assets[0];
        }

        internal Environment GetEnvironment() {
            const string name = "Testing env abv123";
            var filter = new EnvironmentFilter();
            filter.Name.Add(name);
            
            var env = Instance.Get.Environments(filter);

            if (env.Count == 0) {
                return CreateEnvironment(name, null);
            }

            var environments = new List<Environment>(env);
            return environments[0];
        }

        [TearDown]
        public void TearDown() {
            EntityFactory.Dispose();
        }
        protected class EntityToAssetIDTransformer<T> : ITransformer where T : Entity {
            public object Transform(object input) {
                return ((T)input).ID.Token;
            }
        }

        protected class EntityToNameTransformer<T> : ITransformer where T : BaseAsset {
            public object Transform(object input) {
                return ((T)input).Name;
            }
        }
	    public IEnumerable DeriveListOfNamesFromAssets(IEnumerable baseAssets)
	    {
	        var names = new List<string> {};
	        foreach (BaseAsset asset in baseAssets)
	        {
	            names.Add(asset.Name);
	        }
	        return names;
	    }

	    public IEnumerable DeriveListOfIdsFromAssets(IEnumerable assets)
	    {
	        var ids = new List<string> {};
	        foreach (BaseAsset asset in assets)
	        {
	            ids.Add(asset.ID.Token);
	        }
	        return ids;
	    }
	}
}
