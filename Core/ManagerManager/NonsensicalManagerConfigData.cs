using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类相关配置
    /// </summary>
    [CreateAssetMenu(fileName = "NonsensicalConfigData", menuName = "ScriptableObjects/NonsensicalConfigData")]
    public class NonsensicalManagerConfigData : NonsensicalConfigDataBase
    {
        public string AssetBundlesPath = "AssetBundles";

        public LogLevel EditorLogLevel=LogLevel.ALL;
        public LogLevel BuildLogLevel = LogLevel.OFF;
        public LogPath[] EditorLogPaths = new LogPath[] { LogPath.Console };
        public LogPath[] BuildLogPaths=new LogPath[0];
        public bool EditorLogDateTime=false;
        public bool BuildLogDateTime=true;
        public bool BuildLogClassInfo = false;

        public string LogFilePath = "NonsensicalLog";
    }
}

