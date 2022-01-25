using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类相关配置
    /// </summary>
    [CreateAssetMenu(fileName = "NonsensicalManagerConfigData", menuName = "ScriptableObjects/NonsensicalManagerConfigData")]
    public class NonsensicalManagerConfigData : NonsensicalConfigDataBase
    {
        public string AssetBundlesPath = "AssetBundles";

        public LogLevel EditorLogLevel = LogLevel.DEBUG;
        public LogLevel BuildLogLevel = LogLevel.OFF;
        public LogPath[] EditorLogPaths = new LogPath[] { LogPath.Console };
        public LogPath[] BuildLogPaths = new LogPath[0];
        public bool EditorLogDateTime = false;
        public bool BuildLogDateTime = true;
        public bool BuildLogClassInfo = false;

        public string LogFilePath = "NonsensicalLog";

        public override void CopyForm<T>(T from)
        {
            NonsensicalManagerConfigData fromData = from as NonsensicalManagerConfigData;
            if (fromData != null)
            {
                AssetBundlesPath = fromData.AssetBundlesPath;
                EditorLogLevel = fromData.EditorLogLevel;
                BuildLogLevel = fromData.BuildLogLevel;
                EditorLogPaths = fromData.EditorLogPaths;
                BuildLogPaths = fromData.BuildLogPaths;
                EditorLogDateTime = fromData.EditorLogDateTime;
                BuildLogDateTime = fromData.BuildLogDateTime;
                BuildLogClassInfo = fromData.BuildLogClassInfo;
                LogFilePath = fromData.LogFilePath;

            }
        }
    }
}

