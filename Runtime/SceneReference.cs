#if PACKAGE_ADDRESSABLES
using UnityEngine.AddressableAssets;

namespace GatOR.Logic
{
	[System.Serializable]
	public class SceneReference :
#if UNITY_EDITOR
		AssetReferenceT<UnityEditor.SceneAsset>
#else
		AssetReference
#endif
	{
		public SceneReference(string guid) : base(guid)
		{
		}
	}
}
#endif
