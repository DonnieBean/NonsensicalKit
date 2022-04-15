using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 管理类相关配置
    /// </summary>
    [CreateAssetMenu(fileName = "NonsensicalManagerConfigData", menuName = "ScriptableObjects/NonsensicalManagerConfigData")]
    public class NonsensicalManagerConfigData : NonsensicalConfigDataBase
    {
        public ManagerConfigData managerConfigData;

        public override ConfigDataBase GetData()
        {
            return managerConfigData;
        }

        public override void SetData(ConfigDataBase cd)
        {
            if (CheckType<ManagerConfigData>(cd))
            {
                managerConfigData = cd as ManagerConfigData;
            }
        }
    }

    [System.Serializable]
    public class ManagerConfigData:ConfigDataBase
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
    }
    
}

