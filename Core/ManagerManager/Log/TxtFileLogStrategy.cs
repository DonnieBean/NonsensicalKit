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
        string path; 
        string name;
        FileStream fs;
        StreamWriter sw;
        public void Init()
        {
            string usePath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOfAny( new char[] { '/' ,'\\'}));
            if (AppConfigManager.Instance!=null&&AppConfigManager.Instance.TryGetConfig<ManagerConfigData>(out var v))
            {
                fullLogFilePath = Path.Combine(usePath, v.LogFilePath,$"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            else
            {
                fullLogFilePath = Path.Combine(usePath, "NonsensicalLog", $"Log{ DateTime.Now.ToString("yyyy_MM_dd_HH")}.txt");
            }
            path = Path.GetDirectoryName(fullLogFilePath);
            name = Path.GetFileName(fullLogFilePath);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                fs = new FileStream(fullLogFilePath, FileMode.Append, FileAccess.Write, FileShare.Write);
                sw = new StreamWriter(fs, Encoding.UTF8);
            }
            catch (Exception)
            {
               Debug.Log("流创建失败");
            }
        }

        public void Log(LogContext info)
        {
            try
            {
                sw.Write(info.message);
                sw.Write("\r\n");
                sw.Flush();
                fs.Flush();
            }
            catch (Exception)
            {
                LogManager.Instance.Log("文件写入错误");
            }
        }

        public void Recycle()
        {
            sw?.Flush();
            fs?.Flush();
            sw?.Close();
            fs?.Close();
        }
    }
}
