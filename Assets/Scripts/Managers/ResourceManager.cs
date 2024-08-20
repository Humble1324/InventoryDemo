using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public void GetAssetCache<T>(string assetPath, Action<T> onLoad) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            // if (AssetBundleConfig.IsEditorMode)
            StartCoroutine(GetAsset<T>(assetPath, onLoad));
#endif
            // return AssetBundleManager.Instance.GetAssetCache(name) as T;
        }

        private IEnumerator GetAsset<T>(string assetPath, Action<T> onLoad) where T : UnityEngine.Object
        {
            {
                ResourceRequest request = Resources.LoadAsync<T>(assetPath);
                yield return request;
                if (request.asset == null)
                {
                    print("Load Image Error");
                    onLoad?.Invoke(null);
                }
                else
                {
                    T obj = request.asset as T;
                    onLoad?.Invoke(obj);
                }
            }
        }
    }
}