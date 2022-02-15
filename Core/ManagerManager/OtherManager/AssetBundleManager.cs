using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 只适用于所有ab包都在本地同一个文件夹内的情况
    /// 不支持变体（Variant）
    /// 无法处理互相依赖的包关系（A包依赖B包，同时B包依赖A包）
    /// </summary>
    public class AssetBundleManager : NonsensicalManagerBase<AssetBundleManager>
    {
        public bool isLoading => LoadCount > 0;

        private int LoadCount
        {
            get
            {
                return loadCount;
            }
            set
            {
                loadCount = value;
                OnLoadCountChanged();
            }
        }
        private int loadCount;

        private string assetBundlePath;

        private Dictionary<string, AssetBundleInfo> assstBundleDic = new Dictionary<string, AssetBundleInfo>();

        private AssetBundleManifest assetBundleManifest;

        protected override void Awake()
        {
            base.Awake();

            InitSubscribe(2, OnInitStart());
        }

        protected IEnumerator  OnInitStart()
        {
            if (AppConfigManager.Instance != null && AppConfigManager.Instance.TryGetConfig(out ManagerConfigData t))
            {
                assetBundlePath = Path.Combine(Application.streamingAssetsPath, t.AssetBundlesPath);

                yield return  StartCoroutine(InitAssetBundleManager(Path.Combine(assetBundlePath, "AssetBundles")));
            }
            else
            {
                assetBundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
                yield return StartCoroutine(InitAssetBundleManager(Path.Combine(assetBundlePath, "AssetBundles")));
            }
        }

        private void OnLoadCountChanged()
        {
            Publish((int)NonsensicalManagerEnum.ABLoadCountChanged, isLoading);
            Publish("ABLoadCountChanged", isLoading);
        }

        /// <summary>
        /// 初始化管理类
        /// </summary>
        /// <param name="assetBundleManifestBundlePath"></param>
        private IEnumerator InitAssetBundleManager(string assetBundleManifestBundlePath)
        {
            var assetBundleCreateRequest = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleManifestBundlePath);
            yield return assetBundleCreateRequest.SendWebRequest();
            AssetBundle assetBundle = (assetBundleCreateRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

            if (assetBundle == null)
            {
                LogManager.Instance.LogWarning("未找到AssetBundles");
                yield break;
            }

            var manifestRequest = assetBundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
            yield return manifestRequest;
            assetBundleManifest = manifestRequest.asset as AssetBundleManifest;

            string[] bundles = assetBundleManifest.GetAllAssetBundles();

            foreach (var item in bundles)
            {
#if UNITY_EDITOR
                //检测互相依赖
                foreach (var item2 in assetBundleManifest.GetDirectDependencies(item))
                {
                    if (assstBundleDic.ContainsKey(item2))
                    {
                        if (assstBundleDic[item2].Dependencies.Contains(item))
                        {
                            Debug.LogError($"{item}和{item2}互相依赖");
                        }
                    }
                }
#endif

                assstBundleDic.Add(item, new AssetBundleInfo(item, assetBundleManifest.GetDirectDependencies(item)));
            }
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="onComplete"></param>
        /// <param name="onLoading"></param>
        public void LoadAssetBundle(string bundleName, Action onComplete, Action<float> onLoading = null)
        {
            //Debug.Log("加载ab包：" + bundleName);
            if (assstBundleDic.ContainsKey(bundleName) == false)
            {
                Debug.LogWarning($"错误的包名：{bundleName}");
                return;
            }

            if (assstBundleDic[bundleName].AssetBundlePack != null)
            {
                onComplete?.Invoke();
                return;
            }

            if (assstBundleDic[bundleName].Loading == false)
            {
                StartCoroutine(LoadAssetBundleCoroutine(bundleName, onComplete, onLoading));
            }
            else
            {
                assstBundleDic[bundleName].OnLoadComplete += onComplete;
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
            LoadCount++;
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
                LoadCount--;
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
                StartCoroutine(LoadResourceCoroutine<T>(resourcesName, assstBundleDic[bundleName].AssetBundlePack, onComplete, onLoad));
            }
            else
            {
                assstBundleDic[bundleName].OnLoadComplete += () =>
                {
                    assstBundleDic[bundleName].LoadCount++;
                    StartCoroutine(LoadResourceCoroutine<T>(resourcesName, assstBundleDic[bundleName].AssetBundlePack, onComplete, onLoad));
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
            //此处无法正常使用，需要将依赖一起处理
            assstBundleDic[bundleName].LoadCount--;
        }

        public void UnloadBundle(string bundleName, bool unloadAllObjects = false)
        {
            assstBundleDic[bundleName]?.AssetBundlePack?.Unload(unloadAllObjects);
        }

        public void ReleaseUnusedAssetBundle()
        {
            List<string> releaseNames = new List<string>();
            foreach (var item in assstBundleDic)
            {
                if (item.Value.LoadCount <= 0)
                {
                    releaseNames.Add(item.Key);
                }
            }

            foreach (var item in releaseNames)
            {
                assstBundleDic[item].AssetBundlePack?.Unload(true); assstBundleDic[item].AssetBundlePack = null;
            }
        }

        public void ReleaseAllAssetBundle()
        {
            foreach (var item in assstBundleDic)
            {
                item.Value.AssetBundlePack?.Unload(false);
            }
        }
        public void ReleaseAllAssetBundleWithLoadedObject()
        {
            foreach (var item in assstBundleDic)
            {
                item.Value.AssetBundlePack?.Unload(true);
                item.Value.AssetBundlePack = null;
            }
        }

        private class AssetBundleInfo
        {
            public string BundleName;           //包的名称
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
