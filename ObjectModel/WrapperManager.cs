using System;
using System.Collections.Generic;
using System.Reflection;
using VersionOne.SDK.APIClient;
using VersionOne.SDK.ObjectModel.List;
using Attribute=VersionOne.SDK.APIClient.Attribute;

namespace VersionOne.SDK.ObjectModel
{
	internal class WrapperManager
	{
		private readonly V1Instance _instance;
        private readonly IDictionary<IMetaModel, List<IAssetType>> withoutAssetStates = new Dictionary<IMetaModel, List<IAssetType>>();

		internal WrapperManager(V1Instance instance)
		{
			_instance = instance;
		}

		internal T Create<T>() where T : Entity
		{
			// This assumes that every entity will have a constructor that takes a V1Instance
			return (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _instance }, null);
		}

		internal T Create<T>(AssetID id, bool validate) where T : Entity
		{
			Type targetType = typeof(T) == typeof(CustomListValue) || (validate == false && !typeof(T).IsAbstract) ? typeof(T) : FindType(id, validate);
			if (targetType == null)
				return null;

			// This assumes that every entity will have a constructor that takes an AssetID and a V1Instance
			return (T)Activator.CreateInstance(targetType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { id, _instance }, null);			
		}

		private readonly object _typeMapLock = new object();
		private IDictionary<string, List<Type>> _typeMap; 
		private IDictionary<string, List<Type>> TypeMap
		{
			get
			{
				if (_typeMap == null)
				{
					lock (_typeMapLock)
					{
						if (_typeMap == null)
						{
							IDictionary<string, List<Type>> map = new Dictionary<string, List<Type>>();
							FillMap(map);
							_typeMap = map;
						}
					}
				}
				return _typeMap;
			}
		}

		private static void FillMap(IDictionary<string, List<Type>> map)
		{
			Type entityType = typeof(Entity);
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types)
			{
				if (entityType.IsAssignableFrom(type))
				{
					object[] attributes = type.GetCustomAttributes(typeof (MetaDataAttribute), false);
					if (attributes.Length > 0)
					{
						MetaDataAttribute attribute = (MetaDataAttribute) attributes[0];
						string token = attribute.AssetTypeToken;
						if (!string.IsNullOrEmpty(token))
						{
							List<Type> list;
							if ( !map.TryGetValue(token, out list) )
								map[token] = list = new List<Type>();
							list.Add(type);
						}
					}
				}
			}

			foreach (List<Type> list in map.Values)
			{
				list.Sort(delegate(Type a, Type b) { return Comparer<int?>.Default.Compare(GetAssetStateFilter(a), GetAssetStateFilter(b)); });
				list.Reverse();
			}
		}

		private static byte? GetAssetStateFilter(ICustomAttributeProvider type)
		{
			object[] attributes = type.GetCustomAttributes(typeof(MetaDataAttribute), false);
			MetaDataAttribute attribute = (MetaDataAttribute)attributes[0];
			return attribute.AssetState;
		}

		private Type FindType(AssetID id, bool validate)
		{
			Oid oid = GetOid(id);
			List<Type> list;
			if (TypeMap.TryGetValue(oid.AssetType.Token, out list))
			{
				bool attribChecked = false;
				Attribute attrib = null;
				foreach (Type type in list)
				{
					byte? filterState = GetAssetStateFilter(type);
                    if (filterState == null && (!validate || list.Count == 1)) 
                    {
//                        return ((!validate) || (GetAsset(oid) != null)) ? type : null;
                        if(!validate)
                            return type;
                        Asset asset = GetAsset(oid);
                        if(null != asset)
                        {
                            _instance.SetAsset(id, asset);
                            return type;
                        }
                        return null;
                    }
				    if (!attribChecked)
					{
						attribChecked = true;
						attrib = ResolveAssetState(id, validate);
					}
					if (attrib == null)
						continue;
					byte state = (byte) attrib.Value;
					if (filterState == null || state == filterState)
						return type;
				}
			}
			return null;
		}

		private Attribute ResolveAssetState(AssetID id, bool validate)
		{
			Oid oid = GetOid(id);
            IAttributeDefinition assetStateDef = oid.AssetType.GetAttributeDefinition("AssetState");
			Asset asset = validate ? null : _instance.GetAsset(id);
			if (asset == null)
			{
				asset = GetAsset(oid);
				if (asset == null)
					return null;
				_instance.SetAsset(id, asset);
			}
			Attribute attrib = asset.GetAttribute(assetStateDef);
			if (attrib == null)
				attrib = GetAsset(oid).GetAttribute(assetStateDef);
			return attrib;
		}

		private Oid GetOid(AssetID id)
		{
			return Oid.FromToken(id, _instance.ApiClient.MetaModel);
		}

		private Asset GetAsset(Oid oid)
		{
            CreateAssetStatesStorage();
		    IAttributeDefinition stateAttribute = null;
			Query q = new Query(oid);

            IMetaModel model = _instance.ApiClient.MetaModel;
            if (!withoutAssetStates[model].Contains(q.AssetType) &&
                q.AssetType.TryGetAttributeDefinition("AssetState", out stateAttribute)) 
            {
                q.Selection.Add(stateAttribute);
            }
            if (stateAttribute == null && !withoutAssetStates[model].Contains(q.AssetType)) 
            {
                withoutAssetStates[model].Add(q.AssetType);
            }
			AssetList assets = _instance.ApiClient.Services.Retrieve(q).Assets;
			if (assets.Count == 0)
				return null;
			return assets[0];
		}

	    private void CreateAssetStatesStorage() 
        {
            if (!withoutAssetStates.ContainsKey(_instance.ApiClient.MetaModel))
            {
                IMetaModel meta = _instance.ApiClient.MetaModel;
                try
                {
                    IAssetType assetType = meta.GetAssetType("Actual");
                    withoutAssetStates.Add(meta, new List<IAssetType>(new IAssetType[] { assetType }));
                }
                catch (MetaException)
                {
                    withoutAssetStates.Add(meta, new List<IAssetType>());
                }
            }
	    }
	}
}