using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 只适用于所有ab包都在本地同一个文件夹内的情况
    /// </summary>
    public class AssetBundleManager : NonsensicalManagerBase<AssetBundleManager>
    {
        private string assetBundlePath;

        private Dictionary<string, AssetBundleInfo> assstBundleDic;

        private AssetBundleManifest assetBundleManifest;

        protected override void InitStart()
        {
            InitComplete();
        }

        protected override void LateInitStart()
        {
            if (AppConfigManager.Instance != null)
            {
                if (AppConfigManager.Instance.TryGetConfig(out NonsensicalConfigDataTemplate t))
                {
                    assetBundlePath = Path.Combine(Application.streamingAssetsPath, t.AssetBundlesPath);

                    NonsensicalUnityInstance.Instance.StartCoroutine(InitAssetBundleManager(Path.Combine(assetBundlePath, "AssetBundles")));
                }
                else
                {
                    LateInitComplete();
                }
            }
            else
            {
                assetBundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
                NonsensicalUnityInstance.Instance.StartCoroutine(InitAssetBundleManager(Path.Combine(assetBundlePath, "AssetBundles")));
            }

        }

        /// <summary>
        /// 初始化管理类
        /// </summary>
        /// <param name="assetBundleManifestBundlePath"></param>
        private IEnumerator InitAssetBundleManager(string assetBundleManifestBundlePath)
        {
            //if (File.Exists( assetBundleManifestBundlePath)==false)
            //{
            //    LateInitComplete();
            //    yield break;
            //}

            var assetBundleCreateRequest = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleManifestBundlePath);
            yield return assetBundleCreateRequest.SendWebRequest();
            AssetBundle assetBundle = (assetBundleCreateRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

            if (assetBundle == null)
            {
                Debug.LogWarning("未找到AssetBundles");
                LateInitComplete();
                yield break;
            }

            assstBundleDic = new Dictionary<string, AssetBundleInfo>();

            var manifestRequest = assetBundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
            yield return manifestRequest;
            assetBundleManifest = manifestRequest.asset as AssetBundleManifest;

            string[] bundles = assetBundleManifest.GetAllAssetBundlesWithVariant();

            foreach (var item in bundles)
            {
                assstBundleDic.Add(item, new AssetBundleInfo(item, assetBundleManifest.GetDirectDependencies(item)));
            }

            LateInitComplete();
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="onComplete"></param>
        /// <param name="onLoading"></param>
        public void LoadAssetBundle(string bundleName, Action onComplete, Action<float> onLoading = null)
        {
            if (assstBundleDic.ContainsKey(bundleName) == false)
            {
                Debug.LogWarning($"错误的包名{bundleName}");
                return;
            }
            if (assstBundleDic[bundleName].AssetBundlePack == true)
            {
                onComplete?.Invoke();
                return;
            }
            if (assstBundleDic[bundleName].Loading == false)
            {
                NonsensicalUnityInstance.Instance.StartCoroutine(LoadAssetBundleCoroutine(bundleName, onComplete, onLoading));
            }
        }

        /// <summary>
        /// 加载Ab包协程
        /// </summary>
        /// <param name="_bundleName"></param>
        /// <param name="_onComplete"></param>
        /// <param name="_onLoading"></param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundleCoroutine(string _bundleName, Action _onComplete, Action<float> _onLoading)
        {
            assstBundleDic[_bundleName].Loading = true;
            string[] dependencies = assstBundleDic[_bundleName].Dependencies;
            int completeCount = 0;
            foreach (var item in dependencies)
            {
                assstBundleDic[item].DependencieCount++;
                LoadAssetBundle(item, () => { completeCount++; });
            }
            while (completeCount < dependencies.Length)
            {
                yield return null;
            }

            string bundlePath = Path.Combine(assetBundlePath, _bundleName);

            var request = UnityWebRequestAssetBundle.GetAssetBundle(bundlePath);

            if (_onLoading == null)
            {
                yield return request.SendWebRequest();
            }
            else
            {
                yield return request.SendWebRequest();
                do
                {
                    yield return null;

                    _onLoading(request.downloadProgress);
                }
                while (request.downloadProgress < 1);
            }

            AssetBundle assetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

            if (assetBundle != null)
            {
                assstBundleDic[_bundleName].AssetBundlePack = assetBundle;
                assstBundleDic[_bundleName].Loading = false;
                assstBundleDic[_bundleName].OnLoadComplete?.Invoke();
                assstBundleDic[_bundleName].OnLoadComplete = null;
            }
            else
            {
                Debug.LogError($"AB包加载失败，路径：{bundlePath}");
            }
            _onComplete?.Invoke();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesName">资源名</param>
        /// <param name="bundleName">包名</param>
        /// <param name="onComplete">完成回调</param>
        public void LoadResource<T>(string resourcesName, string bundleName, Action<T> onComplete, Action<float> onLoad = null) where T : UnityEngine.Object
        {
            if (assstBundleDic.ContainsKey(bundleName) == false)
            {
                Debug.LogWarning($"错误的包名{bundleName}");
                return;
            }

            if (assstBundleDic[bundleName].AssetBundlePack != null)
            {
                assstBundleDic[bundleName].LoadCount++;
                NonsensicalUnityInstance.Instance.StartCoroutine(LoadResourceCoroutine<T>(resourcesName, assstBundleDic[bundleName].AssetBundlePack, onComplete, onLoad));
            }
            else
            {
                assstBundleDic[bundleName].OnLoadComplete += () =>
                {
                    assstBundleDic[bundleName].LoadCount++;
                    NonsensicalUnityInstance.Instance.StartCoroutine(LoadResourceCoroutine<T>(resourcesName, assstBundleDic[bundleName].AssetBundlePack, onComplete, onLoad));
                };
                LoadAssetBundle(bundleName, null);
            }
        }

        /// <summary>
        /// 加载资源协程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesNameOrPath"></param>
        /// <param name="assetBundle"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator LoadResourceCoroutine<T>(string resourcesNameOrPath, AssetBundle assetBundle, Action<T> onComplete, Action<float> onLoad) where T : UnityEngine.Object
        {
            AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync<T>(resourcesNameOrPath);

            if (onLoad == null)
            {
                yield return assetBundleRequest;
            }
            else
            {
                do
                {
                    yield return null;

                    onLoad(assetBundleRequest.progress);

                } while (!assetBundleRequest.isDone);
            }

            if (assetBundleRequest.asset != null)
            {
                T Object = assetBundleRequest.asset as T;
                onComplete(Object);
            }
            else
            {
                Debug.LogError($"未加载到对象:{resourcesNameOrPath}");
            }
        }

        public void ReleaseAsset(string bundleName)
        {
            assstBundleDic[bundleName].LoadCount--;
        }

        private class AssetBundleInfo
        {
            public string BundleName;   //包的名称
            public string[] Dependencies;       //直接依赖的包名称

            public AssetBundle AssetBundlePack; //ab包
            public int LoadCount = 0;           //包内对象加载的次数
            public int DependencieCount = 0;    //被依赖加载的次数
            public bool Loading;                //是否正在进行加载
            public Action OnLoadComplete;

            public AssetBundleInfo(string _bundleName, string[] _dependencies)
            {
                this.BundleName = _bundleName;
                this.Dependencies = _dependencies;
            }
        }
    }
}
