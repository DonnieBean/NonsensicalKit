using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// �������������
    /// </summary>
    [CreateAssetMenu(fileName = "NonsensicalConfigData", menuName = "ScriptableObjects/NonsensicalConfigData")]
    public class NonsensicalManagerConfigData : NonsensicalConfigDataBase
    {
        public string AssetBundlesPath = "AssetBundles";

        public LogLevel EditorLogLevel = LogLevel.ALL;
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

