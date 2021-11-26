using NonsensicalKit.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NonsensicalKit.Manager
{
    /// <summary>
    /// 项目内路径的txt文件输出策略
    /// </summary>
    public class TxtFileLogStrategy:LogStrategy
    {
        string fullLogFilePath;
        public void Init()
        {
            if (AppConfigManager.Instance.TryGetConfig<NonsensicalManagerConfigData>(out var v))
            {
                fullLogFilePath = Path.Combine(Application.dataPath , v.LogFilePath,$"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            else
            {
                fullLogFilePath = Path.Combine(Application.dataPath, "NonsensicalLog", $"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            Debug.Log("日志文件路径："+fullLogFilePath);
        }

        public  void Log(LogContext info)
        {
            FileHelper.FileAppendWrite(fullLogFilePath, info.message);
        }
    }
}
